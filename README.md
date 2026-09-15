# ToDoApp

ToDoApp — це повноцінний застосунок для управління задачами з розділенням на frontend та backend. Проєкт складається з React/Vite клієнтської частини та ASP.NET Core API на C#, які взаємодіють через REST API. Дані зберігаються в SQL Server за допомогою Entity Framework Core.

## 1. Огляд проєкту

Проєкт реалізує базовий, але повноцінний workflow для роботи зі списком задач:

- створення задачі;
- перегляд усіх задач;
- перегляд задачі за ID;
- оновлення задачі;
- видалення задачі;
- фільтрація/поділ задач за статусом;
- візуальне представлення задач у вигляді колонок (ToDo / In Progress / Done);
- валідація даних на бекенді;
- інтеграція через MediatR та CQRS-підхід.

## 2. Технологічний стек

### Frontend

- React 19
- TypeScript
- Vite
- Redux Toolkit
- Axios
- Ant Design
- Day.js

### Backend

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- MediatR
- FluentValidation
- AutoMapper
- Swagger / OpenAPI

### Тести

- xUnit
- Moq
- coverlet

## 3. Архітектура проєкту

Проєкт побудований у стилі розділених шарів:

- Domain — доменні моделі та енумерації.
- Application — бізнес-логіка, команди, запити, інтерфейси репозиторіїв.
- Infrastructure — реалізація доступу до даних, DbContext, репозиторій.
- API — REST-контролери, валідація, middleware, налаштування сервіса.
- Frontend — React UI, Redux store, асинхронні запити до API.

## 4. Структура репозиторію

```text
ToDoApp/
├── README.md
├── ToDoApp_Front/
│   ├── package.json
│   ├── ToDoApp_Front.sln
│   └── todoapp_front/
│       ├── package.json
│       ├── vite.config.ts
│       ├── index.html
│       ├── README.md
│       ├── eslint.config.js
│       ├── tsconfig.json
│       ├── tsconfig.app.json
│       ├── tsconfig.node.json
│       ├── public/
│       └── src/
│           ├── App.tsx
│           ├── App.css
│           ├── main.tsx
│           ├── index.css
│           ├── app/
│           │   └── store.ts
│           └── features/
│               └── tasks/
│                   ├── CreateTaskButton.tsx
│                   ├── TaskCard.tsx
│                   ├── TaskCard.css
│                   ├── TaskList.tsx
│                   ├── TaskList.css
│                   ├── tasksSlice.ts
│                   └── ...
└── ToDoProject/
    ├── ToDoProject.sln
    ├── TestProject/
    │   ├── CreateTaskCommandTests.cs
    │   ├── UpdateTaskCommandTests.cs
    │   └── TestProject.csproj
    ├── TodoApp.API/
    │   ├── Program.cs
    │   ├── appsettings.json
    │   ├── appsettings.Development.json
    │   ├── ToDoApp.API.csproj
    │   ├── ToDoApp.API.http
    │   ├── Controllers/
    │   │   └── TasksController.cs
    │   ├── Dto/
    │   │   └── TaskItemOutputDto.cs
    │   ├── Mappings/
    │   │   └── TaskMappingProfile.cs
    │   ├── Middlewares/
    │   │   └── ExceptionHandlingMiddleware.cs
    │   ├── Properties/
    │   │   └── launchSettings.json
    │   └── Validators/
    │       ├── CreateTaskCommandValidator.cs
    │       ├── UpdateTaskCommandValidator.cs
    │       └── ValidationBehaviour.cs
    ├── ToDoApp.Application/
    │   ├── Interfaces/
    │   │   ├── IAppDbContext.cs
    │   │   └── ITaskRepository.cs
    │   └── Tasks/
    │       ├── Commands/
    │       │   ├── CreateTaskCommand.cs
    │       │   ├── DeleteTaskCommand.cs
    │       │   └── UpdateTaskCommand.cs
    │       ├── Handlers/
    │       │   ├── CreateTaskCommandHandler.cs
    │       │   ├── DeleteTaskCommandHandler.cs
    │       │   ├── GetAllTaskQueryHandler.cs
    │       │   ├── GetTaskByIdQueryHandler.cs
    │       │   └── UpdateTaskCommandHandler.cs
    │       └── Queries/
    │           ├── GetAllTasksQuery.cs
    │           └── GetTaskByIdQuery.cs
    ├── ToDoApp.Domain/
    │   ├── Enums/
    │   │   └── TaskStatus.cs
    │   └── Models/
    │       └── TaskItem.cs
    └── ToDoApp.Inftrastructure/
        ├── Migrations/
        └── Persistance/
            ├── AppDbContext.cs
            └── TaskRepository.cs
```

## 5. Основні доменні сутності

### TaskItem

Модель задачі має такі поля:

- Id
- Title
- Description
- Status
- Deadline
- CreatedAt
- UpdatedAt

### TaskProgressStatus

Статуси задач визначені в enum:

- ToDo
- InProgress
- Done

## 6. API

API знаходиться в контролері `TasksController` і працює за маршрутом `/Tasks`.

### 6.1. Створення задачі

- Method: `POST`
- Route: `/Tasks`
- Body: `CreateTaskCommand`

Приклад:

```json
{
  "title": "Написати README",
  "description": "Підготувати детальний опис для проєкту",
  "status": "ToDo",
  "deadline": "2026-10-01T00:00:00Z"
}
```

### 6.2. Отримати всі задачі

- Method: `GET`
- Route: `/Tasks`

### 6.3. Отримати задачу за ID

- Method: `GET`
- Route: `/Tasks/{id}`

### 6.4. Оновити задачу

- Method: `PATCH`
- Route: `/Tasks/{id}`

### 6.5. Видалити задачу

- Method: `DELETE`
- Route: `/Tasks/{id}`

## 7. Валідація

Валідація даних реалізована через `FluentValidation`.

Для створення задачі перевіряються:

- Title не може бути порожнім;
- максимальна довжина Title — 50 символів;
- Description не може перевищувати 500 символів;
- Deadline, якщо вказаний, має бути не раніше поточного дня.

## 8. База даних

Проєкт використовує SQL Server і підключення задається в `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoAppDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

Це означає, що для запуску потрібно мати локальний SQL Server або доступний екземпляр на `localhost` із Windows-аутентифікацією.

## 9. Передумови для запуску

### Для backend

- .NET 9 SDK
- SQL Server
- доступ до локального SQL Server на `localhost`

### Для frontend

- Node.js 18+
- npm або yarn

## 10. Запуск проєкту

### 10.1. Клонування репозиторію

```bash
git clone <repository-url>
cd ToDoApp
```

### 10.2. Запуск backend

Перейдіть до каталогу проєкту:

```bash
cd ToDoProject
```

Відновіть пакети:

```bash
dotnet restore
```

Запустіть API:

```bash
dotnet run --project TodoApp.API/ToDoApp.API.csproj
```

Після запуску сервіс буде доступний за адресами:

- HTTPS: `https://localhost:7233`
- HTTP: `http://localhost:5233`

У режимі development включені:

- Swagger UI
- OpenAPI endpoint

### 10.3. Запуск frontend

Відкрийте інший термінал і виконайте:

```bash
cd ToDoApp_Front/todoapp_front
npm install
npm run dev
```

Після цього Vite запустить розробницький сервер, зазвичай на:

- `http://localhost:5173`

## 11. Робота з базою даних

У проєкті використовуються міграції Entity Framework Core.

Щоб застосувати міграції або створити базу даних, можна виконати:

```bash
dotnet ef database update --project ToDoProject/ToDoApp.Inftrastructure/ToDoApp.Inftrastructure.csproj --startup-project ToDoProject/TodoApp.API/ToDoApp.API.csproj
```

Якщо `dotnet ef` не встановлений, спочатку виконайте:

```bash
dotnet tool install --global dotnet-ef
```

## 12. Особливості UI

Frontend реалізує канбан-подібне відображення задач:

- колонки: `ToDo`, `InProgress`, `Done`;
- кожна задача відображається як картка;
- можливість редагування задачі через модальне вікно;
- видалення напряму з картки;
- завантаження списку задач при старті;
- оновлення state через Redux Toolkit.

## 13. API та клієнтська інтеграція

Frontend робить запити до бекенду за адресою:

```ts
https://localhost:7233/Tasks
```

Конфігурація CORS у backend дозволяє доступ з будь-якого origin, що спрощує локальну розробку між frontend і backend.

## 14. Тести

Тести знаходяться в директорії:

- `ToDoProject/TestProject`

Запуск всіх тестів:

```bash
cd ToDoProject
dotnet test
```

## 15. Поточні приналежності та структура логіки

### Backend

- `TasksController` — точка входу для HTTP-запитів;
- `MediatR` — обробка команд і запитів;
- `TaskRepository` — робота з базою даних;
- `AppDbContext` — DbContext для `TaskItem`;
- `ValidationBehaviour` — глобальна валідація команд через пайплайн MediatR;
- `ExceptionHandlingMiddleware` — централізована обробка помилок.

### Frontend

- `tasksSlice.ts` — Redux slice для API-взаємодії;
- `TaskList.tsx` — головний UI компонент зі списком задач і колонками;
- `CreateTaskButton.tsx` — кнопка створення нової задачі;
- `TaskCard.tsx` — картка задачі;
- `store.ts` — конфігурація Redux store.

## 16. Приклади типових сценаріїв

### Створити задачу

1. Увійти в frontend.
2. Натиснути кнопку створення задачі.
3. Заповнити title, description, deadline та статус.
4. Відправити форму.
5. Задача з'явиться у відповідній колонці.

### Редагувати задачу

1. Натиснути кнопку редагування на картці задачі.
2. Внести зміни у форму.
3. Зберегти зміни.
4. Дані оновляться в API і Redux store.

### Видалити задачу

1. Натиснути кнопку видалення.
2. Запит йде до `/Tasks/{id}`.
3. Задача видаляється із списку.

## 17. Відомі важливі моменти

- Бекенд використовує `UseHttpsRedirection`, тому під час локального запуску може з'являтися перенаправлення на HTTPS.
- Frontend звертається до API за адресою `https://localhost:7233`.
- У проекті використовується `AllowAll` CORS для простоти локальної розробки.
- У `Program.cs` налаштовано JSON enum converter, тому enum значення можуть відправлятися у вигляді рядків у JSON.

## 18. Рекомендації для розвитку

- додати авторизацію та аутентифікацію;
- розширити домен задач вітками, пріоритетами та тегами;
- додати вхідні DTO для створення/оновлення та окремі схеми валідації;
- додати сторінку деталей задачі;
- реалізувати пагінацію для великих списків;
- додати unit-тести для handlers та repository.

## 19. Висновок

ToDoApp — це приклад практичного проєкту, який поєднує сучасну React-архітектуру з чистою .NET-архітектурою на основі CQRS/MediatR. Проєкт демонструє типову структуру для повнофункціонального додатку з REST API, базою даних, UI, валідаторами, middleware та тестами.

## 20. Корисні команди швидкого старту

### Backend

```bash
cd ToDoProject
dotnet restore
dotnet run --project TodoApp.API/ToDoApp.API.csproj
```

### Frontend

```bash
cd ToDoApp_Front/todoapp_front
npm install
npm run dev
```

### Тести

```bash
cd ToDoProject
dotnet test
```

---

Якщо потрібно, я можу також підготувати:

1. коротку версію README для GitHub;
2. більш технічну версію для команди розробників;
3. README українською або англійською мовою;
4. README з конкретними інструкціями під ваш середовище (Windows/macOS/Linux).
