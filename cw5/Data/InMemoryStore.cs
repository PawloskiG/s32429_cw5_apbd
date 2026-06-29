using s32429cw5apbd.Models;

namespace s32429cw5apbd.Data;

/// <summary>
/// Prosty magazyn danych w pamięci aplikacji.
/// Listy są statyczne i inicjalizowane przy starcie programu.
/// Brak bazy danych / EF Core na tym etapie.
/// </summary>
public static class InMemoryStore
{
    public static readonly List<Room> Rooms = new()
    {
        new Room { Id = 1, Name = "Sala 101",  BuildingCode = "A", Floor = 1, Capacity = 30, HasProjector = true,  IsActive = true },
        new Room { Id = 2, Name = "Lab 204",   BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true,  IsActive = true },
        new Room { Id = 3, Name = "Sala 010",  BuildingCode = "A", Floor = 0, Capacity = 12, HasProjector = false, IsActive = true },
        new Room { Id = 4, Name = "Aula 300",  BuildingCode = "C", Floor = 3, Capacity = 120, HasProjector = true, IsActive = true },
        new Room { Id = 5, Name = "Sala 105",  BuildingCode = "A", Floor = 1, Capacity = 18, HasProjector = false, IsActive = false },
    };

    public static readonly List<Reservation> Reservations = new()
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Anna Kowalska", Topic = "Warsztaty z HTTP i REST", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 30), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 1, OrganizerName = "Jan Nowak",     Topic = "Konsultacje projektowe",   Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(14, 0),  Status = "planned" },
        new Reservation { Id = 3, RoomId = 2, OrganizerName = "Maria Wiśniewska", Topic = "Szkolenie z C#",         Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(9, 0),  EndTime = new TimeOnly(11, 0),  Status = "confirmed" },
        new Reservation { Id = 4, RoomId = 4, OrganizerName = "Piotr Zieliński", Topic = "Wykład otwarty",          Date = new DateOnly(2026, 6, 1),  StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(17, 0),  Status = "confirmed" },
        new Reservation { Id = 5, RoomId = 3, OrganizerName = "Katarzyna Lis",   Topic = "Spotkanie zespołu",       Date = new DateOnly(2026, 6, 2),  StartTime = new TimeOnly(8, 0),  EndTime = new TimeOnly(9, 0),   Status = "planned" },
    };

    private static int _nextRoomId = 6;
    private static int _nextReservationId = 6;

    public static int NextRoomId() => _nextRoomId++;
    public static int NextReservationId() => _nextReservationId++;
}
