# LibraryApi

**Projekt ASP.NET Core Web API** umożliwiający operacje CRUD dla trzech encji: Autor, Książka i Egzemplarz. Rozwiązanie wykorzystuje Entity Framework Core z bazą SQLite i jest gotowe do automatycznego testowania za pomocą dostarczonych w projekcie testów Elm (index.html + main.js).

---

## Spis treści

1. [Opis projektu](#opis-projektu)
2. [Technologie](#technologie)
3. [Wymagania wstępne](#wymagania-wstępne)
4. [Instalacja i uruchomienie](#instalacja-i-uruchomienie)

   * [Klonowanie repozytorium](#klonowanie-repozytorium)
   * [Konfiguracja bazy danych](#konfiguracja-bazy-danych)
   * [Migracje i utworzenie bazy](#migracje-i-utworzenie-bazy)
   * [Uruchomienie projektu](#uruchomienie-projektu)
5. [Testowanie automatyczne](#testowanie-automatyczne)
6. [Opis API](#opis-api)

   * [Autorzy `/authors`](#autorzy-authors)
   * [Książki `/books`](#książki-books)
   * [Egzemplarze `/copies`](#egzemplarze-copies)
7. [CORS i pliki statyczne](#cors-i-pliki-statyczne)
8. [Struktura projektu](#struktura-projektu)
9. [Przykłady użycia](#przykłady-użycia)
10. [Licencja](#licencja)

---

## Opis projektu

Celem projektu jest udostępnienie RESTowego API, które pozwala zarządzać danymi:

* **Autor**: `id`, `first_name`, `last_name`
* **Książka**: `id`, `title`, `year`, `authorId` (przy tworzeniu/edycji), w odpowiedzi zwracany jest obiekt autora.
* **Egzemplarz**: `id`, `bookId`

Zaimplementowane operacje CRUD (GET, POST, PUT, DELETE) dla każdej encji, z walidacją danych (\[Required], \[Range]) oraz DTO dla zwracania zagnieżdżonych struktur.

Projekt działa na platformie .NET 6 (lub .NET 7), wykorzystuje Entity Framework Core z SQLite.

---

## Technologie

* **.NET 6.0 / .NET 7.0**
* **ASP.NET Core Web API**
* **Entity Framework Core** (provider: SQLite)
* **SQLite**
* **C# 10/11**
* **HTML + Elm (index.html + main.js)** do automatycznego testowania API

---

## Wymagania wstępne

* Visual Studio 2022 (wersja z obsługą .NET 6+)
* .NET SDK 6.0 lub 7.0
* Konto GitHub (do publikacji, opcjonalnie)

---

## Instalacja i uruchomienie

### Klonowanie repozytorium

```bash
git clone https://github.com/<Twoj-login>/LibraryApi.git
cd LibraryApi
```

### Konfiguracja bazy danych

Plik `appsettings.json` zawiera connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=Library.db"
}
```

W razie potrzeby można zmienić nazwę pliku lub ścieżkę.

### Migracje i utworzenie bazy

1. Otwórz **Package Manager Console** w VS:

   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```
2. Alternatywnie, aplikacja automatycznie utworzy DB przy pierwszym uruchomieniu (metoda `EnsureCreated()` jeśli w kodzie aktywowana).

### Uruchomienie projektu

1. Wybierz profil **IIS Express** lub **LibraryApi** i naciśnij **F5 / Ctrl+F5**.
2. Aplikacja wystartuje pod adresem, np. `https://localhost:5001`.

---

## Testowanie automatyczne

Do folderu `wwwroot` skopiowane są pliki `index.html` i `main.js` z testami Elm. Po uruchomieniu aplikacji otwórz w przeglądarce:

```
https://localhost:<port>/index.html
```

Testy zostaną uruchomione automatycznie i pokażą wynik (zielone = zaliczone, czerwone = niezaliczone).

---

## Opis API

### Autorzy `/authors`

| Metoda | Endpoint        | Opis                             |
| ------ | --------------- | -------------------------------- |
| GET    | `/authors`      | Pobierz listę wszystkich autorów |
| GET    | `/authors/{id}` | Pobierz autora o podanym id      |
| POST   | `/authors`      | Utwórz nowego autora             |
| PUT    | `/authors/{id}` | Zaktualizuj dane autora          |
| DELETE | `/authors/{id}` | Usuń autora                      |

### Książki `/books`

| Metoda | Endpoint                     | Opis                                                               |
| ------ | ---------------------------- | ------------------------------------------------------------------ |
| GET    | `/books`                     | Pobierz listę wszystkich książek                                   |
| GET    | `/books?authorId={authorId}` | Pobierz książki konkretnego autora                                 |
| GET    | `/books/{id}`                | Pobierz książkę o danym id; zwraca nested obiekt autora            |
| POST   | `/books`                     | Utwórz książkę; JSON: `{ "title":"...","year":2020,"authorId":1 }` |
| PUT    | `/books/{id}`                | Zaktualizuj książkę; analogiczny JSON z `id` i `authorId`          |
| DELETE | `/books/{id}`                | Usuń książkę                                                       |

### Egzemplarze `/copies`

| Metoda | Endpoint       | Opis                                      |
| ------ | -------------- | ----------------------------------------- |
| GET    | `/copies`      | Pobierz listę egzemplarzy                 |
| GET    | `/copies/{id}` | Pobierz egzemplarz o podanym id           |
| POST   | `/copies`      | Utwórz egzemplarz; JSON: `{ "bookId":1 }` |
| PUT    | `/copies/{id}` | Zaktualizuj egzemplarz (zmiana `bookId`)  |
| DELETE | `/copies/{id}` | Usuń egzemplarz                           |

---

## CORS i pliki statyczne

* **CORS**: w `Program.cs` zarejestrowano `AddCors` i `UseCors` z `AllowAnyOrigin`, aby testy Elm mogły swobodnie wywoływać API.
* **Pliki statyczne**: folder `wwwroot` zawiera `index.html` i `main.js`, udostępniane przez `UseStaticFiles()`.

---

## Struktura projektu

```
LibraryApi/             # katalog główny projektu
├─ Controllers/         # kontrolery Web API
│  ├─ AuthorsController.cs
│  ├─ BooksController.cs
│  └─ CopiesController.cs
├─ Data/
│  └─ AppDbContext.cs   # EF Core DbContext
├─ Dtos/                # klasy DTO
│  ├─ AuthorDto.cs
│  ├─ BookDto.cs
│  └─ CopyDto.cs
├─ Models/              # encje bazo­we
│  ├─ Author.cs
│  ├─ Book.cs
│  └─ Copy.cs
├─ wwwroot/             # pliki statyczne (testy Elm)
│  ├─ index.html
│  └─ main.js
├─ appsettings.json     # ustawienia, connection string
├─ Program.cs           # główny plik konfiguracyjny
├─ LibraryApi.csproj    # plik projektu .NET
└─ README.md            # to README
```

---

## Przykłady użycia

### Tworzenie autora

```http
POST /authors HTTP/1.1
Content-Type: application/json

{ "first_name": "Jan", "last_name": "Kowalski" }
```

### Pobranie książek danego autora

```http
GET /books?authorId=1 HTTP/1.1
```

### Utworzenie egzemplarza

```http
POST /copies HTTP/1.1
Content-Type: application/json

{ "bookId": 3 }
```

---

## Licencja

Projekt udostępniony według licencji MIT. Zobacz plik `LICENSE` po więcej informacji.
