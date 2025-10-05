using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using VIKTALEA_Backend.Schemas;
using VIKTALEA_Backend.Services;
using VIKTALEA_Backend.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VIKTALEA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService service; 
        public ClienteController(IClienteService service)
{
            this.service = service;
        }

        
        // GET: api/<ClienteController>
        [HttpGet]
        public async Task<ApiResponse<List<ClienteDTO.ListCliente>>> List([FromQuery] string? ruc, string? razonSocial, int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            return await service.ListAsync(ruc, razonSocial, page, pageSize);
        }

        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public async Task<ApiResponse<ClienteDTO.ResponseCliente?>> Get(int id, CancellationToken ct)
        {
            return await service.GetByIdAsync(id, ct);
        }

        // POST api/<ClienteController>
        [HttpPost]
        public async Task<ApiResponse<ClienteDTO.ResponseCliente?>> Post([FromBody] ClienteDTO.CreateCliente value, CancellationToken ct)
        {
            var cliente = await service.CreateAsync(value, ct);
            return cliente;
        }

        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public async Task<ApiResponse<ClienteDTO.ResponseCliente>> Put(int id, [FromBody] ClienteDTO.UpdateCliente value, CancellationToken ct)
        {
            var cliente = await service.UpdateAsync(id, value, ct);
            return cliente;
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return service.DeleteAsync(id, CancellationToken.None);
        }
    }
}
