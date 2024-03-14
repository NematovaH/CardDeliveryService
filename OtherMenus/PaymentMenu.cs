using CardDeliveryService.Domain.Entities.Payments;
using CardDeliveryService.Domain.Enums;
using CardDeliveryService.Service.DTOs.Payments;
using CardDeliveryService.Service.Interfaces;
using Spectre.Console;

namespace CardDeliveryService.UI.OtherMenus
{
    public class PaymentMenu
    {
        private readonly IEntityService<Payment, PaymentCreateModel, PaymentUpdateModel, PaymentViewModel> _paymentService;

        public PaymentMenu(IEntityService<Payment, PaymentCreateModel, PaymentUpdateModel, PaymentViewModel> paymentService)
        {
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Payment Menu")
                    .PageSize(5)
                    .AddChoices(new[] { "Make Payment", "List Payments", "Exit" }));

                switch (choice)
                {
                    case "Make Payment":
                        await MakePaymentAsync();
                        break;
                    case "List Payments":
                        await ListPaymentsAsync();
                        break;
                    case "Exit":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        private async Task MakePaymentAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Make Payment");

            var paymentModel = new PaymentCreateModel();
            paymentModel.Amount = AnsiConsole.Ask<decimal>("Enter payment amount: ");
            paymentModel.PaymentMethod = AnsiConsole.Prompt(
                new SelectionPrompt<PaymentMethod>()
                    .Title("Select payment method")
                    .PageSize(3)
                    .AddChoices(PaymentMethod.Card, PaymentMethod.Cash));
            paymentModel.UserID = AnsiConsole.Ask<long>("Enter user ID: ");
            paymentModel.CardID = AnsiConsole.Ask<long>("Enter card ID: ");
            paymentModel.TransactionDate = AnsiConsole.Ask<DateTime>("Enter transaction date (YYYY-MM-DD HH:MM): ");

            try
            {
                var paymentViewModel = await _paymentService.CreateAsync(paymentModel);
                AnsiConsole.WriteLine($"Payment successfully made! Payment ID: {paymentViewModel.ID}");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to make payment: {ex.Message}");
            }
        }

        private async Task ListPaymentsAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("List Payments");

            try
            {
                var payments = await _paymentService.GetAllAsEnumerableAsync();
                if (payments.Any())
                {
                    var table = new Table().Border(TableBorder.Rounded);
                    table.AddColumn("ID");
                    table.AddColumn("Amount");
                    table.AddColumn("Method");
                    table.AddColumn("User ID");
                    table.AddColumn("Card ID");
                    table.AddColumn("Transaction Date");

                    foreach (var payment in payments)
                    {
                        table.AddRow(payment.ID.ToString(), payment.Amount.ToString(), payment.PaymentMethod.ToString(), payment.UserID.ToString(), payment.CardID.ToString(), payment.TransactionDate.ToString());
                    }

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.WriteLine("No payments found.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to list payments: {ex.Message}");
            }
        }

    }
}
