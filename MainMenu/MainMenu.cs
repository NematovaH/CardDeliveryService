namespace CardDeliveryService.UI.MainMenu;

public class MainMenu
{
    public void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to Card Delivery Service!");
            Console.WriteLine("1. User Panel");
            Console.WriteLine("2. Admin Panel");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var userMenu = new UserMenu.UserMenu();
                    await userMenu.ShowMenuAsync();
                    break;
                case "2":
                    var adminMenu = new AdminMenu.AdminMenu();
                    await dminMenu.ShowMenuAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ReadKey(true);
                    break;
            }
        }
    }
}
