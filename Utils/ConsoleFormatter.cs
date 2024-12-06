namespace CityPowerAndLight.Utils
{
    /// <summary>
    /// Provides utility methods for formatting and printing data to the console, particularly for displaying tables.
    /// </summary>
    public static class ConsoleFormatter
    {
        /// <summary>
        /// Prints the table header with column names and adjusts the separator line based on column widths.
        /// </summary>
        /// <param name="columnNames">An array of strings representing the column names.</param>
        /// <param name="columnWidths">An array of integers representing the width of each column.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="columnNames"/> or <paramref name="columnWidths"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the lengths of <paramref name="columnNames"/> and <paramref name="columnWidths"/> do not match.</exception>
        public static void PrintTableHeader(string[] columnNames, int[] columnWidths)
        {
            if (columnNames == null) 
                throw new ArgumentNullException(nameof(columnNames), "Column names cannot be null.");
            if (columnWidths == null) 
                throw new ArgumentNullException(nameof(columnWidths), "Column widths cannot be null.");
            if (columnNames.Length != columnWidths.Length) 
                throw new ArgumentException("Column names and column widths must have the same length.");

            // Print the column names
            for (int i = 0; i < columnNames.Length; i++)
            {
                Console.Write(columnNames[i].PadRight(columnWidths[i]));
            }
            Console.WriteLine();

            // Print a separator line based on the column widths
            Console.WriteLine(new string('-', columnWidths.Sum())); // Adjust separator length dynamically
        }

        /// <summary>
        /// Prints a single row of data in the table, aligning each column based on the specified widths.
        /// </summary>
        /// <param name="rowData">An array of strings representing the data for the row.</param>
        /// <param name="columnWidths">An array of integers representing the width of each column.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="rowData"/> or <paramref name="columnWidths"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the lengths of <paramref name="rowData"/> and <paramref name="columnWidths"/> do not match.</exception>
        public static void PrintTableRow(string[] rowData, int[] columnWidths)
        {
            if (rowData == null) 
                throw new ArgumentNullException(nameof(rowData), "Row data cannot be null.");
            if (columnWidths == null) 
                throw new ArgumentNullException(nameof(columnWidths), "Column widths cannot be null.");
            if (rowData.Length != columnWidths.Length) 
                throw new ArgumentException("Row data and column widths must have the same length.");

            // Print the row data
            for (int i = 0; i < rowData.Length; i++)
            {
                Console.Write(rowData[i].PadRight(columnWidths[i]));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a header with a title, enclosed by lines of equal signs for emphasis.
        /// </summary>
        /// <param name="title">The title to display in the header.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> is null or empty.</exception>
        public static void PrintHeader(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) 
                throw new ArgumentNullException(nameof(title), "Title cannot be null or empty.");

            // Print a formatted header
            Console.WriteLine("========================================");
            Console.WriteLine(title);
            Console.WriteLine("========================================");
        }
    }
}
