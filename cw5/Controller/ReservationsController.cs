using Microsoft.AspNetCore.Mvc;
using s32429cw5apbd.Data;
using s32429cw5apbd.Models;

namespace s32429cw5apbd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    // GET /api/reservations
    // GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        IEnumerable<Reservation> result = InMemoryStore.Reservations;

        if (date.HasValue)
            result = result.Where(r => r.Date == date.Value);

        if (!string.IsNullOrWhiteSpace(status))
            result = result.Where(r => string.Equals(r.Status, status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            result = result.Where(r => r.RoomId == roomId.Value);

        return Ok(result.ToList());
    }

    // GET /api/reservations/{id}
    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = InMemoryStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Rezerwacja o id {id} nie istnieje.");

        return Ok(reservation);
    }

    // POST /api/reservations
    [HttpPost]
    public ActionResult<Reservation> Create([FromBody] Reservation reservation)
    {
        var businessError = ValidateBusinessRules(reservation, excludeReservationId: null, out var statusCode);
        if (businessError is not null)
            return StatusCode(statusCode, businessError);

        reservation.Id = InMemoryStore.NextReservationId();
        InMemoryStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    // PUT /api/reservations/{id} - pełna aktualizacja
    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Update(int id, [FromBody] Reservation updated)
    {
        var reservation = InMemoryStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Rezerwacja o id {id} nie istnieje.");

        var businessError = ValidateBusinessRules(updated, excludeReservationId: id, out var statusCode);
        if (businessError is not null)
            return StatusCode(statusCode, businessError);

        reservation.RoomId = updated.RoomId;
        reservation.OrganizerName = updated.OrganizerName;
        reservation.Topic = updated.Topic;
        reservation.Date = updated.Date;
        reservation.StartTime = updated.StartTime;
        reservation.EndTime = updated.EndTime;
        reservation.Status = updated.Status;

        return Ok(reservation);
    }

    // DELETE /api/reservations/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = InMemoryStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Rezerwacja o id {id} nie istnieje.");

        InMemoryStore.Reservations.Remove(reservation);
        return NoContent();
    }

    // weryfikacja reguł biznesowych dla rezerwacji
    private static string? ValidateBusinessRules(Reservation reservation, int? excludeReservationId, out int statusCode)
    {
        var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
        {
            statusCode = StatusCodes.Status404NotFound;
            return $"Sala o id {reservation.RoomId} nie istnieje.";
        }

        if (!room.IsActive)
        {
            statusCode = StatusCodes.Status409Conflict;
            return $"Sala o id {reservation.RoomId} jest nieaktywna - nie można jej rezerwować.";
        }

        var overlaps = InMemoryStore.Reservations.Any(r =>
            r.Id != excludeReservationId &&
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.Status != "cancelled" &&
            reservation.StartTime < r.EndTime &&
            r.StartTime < reservation.EndTime);

        if (overlaps)
        {
            statusCode = StatusCodes.Status409Conflict;
            return "Rezerwacja koliduje czasowo z istniejącą rezerwacją tej samej sali.";
        }

        statusCode = StatusCodes.Status200OK;
        return null;
    }
}
