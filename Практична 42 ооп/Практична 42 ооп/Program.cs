using System;
using System.Text;

namespace Практична_42_ооп;

internal class Program
{
    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        bool success = false;

        while (!success)
        {
            try
            {
                Console.Write("Введіть ціле число: ");
                string? input = Console.ReadLine();
                int number = Int32.Parse(input!);
                Console.WriteLine($"Ви ввели: {number}");
                success = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Введене значення не є числом.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Значення виходить за межі допустимого діапазону Int32.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося перетворити значення. {ex.GetType().Name}: {ex.Message}");
            }

            if (!success)
            {
                Console.WriteLine("Спробуйте ввести число знову.\n");
            }
        }

        if (!Console.IsInputRedirected)
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб закрити програму.");
            Console.ReadKey(true);
        }
    }
}
