using System.ComponentModel;
using System.Reflection;

namespace Warehouse.ConsoleApplication.Client.Views;

public enum UserCommand
{
    [Description("1. Создать паллету")]
    CreatePallet = 1,

    [Description("2. Изменить паллету")]
    UpdatePallet = 2,

    [Description("3. Получить паллету")]
    GetPallet = 3,

    [Description("4. Создать коробку")]
    CreateBox = 4,

    [Description("5. Изменить коробку")]
    UpdateBox = 5,

    [Description("6. Выйти")]
    Exit = 6
}

public static class MenuOptionExtensions
{
    public static string GetEntryText(this UserCommand userCommand)
    {
        var field = typeof(UserCommand).GetField(userCommand.ToString());
        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute != null ? attribute.Description : userCommand.ToString();
    }
}
