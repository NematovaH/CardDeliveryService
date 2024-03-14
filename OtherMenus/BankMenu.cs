using CardDeliveryService.Domain.Entities.Banks;
using CardDeliveryService.Service.DTOs.Banks;
using CardDeliveryService.Service.Interfaces;
using Spectre.Console;

namespace CardDeliveryService.UI.OtherMenus
{
    public class BankMenu
    {
        private readonly IEntityService<Bank, BankCreateModel, BankUpdateModel, BankViewModel> _bankService;

        public BankMenu(IEntityService<Bank, BankCreateModel, BankUpdateModel, BankViewModel> bankService)
        {
            _bankService = bankService;
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Bank Menu")
                    .PageSize(5)
                    .AddChoices(new[] { "Create Bank", "List Banks", "Update Bank", "Delete Bank", "Get Bank by ID", "Terminate Bank", "Exit" }));

                switch (choice)
                {
                    case "Create Bank":
                        await CreateBankAsync();
                        break;
                    case "List Banks":
                        await ListBanksAsync();
                        break;
                    case "Update Bank":
                        await UpdateBankAsync();
                        break;
                    case "Delete Bank":
                        await DeleteBankAsync();
                        break;
                    case "Get Bank by ID":
                        await GetBankByIdAsync();
                        break;
                    case "Terminate Bank":
                        await TerminateBankAsync();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        private async Task CreateBankAsync()
        {
            var name = AnsiConsole.Ask<string>("Enter the name of the bank:");
            var location = AnsiConsole.Ask<string>("Enter the location of the bank:");

            var createModel = new BankCreateModel { Name = name, Location = location };
            await _bankService.CreateAsync(createModel);
            AnsiConsole.MarkupLine("[bold green]Bank created successfully![/]");
            Console.ReadKey(true);
        }

        private async Task ListBanksAsync()
        {
            var banks = await _bankService.GetAllAsEnumerableAsync();
            var table = new Table().AddColumn("ID").AddColumn("Name").AddColumn("Location");

            foreach (var bank in banks)
            {
                table.AddRow(bank.ID.ToString(), bank.Name, bank.Location);
            }

            AnsiConsole.Write(table);
            Console.ReadKey(true);
        }

        private async Task UpdateBankAsync()
        {
            var id = AnsiConsole.Ask<long>("Enter the ID of the bank to update:");
            var bank = await _bankService.GetByIdAsync(id);
            if (bank == null)
            {
                AnsiConsole.MarkupLine("[bold red]Bank not found.[/]");
                Console.ReadKey(true);
                return;
            }

            var name = AnsiConsole.Ask<string>($"Enter the new name for bank '{bank.Name}':");
            var location = AnsiConsole.Ask<string>($"Enter the new location for bank '{bank.Location}':");

            var updateModel = new BankUpdateModel { Name = name, Location = location };
            await _bankService.ModifyAsync(id, updateModel, false);
            AnsiConsole.MarkupLine("[bold green]Bank updated successfully![/]");
            Console.ReadKey(true);
        }

        private async Task DeleteBankAsync()
        {
            var id = AnsiConsole.Ask<long>("Enter the ID of the bank to delete:");
            var bank = await _bankService.GetByIdAsync(id);
            if (bank == null)
            {
                AnsiConsole.MarkupLine("[bold red]Bank not found.[/]");
                Console.ReadKey(true);
                return;
            }

            var confirm = AnsiConsole.Confirm($"Are you sure you want to delete bank '{bank.Name}'?");
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[bold yellow]Deletion canceled.[/]");
                Console.ReadKey(true);
                return;
            }

            await _bankService.RemoveAsync(id);
            AnsiConsole.MarkupLine("[bold green]Bank deleted successfully![/]");
            Console.ReadKey(true);
        }

        private async Task GetBankByIdAsync()
        {
            var id = AnsiConsole.Ask<long>("Enter the ID of the bank:");
            var bank = await _bankService.GetByIdAsync(id);
            if (bank == null)
            {
                AnsiConsole.MarkupLine("[bold red]Bank not found.[/]");
                Console.ReadKey(true);
                return;
            }

            var table = new Table().AddColumn("ID").AddColumn("Name").AddColumn("Location");
            table.AddRow(bank.ID.ToString(), bank.Name, bank.Location);
            AnsiConsole.Write(table);
            Console.ReadKey(true);
        }

        private async Task TerminateBankAsync()
        {
            var id = AnsiConsole.Ask<long>("Enter the ID of the bank to terminate:");
            var bank = await _bankService.GetByIdAsync(id);
            if (bank == null)
            {
                AnsiConsole.MarkupLine("[bold red]Bank not found.[/]");
                Console.ReadKey(true);
                return;
            }

            var confirm = AnsiConsole.Confirm($"Are you sure you want to terminate bank '{bank.Name}'?");
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[bold yellow]Termination canceled.[/]");
                Console.ReadKey(true);
                return;
            }

            await _bankService.TerminateAsync(id);
            AnsiConsole.MarkupLine("[bold green]Bank terminated successfully![/]");
            Console.ReadKey(true);
        }
    }
}
