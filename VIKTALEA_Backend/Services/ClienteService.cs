using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Experimental;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using VIKTALEA_Backend.Models;
using VIKTALEA_Backend.Schemas;
using VIKTALEA_Backend.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VIKTALEA_Backend.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _dbContext;
        public ClienteService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<List<ClienteDTO.ListCliente>>> ListAsync(string? ruc, string? razonSocial, int? active, int page, int pageSize)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize is <= 0 or > 100 ? 20 : pageSize;

            var query = _dbContext.Clientes.AsNoTracking().AsQueryable();

            if (active == 1)
            {
                query = query.Where(x => x.activate == 1);
            }
            if (active == 0)
            {
                query = query.Where(x => x.activate == active);
            }
            if (!string.IsNullOrWhiteSpace(ruc))
            {
                query = query.Where(c => c.Ruc.Contains(ruc) || EF.Functions.Like(c.Ruc, $"%{ruc}%"));
            }
            if(!string.IsNullOrWhiteSpace(razonSocial))
            {
                query = query.Where(c => c.RazonSocial.Contains(razonSocial) || EF.Functions.Like(c.RazonSocial.ToUpper(), $"%{razonSocial}%"));
            }

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var clientes = await query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClienteDTO.ListCliente
                {
                    Id = c.Id,
                    Ruc = c.Ruc,
                    RazonSocial = c.RazonSocial,
                    TelefonoContacto = c.TelefonoContacto,
                    CorreoContacto = c.CorreoContacto,
                    Direccion = c.Direccion,
                    activate = c.activate,  
                    createdAt = c.createdAt,
                    updatedAt = c.updatedAt
                })
                .ToListAsync();

            var pagination = new Pagination(totalItems, page, pageSize, totalPages);
            return new ApiResponse<List<ClienteDTO.ListCliente>>(
                true,
                "Lista de clientes",
                clientes ?? new List<ClienteDTO.ListCliente>(),
                null,
                pagination
            );
        }
        public async Task<ApiResponse<ClienteDTO.ResponseCliente>?> GetByIdAsync(int id, CancellationToken ct)
        {
            var c = await _dbContext.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (c == null)
            {
                var apiError = new ApiError(
                    "Error",
                    "ID no encontrado"
                );
                return new ApiResponse<ClienteDTO.ResponseCliente>(
                    false,
                    "NOT_FOUND",
                    null,
                    apiError,
                    null
                );
            }

            var response = new ClienteDTO.ResponseCliente()
            {
                Id = c.Id,
                Ruc = c.Ruc,
                RazonSocial = c.RazonSocial,
                TelefonoContacto = c.TelefonoContacto,
                CorreoContacto = c.CorreoContacto,
                Direccion = c.Direccion,
                activate = c.activate,
                createdAt = c.createdAt,
                updatedAt = c.updatedAt
            };
            return c is null ? null : new ApiResponse<ClienteDTO.ResponseCliente>(
                true,
                "SUCCESS",
                response,
                null,
                null
            );
        }

        public async Task<ApiResponse<ClienteDTO.ResponseCliente>> CreateAsync(ClienteDTO.CreateCliente cliente, CancellationToken ct)
        {
            var exists = await _dbContext.Clientes.FirstOrDefaultAsync(x => x.Ruc == cliente.Ruc, ct);
            if (exists != null)
            {
                if (exists.activate == 0)
                {
                    var apiErrorRuc = new ApiError(
                        "Error",
                        "Cliente con RUC existe pero está inactivo"
                    );
                    return new ApiResponse<ClienteDTO.ResponseCliente>(
                        false,
                        "RUC_INACTIVE",
                        null,
                        apiErrorRuc,
                        null
                    );
                }
                // Corrige el error de sintaxis y el tipo de argumento para ApiError
                var apiError = new ApiError(
                    "Error",
                    "RUC ya existente"
                );
                return new ApiResponse<ClienteDTO.ResponseCliente>(
                    false,
                    "RUC_EXISTS",
                    null,
                    apiError,
                    null
                );

            }
            var c = new Clientes
            {
                Ruc = cliente.Ruc,
                RazonSocial = cliente.RazonSocial,
                TelefonoContacto = cliente.TelefonoContacto,
                CorreoContacto = cliente.CorreoContacto,
                Direccion = cliente.Direccion
            };

            _dbContext.Clientes.Add(c);
            await _dbContext.SaveChangesAsync(ct);
            var response = new ClienteDTO.ResponseCliente{
                Id = c.Id,
                Ruc = c.Ruc,
                RazonSocial = c.RazonSocial,
                TelefonoContacto = c.TelefonoContacto,
                CorreoContacto = c.CorreoContacto,
                Direccion = c.Direccion,
                activate = c.activate,
                createdAt = c.createdAt
            };
            return new ApiResponse<ClienteDTO.ResponseCliente>(true, "CREATE_SUCCESS", response, null, null);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var c = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (c == null) return false;
            if (c.activate == 0) return false;
            c.activate = 0;
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ActiveAsync(int id, CancellationToken ct)
        {
            var c = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (c == null) return false;
            if (c.activate == 1) return false;
            c.activate = 1;
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }

        public async Task<ApiResponse<ClienteDTO.ResponseCliente?>> UpdateAsync(int id, ClienteDTO.UpdateCliente cliente, CancellationToken ct)
        {
            var exists = await _dbContext.Clientes.FirstOrDefaultAsync(x => x.Ruc == cliente.Ruc);
            if (exists != null)
            {
                if (exists.activate == 0)
                {
                    var apiErrorRuc = new ApiError(
                        "Error",
                        "Cliente con RUC existe pero está inactivo"
                    );
                    return new ApiResponse<ClienteDTO.ResponseCliente?>(
                        false,
                        "RUC_INACTIVE",
                        null,
                        apiErrorRuc,
                        null
                    );
                }
                if (exists.Ruc != cliente.Ruc)
                {
                    var apiError = new ApiError(
                        "Error",
                        "RUC ya existente"
                    );
                    return new ApiResponse<ClienteDTO.ResponseCliente?>(
                        false,
                        "RUC_EXISTS",
                        null,
                        apiError,
                        null
                    );
                }
            }
            var c = await _dbContext.Clientes.FirstOrDefaultAsync(x => x.Id == id);
            if (c is null)
            {
                var apiError = new ApiError(
                    "Error",
                    "Id no encontrado"
                );
                return new ApiResponse<ClienteDTO.ResponseCliente?>(
                    false,
                    "NOT_FOUND",
                    null,
                    apiError,
                    null
                );
            };
            

            if (!string.Equals(c.Ruc, cliente.Ruc, StringComparison.Ordinal)){
                var duplicado = await _dbContext.Clientes.FirstOrDefaultAsync(x => x.Ruc == cliente.Ruc && x.Id == id,ct);
                if (duplicado != null)
                {
                    var apiError = new ApiError(
                        "Error",
                        "RUC Duplicado"
                    );
                    return new ApiResponse<ClienteDTO.ResponseCliente?>(
                        false,
                        "DUPLICATED_RUC",
                        null,
                        apiError,
                        null
                    );
                }
                ;

            }

            c.Ruc = cliente.Ruc;
            c.RazonSocial = cliente.RazonSocial;
            c.TelefonoContacto = cliente.TelefonoContacto;
            c.CorreoContacto = cliente.CorreoContacto;
            c.Direccion = cliente.Direccion;
            c.activate = cliente.activate;
            c.updatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(ct);

            var response = new ClienteDTO.ResponseCliente
            {
                Id = c.Id,
                Ruc = c.Ruc,
                RazonSocial = c.RazonSocial,
                TelefonoContacto = c.TelefonoContacto,
                CorreoContacto = c.CorreoContacto,
                Direccion = c.Direccion,
                activate = c.activate,
                createdAt = c.createdAt,
                updatedAt = c.updatedAt
            };

            return new ApiResponse<ClienteDTO.ResponseCliente?>(true, "SUCCESS", response, null, null);
        }

    }
}
