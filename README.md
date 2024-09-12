## Решение тестового задания

Состоит из двух проектов - Warehouse и WarehouseTests.

Использованные зависимости:
```
Проект "Warehouse" содержит следующие ссылки на пакеты
   [net8.0]:
   Пакет верхнего уровня                        Запрошено   Разрешено
   > Microsoft.EntityFrameworkCore              8.0.8       8.0.8
   > Microsoft.EntityFrameworkCore.Design       8.0.8       8.0.8
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

## Предложенный способ запуска
1. `docker-compose up --build` - запустит последний Postgres и создаст схему из migration.sql
2. Дождаться `database system is ready to accept connections` в логах
3. `dotnet run --project Warehouse` - запустит приложение

Альтернативно, можно воспользоваться исполняемым файлом - `.\Warehouse.exe`

## Тесты

Для запуска тестов - `dotnet test WarehouseTests`