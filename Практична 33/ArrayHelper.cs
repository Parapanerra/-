using System;

public static class ArrayHelper
{
    public static int IndexOf<T>(T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (object.Equals(array[i], value))
            {
                return i;
            }
        }

        return -1;
    }

    public static void Reverse<T>(T[] array)
    {
        int left = 0;
        int right = array.Length - 1;

        while (left < right)
        {
            T temp = array[left];
            array[left] = array[right];
            array[right] = temp;

            left++;
            right--;
        }
    }

    public static T Min<T>(T[] array) where T : IComparable<T>
    {
        if (array.Length == 0)
        {
            throw new ArgumentException("Масив не може бути порожнім.");
        }

        T min = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i].CompareTo(min) < 0)
            {
                min = array[i];
            }
        }

        return min;
    }

    public static T Max<T>(T[] array) where T : IComparable<T>
    {
        if (array.Length == 0)
        {
            throw new ArgumentException("Масив не може бути порожнім.");
        }

        T max = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i].CompareTo(max) > 0)
            {
                max = array[i];
            }
        }

        return max;
    }
}