using System;

class Program
{
    static void Main()
    {
        int[] numbers = { 8, 3, 15, 1, 10, 3 };

        Console.WriteLine("Масив чисел:");
        PrintArray(numbers);

        Console.WriteLine("iндекс знайденого елемента: " + ArrayHelper.IndexOf(numbers, 15));
        Console.WriteLine("мiнiмальне: " + ArrayHelper.Min(numbers));
        Console.WriteLine("максимальне: " + ArrayHelper.Max(numbers));

        ArrayHelper.Reverse(numbers);

        Console.WriteLine("Пiсля розвороту масиву:");
        PrintArray(numbers);

        string[] words = { "яблуко", "банан", "груша", "апельсин" };

        Console.WriteLine();
        Console.WriteLine("Масив рядкiв:");
        PrintArray(words);

        Console.WriteLine("iндекс знайденого елемента (\"груша\"): " + ArrayHelper.IndexOf(words, "груша"));
        Console.WriteLine("мiнiмальне: " + ArrayHelper.Min(words));
        Console.WriteLine("максимальне : " + ArrayHelper.Max(words));

        ArrayHelper.Reverse(words);

        Console.WriteLine("Пiсля розвороту масиву:");
        PrintArray(words);
    }

    static void PrintArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(array[i]);

            if (i < array.Length - 1)
            {
                Console.Write(", ");
            }
        }

        Console.WriteLine();
    }
}