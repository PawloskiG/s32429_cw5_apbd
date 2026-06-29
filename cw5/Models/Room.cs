using System.ComponentModel.DataAnnotations;

namespace s32429cw5apbd.Models;

public class Room : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name jest wymagane.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "BuildingCode jest wymagane.")]
    [MaxLength(10)]
    public string BuildingCode { get; set; } = string.Empty;

    [Range(0, 200, ErrorMessage = "Floor musi mieścić się w zakresie 0-200.")]
    public int Floor { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity musi być większe od zera.")]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }

    public bool IsActive { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult(
                "Name nie może być pustym ciągiem znaków.",
                new[] { nameof(Name) });
        }

        if (string.IsNullOrWhiteSpace(BuildingCode))
        {
            yield return new ValidationResult(
                "BuildingCode nie może być pustym ciągiem znaków.",
                new[] { nameof(BuildingCode) });
        }
    }
}
