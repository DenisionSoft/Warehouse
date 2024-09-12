using Warehouse.Models;
using Warehouse.Repositories;
using Warehouse.Services;
using Warehouse.Utils;

namespace Warehouse;

class Program
{
    static async Task Main()
    {
        using (var context = new WarehouseContext())
        {
            PalletsRepository palletsRepository = new(context);
            BoxesRepository boxesRepository = new(context);
            
            PalletService palletService = new(palletsRepository);
            BoxService boxService = new(boxesRepository, palletsRepository);
            
            InputUtil inputUtil = new();
            RandomDataGenerator randomDataGenerator = new();

            int option;
            
            while (true)
            {
                option = inputUtil.GetMenuOption();

                switch (option)
                {
                    case 1:
                        try
                        {
                            NewPalletDto palletDto = inputUtil.CreatePalletInput();
                            await palletService.AddAsync(palletDto);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Ошибка: {e.Message}");
                            Console.WriteLine("Нажмите Enter для возврата в меню");
                            Console.ReadLine();
                        }
                        break;
                    case 2:
                        try
                        {
                            NewBoxDto boxDto = inputUtil.CreateBoxInput();
                            await boxService.AddAsync(boxDto);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Ошибка: {e.Message}");
                            Console.WriteLine("Нажмите Enter для возврата в меню");
                            Console.ReadLine();
                        }
                        break;
                    case 3:
                        try
                        {
                            int palletsCount = inputUtil.GenerateDataInput();
                            await palletService.AddRangeAsync(randomDataGenerator.GeneratePallets(palletsCount));
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Ошибка: {e.Message}");
                            Console.WriteLine("Нажмите Enter для возврата в меню");
                            Console.ReadLine();
                        }
                        break;
                    case 4:
                        Console.Clear();
                        List<Pallet> mepallets = await palletService.GetMostExpired();
                        foreach (var pallet in mepallets)
                        {
                            Console.WriteLine(pallet);
                            pallet.PrintBoxes();
                            Console.WriteLine();
                        }
                        Console.WriteLine("Нажмите Enter для возврата в меню");
                        Console.ReadLine();
                        break;
                    case 5:
                        Console.Clear();
                        List<Pallet> lepallets = await palletService.GetLeastExpiredByBoxes();
                        foreach (var pallet in lepallets)
                        {
                            Console.WriteLine(pallet);
                            pallet.PrintBoxes();
                            Console.WriteLine();
                        }
                        Console.WriteLine("Нажмите Enter для возврата в меню");
                        Console.ReadLine();
                        break;
                    case 6:
                        return;
                }
            }
        }
    }
}