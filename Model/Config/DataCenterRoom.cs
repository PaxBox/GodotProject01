using Godot;
using System.Collections.Generic;

namespace ProgrammaticallyBuilding.Model.Config
{
    public class DataCenterRoom
    {
        public List<Rack> Racks { get; set; } = new List<Rack>();

        public Size RoomSize { get; set; } = new Size();

        public Point InitialCameraPosition { get; set; } = new Point();

        public V3 InitialCameraRotation { get; set; } = new V3();
    }
}
