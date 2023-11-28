using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
