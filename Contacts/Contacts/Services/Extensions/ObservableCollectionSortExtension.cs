using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Contacts.Services.Extensions
{
    public static class ObservableCollectionSortExtension
    {
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }

        public static int BinarySearh<T>(this ObservableCollection<T> collection, T itemToFind, Comparison<T> comparison)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            int lower = 0;
            int upper = collection.Count - 1;

            while (lower <= upper)
            {
                int middle = lower + (upper - lower) / 2;
                int comparisonResult = comparison.Invoke(itemToFind, collection[middle]);
                if (comparisonResult == 0)
                {
                    return middle;
                }
                else if (comparisonResult < 0)
                {
                    upper = middle - 1;
                }
                else
                {
                    lower = middle + 1;
                }
            }

            return lower;
        }
    }
}