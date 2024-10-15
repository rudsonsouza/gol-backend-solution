namespace gol_backend_api.Models;

public class RevisionDTO
{
    public int Id { get; set; }
    public int Km { get; set; }
    public DateTime Data { get; set; }
    public decimal ValorDaRevisao { get; set; }
}