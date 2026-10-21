using System.Net;
using System.Text;
using System.Text.Json;
using PostService.App;
using PostService.CommonTypes;
using PostService.Dtos;

var baseUrl = args.FirstOrDefault() ?? "http://localhost:8046/";
var passed = 0;
void Check(bool condition, string name)
{
    if (!condition) throw new Exception("FAIL: " + name);
    Console.WriteLine("PASS: " + name);
    passed++;
}
async Task<T> Throws<T>(Func<Task> action) where T : Exception
{
    try { await action(); }
    catch (T ex) { return ex; }
    throw new Exception("Expected " + typeof(T).Name);
}

using var client = new PostServiceClient(new Uri(baseUrl));
Check(await client.GetPostingsAsync() is not null, "GET list");
var missing = await Throws<HttpRequestException>(() => client.GetPostingByIdAsync(int.MaxValue));
Check(missing.StatusCode == HttpStatusCode.NotFound, "404 retains status");
var validation = await Throws<PostingValidationException>(() => client.CreatePostingAsync(new()));
Check(validation.Errors.ContainsKey("From") && validation.Errors.ContainsKey("Weight"), "400 retains field errors");
var dto = new PostingPostDto { From = "PR46 test", To = "Verification", Content = "Temporary parcel", DeliveryType = DeliveryType.Department, Weight = 1, Width = 10, Height = 10, Depth = 10 };
var created = await client.CreatePostingAsync(dto);
try
{
    Check(created.Id > 0, "POST creates record");
    Check((await client.GetPostingByIdAsync(created.Id)).From == dto.From, "GET by ID");
    var put = new PostingPutDto { Id = created.Id, From = dto.From, To = dto.To, Content = dto.Content, DeliveryType = dto.DeliveryType, Weight = 2, Width = 10, Height = 10, Depth = 10 };
    Check((await client.UpdatePostingAsync(created.Id, put)).Weight == 2, "PUT updates record");
    put.Id++;
    Check((await Throws<PostingValidationException>(() => client.UpdatePostingAsync(created.Id, put))).Errors.ContainsKey("request"), "400 JSON string is supported");
}
finally { await client.DeletePostingAsync(created.Id); }
Check((await Throws<HttpRequestException>(() => client.DeletePostingAsync(created.Id))).StatusCode == HttpStatusCode.NotFound, "DELETE stale record returns 404");
Check((await Throws<HttpRequestException>(() => client.UpdatePostingAsync(created.Id, new PostingPutDto { Id=created.Id, From=dto.From, To=dto.To, Content=dto.Content, DeliveryType=dto.DeliveryType, Weight=1, Width=10, Height=10, Depth=10 }))).StatusCode == HttpStatusCode.NotFound, "PUT stale record returns 404");

foreach (var status in new[] { 500, 503 })
{
    using var http = new HttpClient { BaseAddress = new Uri(baseUrl) };
    http.DefaultRequestHeaders.Add("X-PR46-Fault", status.ToString());
    using var faulty = new PostServiceClient(http);
    Check((int?)(await Throws<HttpRequestException>(() => faulty.GetPostingsAsync())).StatusCode == status, $"Real server {status} retains status");
}
using (var http = new HttpClient(new StubHandler(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("broken json", Encoding.UTF8, "application/json") })) { BaseAddress = new Uri(baseUrl) })
using (var malformed = new PostServiceClient(http))
    Check(await Throws<JsonException>(() => malformed.GetPostingsAsync()) is not null, "Malformed success JSON propagates");
using (var http = new HttpClient(new StubHandler(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("null", Encoding.UTF8, "application/json") })) { BaseAddress = new Uri(baseUrl) })
using (var empty = new PostServiceClient(http))
    Check(await Throws<JsonException>(() => empty.GetPostingsAsync()) is not null, "Null success JSON propagates");
using (var http = new HttpClient(new StubHandler(() => throw new TaskCanceledException("timeout", new TimeoutException()))) { BaseAddress = new Uri(baseUrl) })
using (var timeout = new PostServiceClient(http))
    Check((await Throws<HttpRequestException>(() => timeout.GetPostingsAsync())).StatusCode is null, "Timeout becomes retryable transport error");
// Bind a port without listening so no unrelated server can answer the unavailable-server check.
using (var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
{
    socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
    using var offline = new PostServiceClient(new Uri($"http://127.0.0.1:{((IPEndPoint)socket.LocalEndPoint!).Port}/"));
    Check((await Throws<HttpRequestException>(() => offline.GetPostingsAsync())).StatusCode is null, "Unavailable server has no HTTP status");
}
Console.WriteLine($"Completed: {passed} checks passed.");

sealed class StubHandler(Func<HttpResponseMessage> respond) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(respond());
}
