using System.Globalization;
using System.Text;

namespace Warehouse.Utils;

public class InputUtil
{
    private static int _position = 1;
    
    public InputUtil()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }
    
    /// <summary>
    /// Метод <c>GetMenuOption</c> отвечает за отображение меню и ввод пользовательского выбора.
    /// </summary>
    /// <returns>Возвращает выбранный пользователем пункт меню.</returns>
    public int GetMenuOption()
    {
        while (true)
        {
            Console.Clear();
            RenderMenu();
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _position = _position > 1 ? _position - 1 : 6;
                    continue;
                case ConsoleKey.DownArrow:
                    _position = _position < 6 ? _position + 1 : 1;
                    continue;
                case ConsoleKey.Escape:
                    return 6;
                case ConsoleKey.Enter:
                    return _position;
            }
            
            if (key >= ConsoleKey.D1 && key <= ConsoleKey.D6)
            {
                _position = (int)key - 48;
            }
        }
    }
    
    /// <summary>
    /// Метод <c>CreatePalletInput</c> отвечает за ввод данных для создания паллеты.
    /// </summary>
    /// <returns>Возвращает объект NewPalletDto с введенными данными.</returns>
    public NewPalletDto CreatePalletInput()
    {
        Console.Clear();
        Console.WriteLine("Введите данные паллеты:");
        var (width, height, length) = GetCommonValues();
        return new NewPalletDto(width, height, length);
    }

    /// <summary>
    /// Метод <c>CreateBoxInput</c> отвечает за ввод данных для создания коробки.
    /// </summary>
    /// <returns>Возвращает объект NewBoxDto с введенными данными.</returns>
    /// <exception cref="ArgumentException">Возникает при неверном формате ввода.</exception>
    public NewBoxDto CreateBoxInput()
    {
        Console.Clear();
        Console.WriteLine("Введите данные коробки:");
        var (width, height, length) = GetCommonValues();
        
        Console.Write("Вес: ");
        double.TryParse(Console.ReadLine(), out double weight);
        if (weight <= 0)
        {
            throw new ArgumentException("неверный формат ввода.");
        }
        
        DateOnly givenDate;
        bool isExpirationDate;
        Console.WriteLine("Вы можете ввести дату производства или дату окончания срока годности.");
        Console.WriteLine("Для этого, нажмите клавишу 1 или 2 соответственно.");
        
        
        var key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.D1:
                isExpirationDate = false;
                break;
            case ConsoleKey.D2:
                isExpirationDate = true;
                break;
            default:
                throw new ArgumentException("нажата клавиша, отличная от 1 или 2.");
        }
        
        Console.Write("Введите дату в формате DD.MM.YYYY: ");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out givenDate))
        {
            throw new ArgumentException("неверный формат ввода.");
        }
        
        Console.Write("Введите идентификатор паллеты для новой коробки: ");
        long.TryParse(Console.ReadLine(), out long palletId);
        
        return new NewBoxDto(width, height, length, weight, givenDate, isExpirationDate, palletId);
    }

    /// <summary>
    /// Метод <c>GenerateDataInput</c> отвечает за ввод количества паллет для генерации.
    /// </summary>
    /// <returns>Возвращает количество паллет для генерации.</returns>
    /// <exception cref="ArgumentException">Возникает при неверно введённых данных.</exception>
    public int GenerateDataInput()
    {
        Console.Clear();
        Console.Write("Введите количество паллет для генерации: ");
        int.TryParse(Console.ReadLine(), out var palletsCount);
        if (palletsCount <= 0)
        {
            throw new ArgumentException("неверные данные.");
        }
        Console.WriteLine("Генерирую...");
        return palletsCount;
    }

    private (double, double, double) GetCommonValues()
    {
        Console.Write("Ширина: ");
        double.TryParse(Console.ReadLine(), out double width);
        Console.Write("Высота: ");
        double.TryParse(Console.ReadLine(), out double height);
        Console.Write("Глубина: ");
        double.TryParse(Console.ReadLine(), out double length);
        
        if (width <= 0 || height <= 0 || length <= 0)
        {
            throw new ArgumentException("неверные данные.");
        }
        
        return (width, height, length);
    }
    
    private void RenderMenu()
    {
        Console.WriteLine("Используйте стрелки и цифры 1-6 для навигации.");
        Console.WriteLine("Нажмите Enter для выбора или Escape для выхода.");
        Console.WriteLine("При вводе используйте запятую в качестве разделителя дробной части.\n");
        RenderMenuOption(1, "Создать паллету");
        RenderMenuOption(2, "Создать коробку");
        RenderMenuOption(3, "Сгенерировать данные");
        RenderMenuOption(4, "Вывести все паллеты с сортировкой");
        RenderMenuOption(5, "Вывести 3 паллеты с наиболее долгохранящимися коробками");
        RenderMenuOption(6, "Выйти");
    }

    private void RenderMenuOption(int pos, string line)
    {
        Console.WriteLine(pos == _position ? $"> {pos}) {line}" : $"  {pos}) {line}");
    }
}