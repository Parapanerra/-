using System.Threading;

int[] numbers = new int[20];
Thread[] threads = new Thread[numbers.Length];
FactorialCalculator calculator = new();

for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] = Random.Shared.Next(1, 13);
    int num = numbers[i];

    threads[i] = new Thread(() =>
    {
        long numF = calculator.Calculate(num);
        Console.WriteLine($"Факторіал числа: {num}! = {numF}");
    });

    threads[i].Start();
}

foreach (Thread thread in threads)
{
    thread.Join();
}

Console.WriteLine("Розрахунок закінчено.");

internal sealed class FactorialCalculator
{
    public long Calculate(int number)
    {
        long result = 1;

        for (int i = 2; i <= number; i++)
        {
            result *= i;
        }

        return result;
    }
}
