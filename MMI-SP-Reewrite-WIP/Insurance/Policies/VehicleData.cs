using Newtonsoft.Json;
using System.Collections.Generic;

namespace MMI_SP.Insurance.Policies
{
    public sealed class VehicleData
    {
        // ==========================================
        // BLOQUE: Datos
        // ==========================================
        [JsonProperty] public string Id { get; private set; }
        [JsonProperty] public string ModelName { get; private set; }
        [JsonProperty] public string Plate { get; private set; }
        [JsonProperty] public int PrimaryColor { get; private set; }
        [JsonProperty] public int SecondaryColor { get; private set; }
        [JsonProperty] public bool IsDestroyed { get; private set; }
        [JsonProperty] public int WindowTint { get; private set; }
        [JsonProperty] public int WheelType { get; private set; }
        [JsonProperty] public int WheelColor { get; private set; }
        [JsonProperty] public int TireSmokeColor { get; private set; }

        public VehicleData(
            string id, string modelName, string plate,
            int primaryColor, int secondaryColor, bool isDestroyed,
            int windowTint = 0,
            int wheelType = 0,
            int wheelColor = 0,
            int tireSmokeColor = 0)
        {
            Id = id;
            ModelName = modelName;
            Plate = plate;
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            IsDestroyed = isDestroyed;
            WindowTint = windowTint;
            WheelType = wheelType;
            WheelColor = wheelColor;
            TireSmokeColor = tireSmokeColor;
        }
    }
}