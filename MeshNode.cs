using Godot;

namespace ProgrammaticallyBuilding
{
    public partial class MeshNode : MeshInstance3D
    {
        public override void _Ready()
        {
            Set("name", (string)GetParent().Get("name") + "Mesh");
        }
    }
}
