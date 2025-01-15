# BorAdmin3

**BorAdmin3** to aplikacja napisana w [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) i ASP.NET Core, której celem jest zarządzanie zasobami w sposób efektywny i prosty w obsłudze. Projekt jest rozwijany w celu wsparcia administracji w różnych scenariuszach.

## Funkcjonalności

- **Zarządzanie użytkownikami:** Dodawanie, edytowanie i usuwanie użytkowników.
- **Zarządzanie zasobami:** Obsługa i organizacja zasobów zgodnie z potrzebami użytkowników.
- **Logowanie i autoryzacja:** Obsługa kont użytkowników z mechanizmami logowania i uprawnień.
- **Integracje:** Możliwość integracji z zewnętrznymi API.

## Technologie

- **Frontend:** Blazor WebAssembly (C#)
- **Backend:** ASP.NET Core
- **Baza danych:** [Nazwa używanej bazy danych, np. SQL Server, PostgreSQL, SQLite]
- **Inne:** Entity Framework Core, Logger (Microsoft.Extensions.Logging)

## Instalacja

1. **Klonowanie repozytorium:**
   ```bash
   git clone https://github.com/krzyzako/BorAdmin3.git
   cd BorAdmin3
   ```

2. **Konfiguracja backendu:**
   - Skonfiguruj plik `appsettings.json`, aby połączyć aplikację z bazą danych.

3. **Przygotowanie bazy danych:**
   Uruchom migracje Entity Framework, aby utworzyć strukturę bazy danych:
   ```bash
   dotnet ef database update
   ```

4. **Uruchomienie aplikacji:**
   ```bash
   dotnet run
   ```

5. **Otwórz przeglądarkę:**
   Przejdź pod adres `http://localhost:5000` (lub inny wskazany w konsoli).

## Użycie

1. Zaloguj się na konto administracyjne.
2. Zarządzaj użytkownikami i zasobami w odpowiednich sekcjach aplikacji.

## Wkład w projekt

Jeśli chcesz przyczynić się do rozwoju projektu:

1. Sforkuj repozytorium.
2. Utwórz nową gałąź:
   ```bash
   git checkout -b feature/nazwa-funkcji
   ```
3. Wprowadź zmiany i zatwierdź je:
   ```bash
   git commit -m "Dodano nową funkcjonalność"
   ```
4. Wyślij zmiany do swojego forka:
   ```bash
   git push origin feature/nazwa-funkcji
   ```
5. Otwórz pull request do głównej gałęzi tego repozytorium.

## Licencja

[MIT](LICENSE)

## Kontakt

Jeśli masz pytania lub sugestie dotyczące projektu, skontaktuj się poprzez [GitHub Issues](https://github.com/krzyzako/BorAdmin3/issues).

---
Dziękujemy za zainteresowanie projektem BorAdmin3!

