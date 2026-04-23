# Subtrack

A web-based subscription manager built with ASP.NET Core MVC. Users can track their monthly and yearly subscriptions (Netflix, Spotify, gym, software, etc.), view total expenses, and analyze spending by category.

School project, 11th grade, 2025/2026.

## Features

### For users
- Register and log in with email and password
- Add, edit, and delete personal subscriptions
- Set billing cycle (Monthly or Yearly), price, and next payment date
- Mark subscriptions as active or paused
- View a personal dashboard with:
  - Total monthly expenses
  - Total yearly expenses
  - Number of active subscriptions
  - Top 3 most expensive subscriptions
  - Spending grouped by category

### For administrators
- Manage shared categories (create, edit, delete)
- View the list of registered users
- All user features above

## Tech stack

- **ASP.NET Core MVC** (.NET 10)
- **Entity Framework Core** (Code First) with **SQLite**
- **ASP.NET Core Identity** for authentication and role management
- **Bootstrap 5** for UI
- **Razor Views** with Tag Helpers

## Architecture
Browser -> Controller -> Service (via Interface) -> EF Core -> SQLite

- **Controllers** handle HTTP requests and call services. No business logic inside controllers.
- **Services** contain all business logic (CRUD rules, calculations, ownership checks, uniqueness checks).
- **DTOs** separate what the database stores from what flies over HTTP (protection against field tampering).
- **Identity** handles registration, login, password hashing, and roles.

## Project structure
SubtrackProject/
├── Controllers/       MVC controllers (thin  only routing and view selection)
├── Data/              DbContext and database seeding
├── DTOs/              Data transfer objects
│   ├── Subscription/
│   ├── Category/
│   └── Dashboard/
├── Models/            EF Core entities and enums
│   └── Base/          BaseEntity
├── Services/          Business logic behind interfaces
│   └── Interfaces/
├── Views/             Razor views styled with Bootstrap
├── Migrations/        EF Core migrations
├── wwwroot/           Static files (Bootstrap, CSS, JS)
└── Program.cs         App configuration, DI, middleware

## Running the project

### Prerequisites
- .NET 10 SDK
- Any IDE (JetBrains Rider, Visual Studio, VS Code)

### Steps

1. Clone the repository:
```bash
   git clone <https://github.com/codingburgas/2526-11V-SD-NVBalandin22.git>
   cd SubtrackProject/SubtrackProject
```

2. Restore NuGet packages and local tools:
```bash
   dotnet restore
   dotnet tool restore
```

3. Apply migrations (creates `subtrack.db`):
```bash
   dotnet tool run dotnet-ef database update
```

4. Run:
```bash
   dotnet run
```

5. Open in browser: `https://localhost:5249` (port may vary).

## Default admin account

Seeded automatically on first startup:

- **Email:** `admin@subtrack.com`
- **Password:** `admin123`

Newly registered users are automatically assigned the `User` role.

## Security

Three layers of access control:

1. `[Authorize]`  user must be logged in
2. `[Authorize(Roles = "Admin")]`  user must have the Admin role
3. User-scoped filtering in services  users can only access their own subscriptions, even if they tamper with URLs

Additional protections:

- **CSRF**  `[ValidateAntiForgeryToken]` on every POST action
- **Password hashing**  handled by ASP.NET Core Identity
- **Input validation**  Data Annotations + `IValidatableObject` for cross-field rules
- **Unique category names**  enforced in the service layer (case-insensitive)

## Calculations

All totals are **normalized to a monthly base** for fair comparison:

- Monthly subscriptions: taken as-is
- Yearly subscriptions: divided by 12

A yearly plan of 120 EUR and a monthly plan of 10 EUR are treated equally in rankings.

## Database model

Entities:

- `ApplicationUser` (extends `IdentityUser`)  has many `Subscription`
- `Category`  has many `Subscription`
- `Subscription`  belongs to one `ApplicationUser` and one `Category`

Relationships:

- User -> Subscription: **Cascade** delete (deleting a user removes their subscriptions)
- Category -> Subscription: **Restrict** delete (a category cannot be deleted if it has subscriptions)

## Notable implementation details

- **Auto-calculation of next payment date** on the Create form via JavaScript, with a server-side fallback.
- **Invariant culture** is forced in `Program.cs` to avoid decimal parsing issues in SQLite.
- **Service layer** returns error messages (`Task<string?>`) for business rules that cannot be expressed via Data Annotations (e.g. unique category name).

## Author

Nikita Balandin - 11V

## License

Educational use only.
