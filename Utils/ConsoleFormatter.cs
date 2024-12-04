using System;

namespace CityPowerAndLight.Utils
{
    
    public static class ConsoleFormatter
    {
        public static void PrintTableHeader(string[] columnNames, int[] columnWidths)
        {
            for (int i = 0; i < columnNames.Length; i++)
            {
                Console.Write(columnNames[i].PadRight(columnWidths[i]));
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', columnWidths.Sum())); // Adjust separator length dynamically
        }

        public static void PrintTableRow(string[] rowData, int[] columnWidths)
        {
            for (int i = 0; i < rowData.Length; i++)
            {
                Console.Write(rowData[i].PadRight(columnWidths[i]));
            }
            Console.WriteLine();
        }
        
        public static void PrintHeader(string title)
        {
            Console.WriteLine("========================================");
            Console.WriteLine(title);
            Console.WriteLine("========================================");
        }

        public static void PrintFooter()
        {
            Console.WriteLine("========================================");
            Console.WriteLine();
        }

    }

}
