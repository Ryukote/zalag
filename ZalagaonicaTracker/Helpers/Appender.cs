namespace ZalagaonicaTracker.Helpers
{
    public static class Appender
    {
        public static bool CheckAndAppend(string value)
        {
            string filePath = "C:\\Users\\ryuko\\Downloads\\ZalagaonicaTracker (2)\\ZalagaonicaTracker (2)\\ZalagaonicaTracker\\ZalagaonicaTracker\\numbers.txt";

            // Read all lines from the file
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                // Check if the value already exists
                if (Array.Exists(lines, line => line.Trim() == value))
                {
                    return false; // Value already exists
                }
            }

            // Append the value to the file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(value);
            }

            return true; // Value added successfully
        }
    }
}
