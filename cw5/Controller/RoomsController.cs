using Microsoft.AspNetCore.Mvc;
using s32429cw5apbd.Data;
using s32429cw5apbd.Models;

namespace s32429cw5apbd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    // GET /api/rooms
    // GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        IEnumerable<Room> result = InMemoryStore.Rooms;

        if (minCapacity.HasValue)
            result = result.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            result = result.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly == true)
            result = result.Where(r => r.IsActive);

        return Ok(result.ToList());
    }

    // GET /api/rooms/{id}
    [HttpGet("{id:int}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Sala o id {id} nie istnieje.");

        return Ok(room);
    }

    // GET /api/rooms/building/{buildingCode}
    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
    {
        var rooms = InMemoryStore.Rooms
            .Where(r => string.Equals(r.BuildingCode, buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    // POST /api/rooms
    [HttpPost]
    public ActionResult<Room> Create([FromBody] Room room)
    {
        room.Id = InMemoryStore.NextRoomId();
        InMemoryStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    // PUT /api/rooms/{id} - pełna aktualizacja
    [HttpPut("{id:int}")]
    public ActionResult<Room> Update(int id, [FromBody] Room updated)
    {
        var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Sala o id {id} nie istnieje.");

        room.Name = updated.Name;
        room.BuildingCode = updated.BuildingCode;
        room.Floor = updated.Floor;
        room.Capacity = updated.Capacity;
        room.HasProjector = updated.HasProjector;
        room.IsActive = updated.IsActive;

        return Ok(room);
    }

    // DELETE /api/rooms/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = InMemoryStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Sala o id {id} nie istnieje.");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var hasFutureReservations = InMemoryStore.Reservations
            .Any(r => r.RoomId == id && r.Date >= today && r.Status != "cancelled");

        if (hasFutureReservations)
            return Conflict($"Nie można usunąć sali o id {id} - istnieją powiązane przyszłe rezerwacje.");

        InMemoryStore.Rooms.Remove(room);
        return NoContent();
    }
}
