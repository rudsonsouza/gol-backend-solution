namespace gol_backend_api.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public int Ano { get; set; }
    public string Cor { get; set; }
    public string Modelo { get; set; }
    public ICollection<Revision> Revisoes { get; set; } = new List<Revision>();
}


public class VehicleCreator
{
    public static Vehicle CreateVehicle(VehicleDTO vehicle)
    {
        var revisoes = new List<Revision>();
        foreach (var item in vehicle.Revisoes)
        {
            revisoes.Add(
                new Revision()
                {
                    Id = item.Id,
                    Data = item.Data,
                    Km = item.Km,
                    ValorDaRevisao = item.ValorDaRevisao
                });
        }
        if (vehicle.CapacidadeCarga > 0)
            return new Truck() {Placa = vehicle.Placa, Ano = vehicle.Ano, Cor = vehicle.Cor, Modelo = vehicle.Modelo, CapacidadeCarga = vehicle.CapacidadeCarga, Revisoes = revisoes};
        else return new Car(){Placa = vehicle.Placa, Ano = vehicle.Ano, Cor = vehicle.Cor, Modelo = vehicle.Modelo, CapacidadePassageiro = vehicle.CapacidadePassageiro, Revisoes = revisoes};
    }
}