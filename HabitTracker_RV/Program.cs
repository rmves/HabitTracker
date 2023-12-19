using HabitTracker_RV;
using Microsoft.Data.Sqlite;

class Program
{
    
    static void Main(string[] args)
    {
        DatabaseManager dbManager = new DatabaseManager();
        dbManager.CreateDatabase();

        MainMenu.DisplayMainMenu();
    }
}