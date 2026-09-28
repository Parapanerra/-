using System.Text;
using PostService.CommonTypes;
using PostService.DataAccess;
using PostService.Models;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Узагальнений репозиторій сутностей\n");

IRepository<Posting> postings = new GenericRepository<Posting>();
var firstPosting = postings.Add(new Posting
{
    From = "Київ", To = "Львів", Content = "Книга", Weight = 1.5f,
    DeliveryType = DeliveryType.Department, CreatedAt = DateTime.UtcNow
});
var secondPosting = postings.Add(new Posting
{
    From = "Одеса", To = "Дніпро", Content = "Документи", Weight = 0.3f,
    DeliveryType = DeliveryType.Courier, CreatedAt = DateTime.UtcNow
});
Check(postings.GetAll().Count == 2, "Додано два відправлення");
Check(postings.GetById(firstPosting.Id)?.Content == "Книга", "Відправлення знайдено за Id");

var updatedPosting = (Posting)firstPosting.Clone();
updatedPosting.To = "Харків";
Check(postings.Update(updatedPosting) && postings.GetById(firstPosting.Id)?.To == "Харків",
    "Адресу отримувача оновлено");
Check(postings.Delete(secondPosting.Id) && postings.GetById(secondPosting.Id) is null,
    "Друге відправлення видалено");

Console.WriteLine("\nВідправлення, що залишилися:");
foreach (var posting in postings.GetAll())
{
    Console.WriteLine($"Id={posting.Id}: {posting.From} → {posting.To}; {posting.Content}; {posting.Weight} кг");
}

IRepository<ContactPerson> contacts = new GenericRepository<ContactPerson>();
var firstContact = contacts.Add(new ContactPerson
{
    Name = "Олена Коваль", Phone = "+380501112233", Email = "olena@example.com"
});
var secondContact = contacts.Add(new ContactPerson
{
    Name = "Андрій Бондар", Phone = "+380672223344", Email = "andrii@example.com"
});
Console.WriteLine();
Check(contacts.GetAll().Count == 2, "Додано дві контактні особи");
Check(contacts.GetById(firstContact.Id)?.Name == "Олена Коваль", "Контакт знайдено за Id");
var updatedContact = new ContactPerson
{
    Id = firstContact.Id, Name = firstContact.Name,
    Phone = "+380993334455", Email = firstContact.Email
};
Check(contacts.Update(updatedContact) && contacts.GetById(firstContact.Id)?.Phone == "+380993334455",
    "Телефон контакту оновлено");
Check(contacts.Delete(secondContact.Id) && contacts.GetById(secondContact.Id) is null,
    "Другий контакт видалено");

Console.WriteLine("\nКонтактні особи, що залишилися:");
foreach (var contact in contacts.GetAll())
{
    Console.WriteLine($"Id={contact.Id}: {contact.Name}; {contact.Phone}; {contact.Email}");
}

Console.WriteLine();
Check(firstPosting.Id == firstContact.Id, "Різні типи мають незалежні лічильники Id");
Check(postings.GetAll().Count == 1 && postings.GetById(firstPosting.Id)?.To == "Харків"
    && contacts.GetAll().Count == 1 && contacts.GetById(firstContact.Id)?.Name == "Олена Коваль",
    "Операції з контактами не змінили відправлення: дані типів не змішуються");
Check(new GenericRepository<Posting>().GetById(firstPosting.Id) is not null,
    "Інший екземпляр репозиторію того самого типу бачить спільні дані");
Check(!postings.Update(new Posting { Id = int.MaxValue }) && !postings.Delete(int.MaxValue)
    && postings.GetById(int.MaxValue) is null,
    "Відсутнє відправлення коректно обробляється");
Check(!contacts.Update(new ContactPerson { Id = int.MaxValue }) && !contacts.Delete(int.MaxValue)
    && contacts.GetById(int.MaxValue) is null,
    "Відсутній контакт коректно обробляється");

Console.WriteLine("\nУсі перевірки пройдено. Дані зберігалися лише в оперативній пам'яті.");

static void Check(bool condition, string description)
{
    if (!condition) throw new InvalidOperationException($"Перевірку не пройдено: {description}");
    Console.WriteLine($"[OK] {description}");
}
