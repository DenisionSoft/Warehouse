# Warehouse Web API & Console App

Приложение управления складом. Представляет собой Web API с клиентами для взаимодействия и хранилище данных.

Ключевые проекты в решении:
* `Web` - Web API
* `ConsoleApplication` - интерактивное консольное приложение для взаимодействия с API
* `WarehouseTests` - тесты

## Описание API

Приложение предоставляет доступ к ручкам для выполнения операций над сущностями.
Спецификация OpenAPI доступна в автогенерируемом файле [swagger.json](src/Web.GeneratedClient/swagger.json). Он создаётся при
каждом запуске Web API с использованием локального инструмента `Swashbuckle.AspNetCore.Cli`.

### Pallet
* `GET /api/v1/pallets` - получение всех паллет
* `GET /api/v1/pallets/{palletId}` - получение паллеты
* `POST /api/v1/pallets` - добавление паллеты
* `PATCH /api/v1/pallets/{palletId}` - изменение паллеты
* `DELETE /api/v1/pallets/{palletId}` - удаление паллеты

### Box
* `GET /api/v1/boxes` - получение всех коробок
* `GET /api/v1/boxes/{boxId}` - получение коробки
* `GET /api/v1/pallets/{palletId}/boxes` - получение всех коробок на паллете
* `POST /api/v1/pallets/{palletId}/boxes` - добавление коробки на паллету
* `PATCH /api/v1/pallets/{palletId}/boxes/{boxId}` - изменение коробки
* `DELETE /api/v1/boxes/{boxId}` - удаление коробки

## Установка и запуск

Необходимые зависимости:
* [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* [EF Core CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) (для работы с миграциями)
* [Docker](https://www.docker.com/products/docker-desktop) (для запуска тестов или базы данных)

### Web API

Убедитесь, что выбран нужный поставщик данных в [appsettings.json](src/Web/appsettings.json). Приложение поддерживает SQLite и PostgreSQL.

При выборе PostgreSQL, убедитесь, что база данных запущена и доступна. Вы можете воспользоваться готовым docker-compose,
который создаст и запустит контейнер с PostgreSQL. Для этого, запустите Docker и из `\docker` выполните:

```shell
docker-compose up --build
```

Для подключения к этой БД восползьуйтесь следующей строкой подключения в [appsettings.json](src/Web/appsettings.json):

`Host=localhost;Port=5432;Database=warehouse;Username=warehouse;Password=password`

Из корневой директории проекта:

```shell
dotnet run --project .\src\Web
```

### Консольное приложение

Убедитесь, что Web API запущено и доступно по адресу, указанному в [appsettings.json](src/ConsoleApplication/appsettings.json).

Из корневой директории проекта:

```shell
dotnet run --project .\src\ConsoleApplication
```

### Тестирование

Убедитесь, что Docker запущен. Тесты используют TestContainers.

Из корневой директории проекта:

```shell
dotnet test
```

## Миграции

Для создания новой миграции, выберите имя миграции и её расположение. Для миграций к SQLite используйте проект `Data.Migrations.SQLite`, для PostgreSQL - `Data.Migrations.Psql`.

Убедитесь, что установлены желаемые значения в [appsettings.json](src/Web/appsettings.json), и что база данных (в случае PostgreSQL) запущена и доступна.

Из корневой директории проекта выполните команду, заменив `<MigrationName>` и `<MigrationProject>` на соответствующие значения:

```shell
dotnet ef migrations add <MigrationName> --project <MigrationProject> --startup-project src/Web --context WarehouseDbContext
```

Миграция применится автоматически при запуске Web API. Для применения миграции вручную, выполните:

```shell
dotnet ef database update --project <MigrationProject> --startup-project src/Web --context WarehouseDbContext
```
