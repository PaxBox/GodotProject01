using Godot;
using System.Collections.Generic;

namespace ProgrammaticallyBuilding.Model.Config
{

    /// <summary>
    /// Represents a physical rack in the datacenter.
    /// </summary>
    public class Rack
    {
        /// <summary>
        /// Describes the physical location within the datacenter of the rack.
        /// </summary>  
        public Point Position { get; set; } = new Point();

        /// <summary>
        /// Physical width of the rack in meters.
        /// </summary>  
        public float Width { get; set; }

        /// <summary>
        /// Physical height of the rack in meters.
        /// </summary>  
        public float Height { get; set; }

        /// <summary>
        /// Physical depth of the rack in meters.
        /// </summary>  
        public float Depth { get; set; }

        /// <summary>
        /// The height of the rack in rack units
        /// </summary>
        public int RackUnits { get; set; }

        /// <summary>
        /// The devices within the rack
        /// </summary>
        public List<Device> Devices { get; set; } = new List<Device>();

        /// <summary>
        /// Godot Node of rack
        /// </summary>  
        public Node RackNode { get; set; }
    }
}
