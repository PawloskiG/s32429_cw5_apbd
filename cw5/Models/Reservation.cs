using System.ComponentModel.DataAnnotations;

namespace s32429cw5apbd.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "RoomId musi być dodatnie.")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "OrganizerName jest wymagane.")]
    [MaxLength(100)]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Topic jest wymagane.")]
    [MaxLength(200)]
    public string Topic { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date jest wymagane.")]
    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "StartTime jest wymagane.")]
    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "EndTime jest wymagane.")]
    public TimeOnly EndTime { get; set; }

    // planned, confirmed, cancelled
    [Required(ErrorMessage = "Status jest wymagany.")]
    [RegularExpression("^(planned|confirmed|cancelled)$",
        ErrorMessage = "Status musi być jedną z wartości: planned, confirmed, cancelled.")]
    public string Status { get; set; } = "planned";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(OrganizerName))
        {
            yield return new ValidationResult(
                "OrganizerName nie może być pustym ciągiem znaków.",
                new[] { nameof(OrganizerName) });
        }

        if (string.IsNullOrWhiteSpace(Topic))
        {
            yield return new ValidationResult(
                "Topic nie może być pustym ciągiem znaków.",
                new[] { nameof(Topic) });
        }

        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "EndTime musi być późniejsze niż StartTime.",
                new[] { nameof(EndTime), nameof(StartTime) });
        }
    }
}
