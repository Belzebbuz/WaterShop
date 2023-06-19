# WaterShop

## Program.cs

- Регистрация сервисов
- Middleware для обарботки несанкционарованного доступа к admin.html
- Minimal Api для переадресации на index.html и admin.html

## Controllers/

### WaterController.cs

- API для создания, удаления, изменения, управления количеством и покупкой воды.
- API для получения списка dto воды для отображения на клиентской части

```cs
public class WaterDto
{
	public Guid Id { get; set; }
    public string Title { get; set; }
    public string? ImagePath { get; set; }
    public int Cost { get; set; }
    public int Count { get; set; }
}
```

- API для импорта списка сущностей из .json файла.
  Загрузить картинки можно через метод PUT, который предназначен для обновления сущностей.
  Пример этого файла находится по пути "TestData/seed.json"

```json
[
  {
    "title": "water test#1",
    "cost": 20
  },
  {
    "title": "water test#2",
    "cost": 30
  }
]
```

### BalanceController.cs

- API для управления текущим балансом, добавления монет и получения сдачи
- Сдача возвращается в номиналах монет от 10 рублей до 1 рубля

## Contracts/

### IWaterService.cs

- Интерфейс для управления сущностью воды. Сохранение, удаление, изменения.
- Получения списка dto сущности воды
- Создания из списка десириалзованного из файла импорта новых сущностей.

### IBalanceService.cs

- Интерфейс для управления балансом и вычисления сдачи.

## Services/

- WaterService.cs - Реализация IWaterService через контекст базы данных
- BalanceService.cs - Реализация IBalanceService. Хранит и изменяет текущее состояние баланса

## Domain/

- Water.cs -Сущность воды, содержит свою статическую фабрику и методы изменяющие ее состояние.

## Exceptions/

- NotFoundException.cs - исключение для обозначения не найденной сущности.
- ValidationDataException.cs - исключение для обозначения не корректных данных.
- Эти исключения обрабатываются глобально в ExceptionMiddleware.cs

## Middlewares/

- ExceptionMiddleware.cs - глобально обрабатывает исключения, волзникающие в ходе запроса.
  В зависимоти от типа исключения устанавливает соответсвующий код ответа.

## Persistance/

- AppDbContext.cs - класс, наследующий DbContext.
- DbSeeder.cs - генерирует тестовые данные, если база данных только создана.
- PersistanceExtensions.cs - содержит методы расширения IServiceCollection для добавления контекста базы данных, и IApplicationBuilder для применения миграций

## ConfigOptions/

- SaveImageOptions.cs - содержит настройки сохранения изображений, а конкретно путь, где их хранить.
- SecretKeyOptions.cs - содержит ключ для доступа к admin.html
- Эти настройки можно указать в appsettings.json, иначе заполнятся стандартными значениями

## wwwroot/

- index.html - главная страница автомата - можно попасть по пути "/"

  ![alt text](https://i.imgur.com/EuKWj2U.png)

- admin.html - страница управления водой - можно попасть по пути "/admin?key=secret",
  где "secret" значение свойства SecretKey из SecretKeyOptions.cs

  ![alt text](https://i.imgur.com/WbZOwBR.png)

- index.js - скрипт js для управления страницей index.html
- admin.js - скрипт js для управления страницей admin.html
