using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammaticallyBuilding.Model.Config
{
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
        /// Depth of the device in meters
        /// </summary>
        public float DeviceDepth { get; set; }
    }
}
