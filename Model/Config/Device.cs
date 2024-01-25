using Godot;
using System.Text.Json.Serialization;

namespace ProgrammaticallyBuilding.Model.Config
{
    [JsonDerivedType(typeof(Server), typeDiscriminator: "Server")]
    public class Device
    {
        /// <summary>
        /// The type of device this object represents
        /// </summary>
        public string DeviceType { get; set; } = string.Empty;

        /// <summary>
        /// Height of the device in rack units
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Rack units from the bottom of the rack where the device is mounted
        public int RackUnit { get; set; }

        /// <summary>
        /// Width of the device in meters
        /// </summary>
        public float DeviceWidth { get; set; }

        /// <summary>
        /// Height of the device in meters
        /// </summary>
        public float DeviceHeight { get; set; }

        /// <summary>
        /// Depth of the device in meters
        /// </summary>
        public float DeviceDepth { get; set; }

        /// <summary>
        /// Godot Node of rack
        /// </summary>
        public Node DeviceNode { get; set; }
    }
}
