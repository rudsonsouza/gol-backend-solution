namespace gol_backend_api.Models;

public class VehicleDTO
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public int Ano { get; set; }
    public string Cor { get; set; }
    public string Modelo { get; set; }
    public int CapacidadePassageiro { get; set; }
    public double CapacidadeCarga { get; set; }
    public List<RevisionDTO> Revisoes { get; set; } = new List<RevisionDTO>();
}