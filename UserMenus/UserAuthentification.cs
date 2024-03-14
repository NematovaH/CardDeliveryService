using CardDeliveryService.Domain.Entities.Users;
using CardDeliveryService.Service.DTOs.Users;
using CardDeliveryService.Service.Interfaces;
using System;

namespace CardDeliveryService.UI.UserMenu
{
    public class UserAuthentification
    {
        private readonly UserMenu _userMenu;
        private readonly IEntityService<User, UserCreateModel, UserUpdateModel, UserViewModel> _userService;

        public UserAuthentification(UserMenu userMenu, IEntityService<User, UserCreateModel, UserUpdateModel, UserViewModel> userService)
        {
            _userMenu = userMenu ?? throw new ArgumentNullException(nameof(userMenu));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("User Menu");
                Console.WriteLine("1. Register User");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RegisterUserAsync();
                        break;
                    case "2":
                        var isLoggedIn = await LoginAsync();
                        if (isLoggedIn)
                        {
                            await _userMenu.ShowMenuAsync();
                        }
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

        private async Task RegisterUserAsync()
        {
            // Your registration logic here
        }

        private async Task<bool> LoginAsync()
        {
            Console.Clear();
            Console.Write("Enter your email: ");
            var email = Console.ReadLine();

            Console.Write("Enter your password: ");
            var password = Console.ReadLine();

            try
            {
                var user = await _userService.LoginAsync(email, password);
                if (user != null)
                {
                    Console.WriteLine("Login successful.");
                    Console.ReadKey(true);
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid email or password. Please try again.");
                    Console.ReadKey(true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                Console.ReadKey(true);
                return false;
            }
        }
    }
}
