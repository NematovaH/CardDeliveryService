using CardDeliveryService.Domain.Entities.Users;
using CardDeliveryService.Service.DTOs.Users;
using CardDeliveryService.Service.Interfaces;
using Spectre.Console;

namespace CardDeliveryService.UI.UserMenu
{
    public class UserMenu
    {
        private readonly IEntityService<User, UserCreateModel, UserUpdateModel, UserViewModel> _userService;

        public UserMenu(IEntityService<User, UserCreateModel, UserUpdateModel, UserViewModel> userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("User Menu")
                    .PageSize(4)
                    .AddChoices(new[] { "Register User", "List Users", "Update User", "Delete User", "Exit" }));

                switch (choice)
                {
                    case "Register User":
                        await RegisterUserAsync();
                        break;
                    case "List Users":
                        await ListUsersAsync();
                        break;
                    case "Update User":
                        await UpdateUserAsync();
                        break;
                    case "Delete User":
                        await DeleteUserAsync();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        private async Task RegisterUserAsync()
        {
            Console.Clear();
            var user = new UserCreateModel
            {
                FirstName = AnsiConsole.Ask<string>("Enter first name:"),
                LastName = AnsiConsole.Ask<string>("Enter last name:"),
                Email = AnsiConsole.Ask<string>("Enter email:"),
                Password = AnsiConsole.Ask<string>("Enter password:"),
                Address = AnsiConsole.Ask<string>("Enter address:")
            };

            try
            {
                await _userService.CreateAsync(user);
                AnsiConsole.MarkupLine("[bold green]User registered successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Failed to register user: {ex.Message}[/]");
            }

            Console.ReadKey(true);
        }

        private async Task ListUsersAsync()
        {
            Console.Clear();
            var users = await _userService.GetAllAsEnumerableAsync();
            if (users.Any())
            {
                var table = new Table().AddColumn("ID").AddColumn("First Name").AddColumn("Last Name").AddColumn("Email").AddColumn("Address");

                foreach (var user in users)
                {
                    table.AddRow(user.Id.ToString(), user.FirstName, user.LastName, user.Email, user.Address);
                }

                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[bold yellow]No users found.[/]");
            }

            Console.ReadKey(true);
        }

        private async Task UpdateUserAsync()
        {
            Console.Clear();
            var userId = AnsiConsole.Ask<long>("Enter the ID of the user to update:");
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                AnsiConsole.MarkupLine("[bold red]User not found.[/]");
                Console.ReadKey(true);
                return;
            }

            var updateUser = new UserUpdateModel
            {
                FirstName = AnsiConsole.Ask<string>($"Enter the new first name for user '{user.FirstName}':"),
                LastName = AnsiConsole.Ask<string>($"Enter the new last name for user '{user.LastName}':"),
                Email = AnsiConsole.Ask<string>($"Enter the new email for user '{user.Email}':"),
                Password = AnsiConsole.Ask<string>($"Enter the new password for user '{user.Password}':"),
                Address = AnsiConsole.Ask<string>($"Enter the new address for user '{user.Address}':")
            };

            try
            {
                await _userService.ModifyAsync(userId, updateUser, false);
                AnsiConsole.MarkupLine("[bold green]User updated successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Failed to update user: {ex.Message}[/]");
            }

            Console.ReadKey(true);
        }

        private async Task DeleteUserAsync()
        {
            Console.Clear();
            var userId = AnsiConsole.Ask<long>("Enter the ID of the user to delete:");
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                AnsiConsole.MarkupLine("[bold red]User not found.[/]");
                Console.ReadKey(true);
                return;
            }

            var confirm = AnsiConsole.Confirm($"Are you sure you want to delete user '{user.FirstName} {user.LastName}'?");
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[bold yellow]Deletion canceled.[/]");
                Console.ReadKey(true);
                return;
            }

            try
            {
                await _userService.RemoveAsync(userId);
                AnsiConsole.MarkupLine("[bold green]User deleted successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Failed to delete user: {ex.Message}[/]");
            }

            Console.ReadKey(true);
        }
    }
}
