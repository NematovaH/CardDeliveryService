using Spectre.Console;

namespace CardDeliveryService.UI.AdminMenu;

public class AdminAuthentification
{
    public async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Admin Menu");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Exit");

            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    //
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ReadKey(true);
                    break;
            }
        }
    }
}