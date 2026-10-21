// ООП ПР-51. Три потоки працюють зі спільним масивом і списками.
int[] fullArray = new int[1000];
for (int i = 0; i < fullArray.Length; i++)
    fullArray[i] = Random.Shared.Next(1, 11); // Від 1 до 10 включно.

List<int> evenList = new();
List<int> oddList = new();

int nextIndex = 0;
object indexLock = new();
object evenLock = new();
object oddLock = new();

// Усі три потоки виконують одну й ту саму функцію.
void DistributeNumbers()
{
    while (true)
    {
        int number;
        lock (indexLock)
        {
            // Перевірка межі та отримання наступного елемента неподільні.
            if (nextIndex >= fullArray.Length)
                return;

            number = fullArray[nextIndex];
            nextIndex++;
        }

        // List<T> не є потокобезпечним, тому кожен список має власний замок.
        if (number % 2 == 0)
        {
            lock (evenLock)
                evenList.Add(number);
        }
        else
        {
            lock (oddLock)
                oddList.Add(number);
        }
    }
}

Thread[] threads = new Thread[3];
for (int i = 0; i < threads.Length; i++)
{
    threads[i] = new Thread(DistributeNumbers);
    threads[i].Start();
}

// Читати результати можна лише після завершення всіх робочих потоків.
foreach (Thread thread in threads)
    thread.Join();

// Перевіряємо не лише кількість, а й збереження кожного значення масиву.
if (evenList.Count + oddList.Count != fullArray.Length
    || evenList.Any(number => number % 2 != 0)
    || oddList.Any(number => number % 2 == 0)
    || !fullArray.Order().SequenceEqual(evenList.Concat(oddList).Order()))
    throw new InvalidOperationException("Помилка розподілу: дані втрачено, продубльовано або розміщено не в тому списку.");

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine($"Парні елементи ({evenList.Count}):");
Console.WriteLine(string.Join(" ", evenList));
Console.WriteLine();
Console.WriteLine($"Непарні елементи ({oddList.Count}):");
Console.WriteLine(string.Join(" ", oddList));
Console.WriteLine();
Console.WriteLine($"Усього оброблено: {evenList.Count + oddList.Count} елементів у {threads.Length} потоки.");
