using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain
{
    public static class ListExtensions
    {
        /// <summary>
        /// Checks if the List<string> is null or empty.
        /// </summary>
        /// <param name="list">The List<string> to check.</param>
        /// <returns>True if the list is null or contains no elements; otherwise, false.</returns>
        public static bool IsNullOrEmpty(this List<string> list)
        {
            return list == null || list.Count == 0;
        }

        public static string ToCommaSeparatedString(this List<string> list, bool includeNullOrEmptyStrings = false)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }

            // If we don't want to include null or empty strings, filter them out first
            if (!includeNullOrEmptyStrings)
            {
                // Using LINQ's Where to filter out null or empty strings
                IEnumerable<string> filteredList = list.Where(s => !string.IsNullOrEmpty(s));
                return string.Join(",", filteredList);
            }
            else
            {
                // If including null/empty strings, ensure nulls are represented as empty strings for Join
                // This prevents "System.String,System.String," if there's a null in the list
                IEnumerable<string> formattedList = list.Select(s => s ?? "");
                return string.Join(",", formattedList);
            }
        }
    }

}
