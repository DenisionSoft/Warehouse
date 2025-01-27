using System.Globalization;
using Warehouse.Business.Pallets;
using Warehouse.Web.Contracts.Models.Box;
using Warehouse.Web.Contracts.Models.Pallet;

namespace Warehouse.ConsoleApplication.Client.Views;

public static class ConsoleView
{

    private static int CursorPosition = 1;
    private static readonly int MaxPosition = Enum.GetNames(typeof(UserCommand)).Length;

    public static UserCommand GetCommandFromMenu()
    {
        while (true)
        {
            RenderMenu();

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    CursorPosition = CursorPosition > 1 ? CursorPosition - 1 : MaxPosition;
                    continue;
                case ConsoleKey.DownArrow:
                    CursorPosition = CursorPosition < MaxPosition ? CursorPosition + 1 : 1;
                    continue;
                case ConsoleKey.Escape:
                    return UserCommand.Exit;
                case ConsoleKey.Enter:
                    return (UserCommand) CursorPosition;
                case >= ConsoleKey.D0 and <= ConsoleKey.D9:
                    CursorPosition = key.Key - ConsoleKey.D0;
                    continue;
                default:
                    continue;
            }
        }
    }

    private static void RenderMenu()
    {
        Console.Clear();
        Console.WriteLine("Используйте стрелки и цифры для навигации.");
        Console.WriteLine("Нажмите Enter для выбора или Escape для выхода.");
        Console.WriteLine("При вводе используйте запятую в качестве разделителя дробной части.\n");

        var menuOptions = Enum.GetValues(typeof(UserCommand)).Cast<UserCommand>();

        foreach (var menuOption in menuOptions)
        {
            var pos = (int)menuOption;
            var entry = menuOption.GetEntryText();
            Console.WriteLine(pos == CursorPosition ? $"> {entry}" : $"  {entry}");
        }
    }

    public static void PrintInputError(ArgumentException e)
    {
        Console.WriteLine($"Ошибка ввода: {e.Message}. Аргумент: {e.ParamName}");
        Console.WriteLine("Нажмите Enter для возврата в меню");
        Console.ReadLine();
    }

    public static long GetIdInputFor(string entityName)
    {
        Console.Clear();
        Console.Write($"Введите идентификатор для сущности {entityName}: ");
        long.TryParse(Console.ReadLine(), out var id);
        return id;
    }

    public static CreatePalletRequest GetCreatePalletRequest()
    {
        Console.Clear();
        Console.WriteLine("Введите данные паллеты:");
        var (width, height, length) = GetCommonValues();
        return new CreatePalletRequest { Width = width, Height = height, Length = length };
    }

    public static UpdatePalletRequest GetUpdatePalletRequest()
    {
        Console.Clear();
        Console.WriteLine("Введите данные паллеты:");
        var (width, height, length) = GetCommonValues();
        return new UpdatePalletRequest { Width = width, Height = height, Length = length };
    }

    public static CreateBoxRequest GetCreateBoxRequest()
    {
        Console.Clear();
        Console.WriteLine("Введите данные коробки:");
        var (width, height, length) = GetCommonValues();

        Console.Write("Вес: ");
        double.TryParse(Console.ReadLine(), out var weight);
        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Вес не может быть меньше или равен нулю");
        }

        Console.WriteLine("Вы можете ввести дату производства или дату окончания срока годности.");
        Console.WriteLine("Для этого, нажмите клавишу 1 или 2 соответственно.");

        var key = Console.ReadKey(true).Key;

        var isExpirationDate = key switch
        {
            ConsoleKey.D1 => false,
            ConsoleKey.D2 => true,
            _ => throw new ArgumentException("Нажата клавиша, отличная от 1 или 2")
        };

        Console.Write("Введите дату в формате DD.MM.YYYY: ");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var givenDate))
        {
            throw new ArgumentException("Неверный формат ввода");
        }

        return new CreateBoxRequest
        {
            Width = width,
            Height = height,
            Length = length,
            Weight = weight,
            GivenDate = givenDate,
            IsExpirationDate = isExpirationDate
        };
    }

    public static UpdateBoxRequest GetUpdateBoxRequest()
    {
        Console.Clear();
        Console.WriteLine("Введите данные коробки:");
        var (width, height, length) = GetCommonValues();

        Console.Write("Вес: ");
        double.TryParse(Console.ReadLine(), out var weight);
        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Вес не может быть меньше или равен нулю");
        }

        Console.Write("Введите дату производства в формате DD.MM.YYYY: ");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var productionDate))
        {
            throw new ArgumentException("Неверный формат ввода");
        }

        Console.Write("Введите дату окончания срока годности в формате DD.MM.YYYY: ");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expirationDate))
        {
            throw new ArgumentException("Неверный формат ввода");
        }

        Console.Write("Введите идентификатор паллеты: ");
        long.TryParse(Console.ReadLine(), out var palletId);

        return new UpdateBoxRequest
        {
            Width = width,
            Height = height,
            Length = length,
            Weight = weight,
            ProductionDate = productionDate,
            ExpirationDate = expirationDate,
            PalletId = palletId
        };
    }

    public static GeneratePalletsCommand GetNewGeneratedDataInput()
    {
        Console.Clear();
        Console.Write("Введите количество паллет для генерации: ");
        int.TryParse(Console.ReadLine(), out var palletsCount);
        if (palletsCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(palletsCount), "Количество паллет не может быть меньше или равен нулю");
        }

        return new GeneratePalletsCommand(palletsCount);
    }

    private static (double, double, double) GetCommonValues()
    {
        Console.Write("Ширина: ");
        double.TryParse(Console.ReadLine(), out double width);
        Console.Write("Высота: ");
        double.TryParse(Console.ReadLine(), out double height);
        Console.Write("Глубина: ");
        double.TryParse(Console.ReadLine(), out double length);

        if (width <= 0 || height <= 0 || length <= 0)
        {
            throw new ArgumentException("Ширина, высота и глубина не могут быть меньше или равны нулю");
        }

        return (width, height, length);
    }

    public static void PrintPallets(IReadOnlyList<PalletResponse?> pallets)
    {
        Console.Clear();

        foreach (var pallet in pallets)
        {
            if (pallet is null)
            {
                continue;
            }
            var expDate = pallet.Boxes.Count == 0 ? "--" : pallet.ExpirationDate.ToString();
            Console.WriteLine($"Паллета #{pallet.Id} | Ш×В×Г: {pallet.Width} × {pallet.Height} × {pallet.Length} | Объём: {pallet.Volume} | Вес: {pallet.Weight} | Срок годности: {expDate}");
            PrintBoxes(pallet.Boxes);
            Console.WriteLine();
        }

        Console.WriteLine("Нажмите Enter для возврата в меню");
        Console.ReadLine();
    }

    public static void PrintBoxes(IReadOnlyList<BoxResponse?> boxes)
    {
        Console.Clear();

        if (boxes.Count == 0)
        {
            Console.WriteLine("Нет коробок для отображения");
        }

        foreach (var box in boxes)
        {
            if (box is null)
            {
                continue;
            }
            Console.WriteLine($"Коробка #{box.Id} | Ш×В×Г: {box.Width} × {box.Height} × {box.Length} | Объём: {box.Volume} | Вес: {box.Weight} | Срок годности: {box.ExpirationDate}");
            Console.WriteLine();
        }

        Console.WriteLine("Нажмите Enter для возврата в меню");
        Console.ReadLine();
    }

    public static void PrintFailureMessage(string message)
    {
        Console.WriteLine("Запрашиваемая операция не смогла быть выполнена так как API вернул ошибку.");
        Console.WriteLine(message);
        Console.WriteLine();
        Console.WriteLine("Нажмите Enter для возврата в меню");
        Console.ReadLine();
    }
}
