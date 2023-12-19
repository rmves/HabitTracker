namespace HabitTracker_RV
{
    public class MainMenu
    {
        public static void DisplayMainMenu()
        {
            Console.Clear();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("---------------------------\n");

                string userInput = Console.ReadLine() ?? string.Empty;

                switch (userInput)
                {
                    case "0":
                        Environment.Exit(0);
                        break;

                    case "1":
                        Console.Clear();
                        DatabaseManager.ViewAllRecords();
                        Console.WriteLine("Press ENTER to return to the main menu...");
                        Console.ReadLine();
                        break;

                    case "2":
                        DatabaseManager.Insert();
                        break;

                    case "3":
                        DatabaseManager.Delete();
                        break;

                    case "4":
                        Console.Clear();
                        DatabaseManager.Update();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
