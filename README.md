## Решение тестового задания

Состоит из двух проектов - Warehouse и WarehouseTests.

Использованные зависимости:
```
Проект "Warehouse" содержит следующие ссылки на пакеты
   [net8.0]:
   Пакет верхнего уровня                        Запрошено   Разрешено
   > Microsoft.EntityFrameworkCore              8.0.4       8.0.4
   > Microsoft.EntityFrameworkCore.Design       8.0.4       8.0.4
   > Npgsql.EntityFrameworkCore.PostgreSQL      8.0.4       8.0.4

Проект "WarehouseTests" содержит следующие ссылки на пакеты
   [net8.0]:
   Пакет верхнего уровня            Запрошено   Разрешено
   > coverlet.collector             6.0.0       6.0.0
   > FluentAssertions               6.12.1      6.12.1
   > Microsoft.NET.Test.Sdk         17.8.0      17.8.0
   > Moq                            4.20.72     4.20.72
   > xunit                          2.5.3       2.5.3
   > xunit.runner.visualstudio      2.5.3       2.5.3
```

## Предложенные способы запуска
### Настройка базы данных
Вариант 1. Вызов `dotnet ef database update` из `\Warehouse` - создаст схему из миграции

Вариант 2. Вызов `docker-compose up --build` из `\docker` - запустит последний Postgres и создаст схему из migration.sql (полученный из схемы путём `dotnet ef migrations script -o migration.sql`)

Вариант 3. Вызов `.\efbundle.exe` из `\Warehouse` - создаст схему из миграции

### Запуск приложения

Вариант 1. Вызов `dotnet run --project Warehouse` из корня - соберёт и запустит приложение

Вариант 2. Вызов `.\Warehouse.exe` из корня - запустит созданный исполняемый файл

## Тесты

Для запуска тестов - `dotnet test WarehouseTests`