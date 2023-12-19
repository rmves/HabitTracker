using Microsoft.Data.Sqlite;
using System.Diagnostics.Metrics;
using System;

namespace HabitTracker_RV
{
    public class DatabaseManager
    {
        static string connectionString = @"Data Source=HabitTracker.db";

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habit (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Habit_Type TEXT,
                Quantity INTEGER)";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        internal static void Insert()
        {
            Console.Clear();
            Console.WriteLine("Enter a habit or '0' to return to main menu.");
            string habit = Console.ReadLine() ?? string.Empty;

            if (habit == null || habit == "0")
            {
                MainMenu.DisplayMainMenu();
                return;
            }

            Console.WriteLine("Enter a Date (DD-MM-YY) you performed the habit:");
            string userDate = Console.ReadLine() ?? string.Empty;

            int quantity;
            Console.WriteLine("What quantity of  the habit would you like to enter?");
            string userQuantity = Console.ReadLine() ?? string.Empty;
            
            if (int.TryParse(userQuantity, out int parsedQuantity))
            {
                quantity = parsedQuantity;
            }
            else
            {
                Console.WriteLine("Invalid input. Using default quantity: 0");
                quantity = 0;
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"INSERT INTO habit (Date, Habit_Type, Quantity) VALUES (@date, @habit, @quantity)";
                tableCmd.Parameters.AddWithValue("@date", userDate);
                tableCmd.Parameters.AddWithValue("@habit", habit);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
            MainMenu.DisplayMainMenu();
        }
        internal static void ViewAllRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"SELECT * FROM habit";
                using (var reader = tableCmd.ExecuteReader())
                {
                    Console.WriteLine("All Records:\n");

                    Console.WriteLine("ID\tDate\t\tHabit Type\tQuantity");
                    Console.WriteLine("------------------------------------------------------------------");

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string date = reader.GetString(1);
                        string habit = reader.GetString(2);
                        int quantity = reader.GetInt32(3);

                        Console.WriteLine($"{id, -4}\t{date,-18}\t{habit, -15}\t{quantity}");
                    }
                }

                connection.Close();
            }
        }

        internal static void Update()
        {
            ViewAllRecords();
            
            Console.WriteLine("Enter a record Id you would like to update:");
            string userRecordId = Console.ReadLine() ?? string.Empty;

            int recordId = int.TryParse(userRecordId, out int parsedId) ? parsedId : 0;

            Console.WriteLine("Which field do you want to update?");
            Console.WriteLine("1. Date");
            Console.WriteLine("2. Habit Type");
            Console.WriteLine("3. Quantity");
            Console.WriteLine("Enter the number of the field you want to update:");

            string userInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(userInput, out int selectedField))
            {
                Console.WriteLine("Invalid input. Please enter a number corresponding to the field.");
                return;
            }

            string columnName;
            string newValue = string.Empty;
            Console.WriteLine("Enter the new value:");

            switch(selectedField)
            {
                case 1:
                    columnName = "Date";
                    string newDate = Console.ReadLine();
                    newValue = newDate;
                    break;

                case 2:
                    columnName = "Habit_Type";
                    // Capture user input for Habit_Type and assign it to newValue
                    newValue = Console.ReadLine();
                    break;

                case 3:
                    columnName = "Quantity";
                    // Capture user input for Quantity and assign it to newValue
                    int newQuantity;
                    if (!int.TryParse(Console.ReadLine(), out newQuantity))
                    {
                        Console.WriteLine("Invalid quantity. Please enter a valid number.");
                        return;
                    }
                    newValue = newQuantity.ToString();
                    break;

                default:
                    Console.WriteLine("Invalid field selected.");
                    return;
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE habit SET {columnName} = @newValue WHERE ID = @recordId";
                tableCmd.Parameters.AddWithValue("@newValue", newValue);
                tableCmd.Parameters.AddWithValue("@recordId", recordId);
                tableCmd.ExecuteNonQuery();

                Console.WriteLine("Record updated successfully.");
                connection.Close();
            }
        }

        internal static void Delete()
        {
            ViewAllRecords();

            Console.WriteLine("Enter a record Id you would like to delete:");
            string userRecordId = Console.ReadLine() ?? string.Empty;

            int recordId = int.TryParse(userRecordId, out int parsedId) ? parsedId : 0;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM habit WHERE ID = @recordId";
                tableCmd.Parameters.AddWithValue("@recordId", recordId);
                tableCmd.ExecuteNonQuery();

                Console.WriteLine("Record successfully deleted.");
                connection.Close();
            }
        }
    }
}
