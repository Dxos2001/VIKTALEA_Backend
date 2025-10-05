using VIKTALEA_Backend.Models;

namespace VIKTALEA_Backend.Schemas
{
    public class ClienteDTO
    {
        public class ListCliente() : Clientes
        {
        }

        public class CreateCliente()
        {
            public string Ruc { get; set; } = default!;
            public string RazonSocial { get; set; } = default!;
            public string? TelefonoContacto { get; set; }
            public string? CorreoContacto { get; set; }
            public string? Direccion { get; set; }

        }

        public class UpdateCliente()
        { 
            public string Ruc { get; set; }
            public string RazonSocial { get; set; } = default!;
            public string? TelefonoContacto { get; set; }
            public string? CorreoContacto { get; set; }
            public string? Direccion { get; set; }
            public bool activate { get; set; } = true;
            public DateTime? updatedAt { get; set; } = DateTime.UtcNow;
        }

        public class DeleteCliente()
        {
            public bool activate { get; set; } = false;
            public DateTime? updatedAt { get; set; } = DateTime.UtcNow;
        }

        public class ResponseCliente() : Clientes
        {
        }
    }
}
