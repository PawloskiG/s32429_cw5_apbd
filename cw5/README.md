# s32429cw5apbd

ASP.NET Core Web API (kontrolery) do zarządzania salami dydaktycznymi i ich rezerwacjami.
Dane przechowywane są **wyłącznie w pamięci aplikacji** (statyczne listy inicjalizowane przy starcie) - bez bazy danych i bez EF Core.


## Endpointy - sale

| Metoda | Endpoint | Opis |
|--------|----------|------|
| GET | `/api/rooms` | Wszystkie sale |
| GET | `/api/rooms/{id}` | Sala po id (id z trasy) |
| GET | `/api/rooms/building/{buildingCode}` | Sale z budynku (buildingCode z trasy) |
| GET | `/api/rooms?minCapacity=20&hasProjector=true&activeOnly=true` | Filtrowanie po query stringu |
| POST | `/api/rooms` | Nowa sala (body JSON) → 201 Created |
| PUT | `/api/rooms/{id}` | Pełna aktualizacja sali |
| DELETE | `/api/rooms/{id}` | Usunięcie sali → 204 / 409 |

## Endpointy - rezerwacje

| Metoda | Endpoint | Opis |
|--------|----------|------|
| GET | `/api/reservations` | Wszystkie rezerwacje |
| GET | `/api/reservations/{id}` | Rezerwacja po id |
| GET | `/api/reservations?date=2026-05-10&status=confirmed&roomId=2` | Filtrowanie po query stringu |
| POST | `/api/reservations` | Nowa rezerwacja (body JSON) → 201 / 404 / 409 |
| PUT | `/api/reservations/{id}` | Pełna aktualizacja rezerwacji |
| DELETE | `/api/reservations/{id}` | Usunięcie rezerwacji → 204 |

## Kody statusu HTTP

- **200 OK** - poprawny odczyt / aktualizacja
- **201 Created** - utworzony zasób (`CreatedAtAction`)
- **204 No Content** - poprawne usunięcie
- **400 Bad Request** - błędne dane wejściowe (walidacja `[ApiController]` + Data Annotations)
- **404 Not Found** - zasób nie istnieje
- **409 Conflict** - kolizja czasowa rezerwacji / sala nieaktywna / usunięcie sali z przyszłymi rezerwacjami

## Walidacja danych wejściowych

- `Name`, `BuildingCode`, `OrganizerName`, `Topic` - niepuste.
- `Capacity` - większe od zera.
- `EndTime` - późniejsze niż `StartTime`.
- `Status` - jedna z wartości `planned`, `confirmed`, `cancelled`.

## Przykładowe body

**POST /api/rooms**

```json
{
  "name": "Lab 204",
  "buildingCode": "B",
  "floor": 2,
  "capacity": 24,
  "hasProjector": true,
  "isActive": true
}
```

**POST /api/reservations**

```json
{
  "roomId": 2,
  "organizerName": "Anna Kowalska",
  "topic": "Warsztaty z HTTP i REST",
  "date": "2026-05-10",
  "startTime": "10:00:00",
  "endTime": "12:30:00",
  "status": "confirmed"
}
```

## Scenariusze testowe (Postman)

1. `GET /api/rooms` - wszystkie sale.
2. `GET /api/rooms/1` - jedna sala.
3. `GET /api/rooms/building/A` - sale z budynku A.
4. `GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true` - filtrowanie.
5. `POST /api/rooms` - dodanie sali 201.
6. `PUT /api/rooms/1` - pełna aktualizacja 200.
7. `POST /api/reservations` (room 2, 2026-05-11 wieczorem) - poprawna rezerwacja 201.
8. `POST /api/reservations` (room 1, 2026-05-10 11:00–12:00) - kolizja **409**.
9. `DELETE /api/reservations/5` - usunięcie 204.
10. `GET /api/rooms/999` - nieistniejąca sala **404**.
11. `POST /api/reservations` z `endTime` < `startTime` - błędne dane **400**.
12. `POST /api/reservations` dla sali 5 (nieaktywna) - **409**.
