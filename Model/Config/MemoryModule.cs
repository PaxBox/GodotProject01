using Godot;

namespace ProgrammaticallyBuilding.Model.Config
{
    public class MemoryModule
    {
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public float Capacity { get; set; }
        public Node MemoryNode { get; set; }
    }
}
