using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammaticallyBuilding
{
    public partial class CollisionNode : CollisionShape3D
    {
        public override void _Ready()
        {
            Set("name", (string)GetParent().Get("name") + "Col");
            //AddChild(AddMesh());
        }
        private static Node AddMesh()
        {
            Node meshNode = new MeshNode();
            meshNode.Set("mesh", new BoxMesh());
            return meshNode;
        }
    }
}
