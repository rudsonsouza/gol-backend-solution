namespace gol_backend_api.Models;

public class Revision
{
    public int Id { get; set; }
    public int Km { get; set; }
    public DateTime Data { get; set; }
    public decimal ValorDaRevisao { get; set; }

    public int VeiculoId { get; set; }
    public Vehicle Veiculo { get; set; }
}