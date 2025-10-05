using System.ComponentModel.DataAnnotations;
using VIKTALEA_Backend.Shared;

namespace VIKTALEA_Backend.Models
{
    public class Clientes : Controls
    {
        public int Id { get; set; }

        [Required, StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "RUC inválido")]
        public string Ruc { get; set; } = default!;

        [Required, StringLength(255)]
        public string RazonSocial { get; set; } = default!;

        [StringLength(50)]
        public string? TelefonoContacto { get; set; }

        [EmailAddress, StringLength(255)]
        public string? CorreoContacto { get; set; }

        [StringLength(500)]
        public string? Direccion { get; set; }

    }
}
