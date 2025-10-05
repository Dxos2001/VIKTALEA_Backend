using VIKTALEA_Backend.Shared;
using VIKTALEA_Backend.Schemas;

namespace VIKTALEA_Backend.Services
{
    public interface IClienteService
    {
        Task<ApiResponse<List<ClienteDTO.ListCliente>>> ListAsync(String? ruc, String? razonSocial, int page, int pageSize);
        Task<ApiResponse<ClienteDTO.ResponseCliente?>> GetByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<ClienteDTO.ResponseCliente>> CreateAsync(ClienteDTO.CreateCliente cliente, CancellationToken ct);
        Task<ApiResponse<ClienteDTO.ResponseCliente>> UpdateAsync(int id, ClienteDTO.UpdateCliente cliente, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
