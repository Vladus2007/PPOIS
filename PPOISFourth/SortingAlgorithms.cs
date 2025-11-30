using System;
using System.Collections.Generic;

namespace SortingAlgorithms
{
    public static class HeapSort
    {
        public static void Sort<T>(T[] array) where T : IComparable<T>
        {
            int n = array.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                Swap(array, 0, i);
                Heapify(array, i, 0);
            }
        }

        public static void Sort<T>(List<T> list) where T : IComparable<T>
        {
            var array = list.ToArray();
            Sort(array);
            list.Clear();
            list.AddRange(array);
        }

        private static void Heapify<T>(T[] array, int n, int i) where T : IComparable<T>
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && array[left].CompareTo(array[largest]) > 0)
                largest = left;

            if (right < n && array[right].CompareTo(array[largest]) > 0)
                largest = right;

            if (largest != i)
            {
                Swap(array, i, largest);
                Heapify(array, n, largest);
            }
        }

        private static void Swap<T>(T[] array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    public static class StoogeSort
    {
        public static void Sort<T>(T[] array) where T : IComparable<T>
        {
            if (array.Length > 0)
                Sort(array, 0, array.Length - 1);
        }

        public static void Sort<T>(List<T> list) where T : IComparable<T>
        {
            var array = list.ToArray();
            Sort(array);
            list.Clear();
            list.AddRange(array);
        }

        private static void Sort<T>(T[] array, int l, int r) where T : IComparable<T>
        {
            if (l >= r) return;

            if (array[l].CompareTo(array[r]) > 0)
                Swap(array, l, r);

            if (r - l + 1 > 2)
            {
                int t = (r - l + 1) / 3;
                Sort(array, l, r - t);
                Sort(array, l + t, r);
                Sort(array, l, r - t);
            }
        }

        private static void Swap<T>(T[] array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}