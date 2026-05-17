using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;
using MMI_SP.Helpers;
using MMI_SP.PatternMatching;

namespace MMI_SP.Insurance.Policies
{
    internal class Insurer
    {
        // ==========================================
        // BLOQUE 1: Datos
        // ==========================================
        private List<VehicleData> _insuredVehicles;
        internal static Insurer Instance { get; private set; }

        internal Insurer()
        {
            Instance = this;
        }

        // ==========================================
        // BLOQUE 2: Funciones públicas
        // ==========================================
        internal void LoadFrom(List<VehicleData> list) => _insuredVehicles = list;

        public static bool IsInsurable(Vehicle veh)
        {
            if (veh == null || !veh.IsAlive) return false;
            return !veh.Model.IsTrain &&
                   (veh.Model.IsCar || veh.Model.IsBike || veh.Model.IsQuadBike ||
                    veh.Model.IsHelicopter || veh.Model.IsPlane || veh.Model.IsBoat);
        }

        internal bool IsInsured(Vehicle veh)
        {
            if (veh == null || !veh.Exists()) return false;
            string id = VehicleIdentifier.Get(veh);
            return _insuredVehicles.Any(v => v.Id == id);
        }

        internal Result<bool> Insure(Vehicle veh)
        {
            return ValidarVehiculo(veh)
                .AndThen(id => VehicleDataFactory.CreateFrom(veh, id))
                .AndThen(data =>
                {
                    _insuredVehicles.Add(data);
                    Logger.Debug($"Vehículo asegurado: {data.Id}");
                    return Repository.Save(_insuredVehicles);
                });
        }

        internal List<string> GetInsuredList() => _insuredVehicles.Select(v => v.Id).ToList();

        internal Result<bool> Cancel(string vehicleId)
        {
            return BuscarVehiculo(vehicleId).Match<Result<bool>>(
                onOk: (VehicleData vehicle) =>
                {
                    _insuredVehicles.Remove(vehicle);
                    Logger.Debug($"Vehículo cancelado: {vehicleId}");
                    return Repository.Save(_insuredVehicles);
                },
                onErr: (string error) => new Err<bool>(error)
            );
        }

        internal Result<bool> MarkAsDestroyed(string vehicleId)
        {
            return BuscarVehiculo(vehicleId).Match<Result<bool>>(
                onOk: (VehicleData vehicle) =>
                {
                    var updated = new VehicleData(
                        vehicle.Id, vehicle.ModelName, vehicle.Plate,
                        vehicle.PrimaryColor, vehicle.SecondaryColor, true,
                        vehicle.WindowTint, vehicle.WheelType, vehicle.WheelColor, vehicle.TireSmokeColor);
                    _insuredVehicles.Remove(vehicle);
                    _insuredVehicles.Add(updated);
                    return new Ok<bool>(true);
                },
                onErr: (string error) => new Err<bool>(error)
            );
        }

        internal Result<bool> UpdateVehicleData(Vehicle veh)
        {
            string id = VehicleIdentifier.Get(veh);
            return BuscarVehiculo(id)
                .AndThen(vehicle => VehicleDataFactory.CreateFrom(veh, id)
                    .AndThen(updated =>
                    {
                        _insuredVehicles.Remove(vehicle);
                        _insuredVehicles.Add(updated);
                        return new Ok<bool>(true);
                    })
                );
        }

        // ==========================================
        // BLOQUE 3: Funciones privadas (validaciones)
        // ==========================================
        private Result<string> ValidarVehiculo(Vehicle veh)
        {
            if (veh == null || !veh.Exists()) return new Err<string>("El vehículo no existe.");
            string id = VehicleIdentifier.Get(veh);
            if (_insuredVehicles.Any(v => v.Id == id)) return new Err<string>("El vehículo ya está asegurado.");
            return new Ok<string>(id);
        }

        private Result<VehicleData> BuscarVehiculo(string vehicleId)
        {
            var vehicle = _insuredVehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null) return new Err<VehicleData>("Vehículo no encontrado.");
            return new Ok<VehicleData>(vehicle);
        }
    }
}