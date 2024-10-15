using gol_backend_api.Data;
using gol_backend_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gol_backend_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController: ControllerBase
{
    private readonly AppDbContext _context;

    public VehiclesController(AppDbContext context)
    {
        _context = context;
    }
    
    // GET: api/products
    [HttpGet()]
    public async Task<ActionResult> GetVehicles(int page = 1, int pageSize = 10)
    {
        var query = _context.Veiculos.Include(v => v.Revisoes).AsQueryable();
        
        var totalRecords = await query.CountAsync();
        var vehicles = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        
        var response = new
        {
            Page = page,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            Vehicles = vehicles
        };

        return Ok(response);
    }
    
    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicle(int id)
    {
        var veiculo = await _context.Veiculos
            .Include(v => v.Revisoes)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (veiculo == null)
        {
            return NotFound();
        }

        return veiculo;
    }
    
    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Vehicle>> PostVehicle(VehicleDTO vehicle)
    {
        var vehicleCreate = VehicleCreator.CreateVehicle(vehicle);
        
        _context.Veiculos.Add(vehicleCreate);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetVehicle", new { id = vehicleCreate.Id }, vehicle);
    }
    
    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, VehicleDTO vehicle)
    {
        if (id != vehicle.Id)
        {
            return BadRequest();
        }

        var vehicleModel = _context.Veiculos
            .Include(p => p.Revisoes)
            .FirstOrDefault(p => p.Id == vehicle.Id);

        vehicleModel.Modelo = vehicle.Modelo;
        vehicleModel.Ano = vehicle.Ano;
        vehicleModel.Cor = vehicle.Cor;
        vehicleModel.Placa = vehicle.Placa;

        foreach (var revisao in vehicleModel.Revisoes)
        {
            revisao.Data = revisao.Data;
            revisao.Km = revisao.Km;
            revisao.ValorDaRevisao = revisao.ValorDaRevisao;
            revisao.VeiculoId = vehicleModel.Id;
        }
        
        var revisionRemove = vehicleModel.Revisoes.FirstOrDefault(r => r.Id == vehicleModel.Revisoes.First().Id);
        if (revisionRemove != null) _context.Revisoes.Remove(revisionRemove);

        foreach (var revision in vehicle.Revisoes)
        {
            var newRevision = new Revision()
            {
                Data = revision.Data,
                Km = revision.Km,
                ValorDaRevisao = revision.ValorDaRevisao,
                Veiculo = vehicleModel,
                VeiculoId = vehicleModel.Id
            };
            vehicleModel.Revisoes.Add(newRevision);
        }
        
        _context.Entry(vehicleModel).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VehicleExistsAsync(id).Result)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    
    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task DeleteProduct(int id)
    {
        var vehicleModel = _context.Veiculos
            .Include(p => p.Revisoes) // If needed
            .FirstOrDefault(p => p.Id == id);

        if (vehicleModel != null)
        {
            // Step 2: Remove the parent entity
            _context.Veiculos.Remove(vehicleModel);

            // Step 3: Save changes
            _context.SaveChanges();
        }
    }
    
    
    private async Task<bool> VehicleExistsAsync(int id)
    {
        return await _context.Veiculos.AnyAsync(e => e.Id == id);
    }
}