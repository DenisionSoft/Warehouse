using Warehouse.ConsoleApplication.Client;
using Warehouse.ConsoleApplication.Client.Views;
using Warehouse.Web.Client.Boxes;
using Warehouse.Web.Client.Pallets;

namespace Warehouse.ConsoleApplication;

public sealed class ConsoleApplication
{

    private readonly IBoxClient boxClient;
    private readonly IPalletClient palletClient;
    private readonly RandomPalletGenerator randomPalletGenerator = new();

    public ConsoleApplication(IBoxClient boxClient, IPalletClient palletClient)
    {
        this.boxClient = boxClient;
        this.palletClient = palletClient;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            UserCommand command = ConsoleView.GetCommandFromMenu();

            try
            {
                switch (command)
                {
                    case UserCommand.CreatePallet:
                        var createPalletRequest = ConsoleView.GetCreatePalletRequest();
                        var createPalletResponse = await palletClient.AddPallet(createPalletRequest);
                        ConsoleView.PrintPallets([createPalletResponse]);
                        continue;
                    case UserCommand.UpdatePallet:
                        var palletIdToUpdate = ConsoleView.GetIdInputFor("Паллета");
                        var updatePalletRequest = ConsoleView.GetUpdatePalletRequest();
                        var updatePalletResponse = await palletClient.UpdatePallet(palletIdToUpdate, updatePalletRequest);
                        ConsoleView.PrintPallets([updatePalletResponse]);
                        continue;
                    case UserCommand.GetPallet:
                        var palletIdToGet = ConsoleView.GetIdInputFor("Паллета");
                        var getPalletResponse = await palletClient.GetPalletById(palletIdToGet);
                        ConsoleView.PrintPallets([getPalletResponse]);
                        continue;
                    case UserCommand.CreateBox:
                        var palletIdToAddBox = ConsoleView.GetIdInputFor("Паллета");
                        var createBoxRequest = ConsoleView.GetCreateBoxRequest();
                        var createBoxResponse = await boxClient.AddBox(palletIdToAddBox, createBoxRequest);
                        ConsoleView.PrintBoxes([createBoxResponse]);
                        continue;
                    case UserCommand.UpdateBox:
                        var palletIdToUpdateBox = ConsoleView.GetIdInputFor("Паллета");
                        var boxIdToUpdate = ConsoleView.GetIdInputFor("Коробка");
                        var updateBoxRequest = ConsoleView.GetUpdateBoxRequest();
                        var updateBoxResponse = await boxClient.UpdateBox(palletIdToUpdateBox, boxIdToUpdate, updateBoxRequest);
                        ConsoleView.PrintBoxes([updateBoxResponse]);
                        continue;
                    case UserCommand.Exit:
                        return;
                }
            }
            catch (ArgumentException e)
            {
                ConsoleView.PrintInputError(e);
            }
            catch (HttpRequestException ex)
            {
                ConsoleView.PrintFailureMessage(ex.Message);
            }
        }
    }
}
