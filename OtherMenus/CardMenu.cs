using CardDeliveryService.Domain.Entities.Cards;
using CardDeliveryService.Service.DTOs.Cards;
using CardDeliveryService.Service.Interfaces;
using Spectre.Console;

namespace CardDeliveryService.UI.OtherMenus
{
    public class CardMenu
    {
        private readonly IEntityService<Card, CardCreateModel, CardUpdateModel, CardViewModel> _cardService;

        public CardMenu(IEntityService<Card, CardCreateModel, CardUpdateModel, CardViewModel> cardService)
        {
            _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Card Menu")
                    .PageSize(6)
                    .AddChoices(new[] { "Create Card", "List Cards", "Get by ID", "Update Card", "Delete Card", "Terminate Card", "Exit" }));

                switch (choice)
                {
                    case "Create Card":
                        await CreateCardAsync();
                        break;
                    case "List Cards":
                        await ListCardsAsync();
                        break;
                    case "Update Card":
                        await UpdateCardAsync();
                        break;
                    case "Delete Card":
                        await DeleteCardAsync();
                        break;
                    case "Terminate Card":
                        await TerminateCardAsync();
                        break;
                    case "Get by ID":
                        await GetByIdAsync();
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

        private async Task CreateCardAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Create Card");

            var cardModel = new CardCreateModel();
            cardModel.DeliveryAddress = AnsiConsole.Ask<string>("Enter delivery address: ");

            try
            {
                var cardViewModel = await _cardService.CreateAsync(cardModel);
                AnsiConsole.WriteLine($"Card successfully created! Card ID: {cardViewModel.ID}");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to create card: {ex.Message}");
            }
        }

        private async Task<CardViewModel> GetByIdAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Get Card Details");

            var id = AnsiConsole.Ask<long>("Enter the ID of the card to get info: ");
            try
            {
                var card = await _cardService.GetByIdAsync(id);
                if (card != null)
                {
                    AnsiConsole.WriteLine($"ID: {card.ID}, Delivery Address: {card.DeliveryAddress}, Delivery Status: {card.DeliveryStatus}");
                }
                else
                {
                    AnsiConsole.WriteLine($"Card with ID {id} not found.");
                }
                return card;
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to retrieve card: {ex.Message}");
                return null;
            }
        }

        private async Task ListCardsAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("List Cards");

            try
            {
                var cards = await _cardService.GetAllAsEnumerableAsync();
                if (cards.Any())
                {
                    var table = new Table().Border(TableBorder.Rounded);
                    table.AddColumn("ID");
                    table.AddColumn("Delivery Address");
                    table.AddColumn("Delivery Status");

                    foreach (var card in cards)
                    {
                        table.AddRow(card.ID.ToString(), card.DeliveryAddress, card.DeliveryStatus.ToString());
                    }

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.WriteLine("No cards found.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to list cards: {ex.Message}");
            }
        }

        private async Task UpdateCardAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Update Card");

            var cardId = AnsiConsole.Ask<long>("Enter the ID of the card to update: ");

            try
            {
                var existingCard = await _cardService.GetByIdAsync(cardId);
                if (existingCard != null)
                {
                    var cardModel = new CardUpdateModel();
                    cardModel.DeliveryAddress = AnsiConsole.Ask<string>("Enter new delivery address: ");

                    var updatedCard = await _cardService.ModifyAsync(cardId, cardModel, isUsesDeleted: false);
                    AnsiConsole.WriteLine($"Card successfully updated! Updated ID: {updatedCard.ID}");
                }
                else
                {
                    AnsiConsole.WriteLine($"Card with ID {cardId} not found.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to update card: {ex.Message}");
            }
        }

        private async Task DeleteCardAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Delete Card");

            var cardId = AnsiConsole.Ask<long>("Enter the ID of the card to delete: ");

            try
            {
                var result = await _cardService.RemoveAsync(cardId);
                if (result)
                {
                    AnsiConsole.WriteLine($"Card with ID {cardId} successfully deleted.");
                }
                else
                {
                    AnsiConsole.WriteLine($"Failed to delete card with ID {cardId}.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to delete card: {ex.Message}");
            }
        }

        private async Task TerminateCardAsync()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Terminate Card");

            var cardId = AnsiConsole.Ask<long>("Enter the ID of the card to terminate: ");

            try
            {
                var result = await _cardService.TerminateAsync(cardId);
                if (result)
                {
                    AnsiConsole.WriteLine($"Card with ID {cardId} successfully terminated.");
                }
                else
                {
                    AnsiConsole.WriteLine($"Failed to terminate card with ID {cardId}.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Failed to terminate card: {ex.Message}");
            }
        }
    }
}
