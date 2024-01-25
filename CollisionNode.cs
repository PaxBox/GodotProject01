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
        public CollisionNode()
        {
            //Set("name", (string)GetParent().Get("name") + "Col");
            //AddChild(AddMesh());
        }
        public override void _Ready()
        {

        }
        private static Node AddMesh()
        {
            Node meshNode = new MeshNode();
            meshNode.Set("mesh", new BoxMesh());
            return meshNode;
        }
    }
}
