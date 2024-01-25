using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammaticallyBuilding
{
    public partial class StaticNode : StaticBody3D
    {
        Vector3 size;
        int childNum;
        public float transparency = 0.7f;
        public bool clicked = false;
        public StaticNode(Vector3 size)
        {
            this.size = size;
            AddChild(CreateLable());
            AddChild(AddCollision(size));
        }

        public override void _Ready()
        {
            base._Ready();
        }

        private static Node AddCollision(Vector3 size)
        {
            Node collisionNode = new CollisionNode();
            BoxShape3D boxShape = new();
            collisionNode.Set("shape", boxShape);
            boxShape.Size = size;

            return collisionNode;
        }

        private static Node CreateLable()
        {
            Node lable = new Label3D();
            lable.Set("scale", new Vector3(0.3f, 0.3f, 0.3f));
            lable.Set("position", new Vector3(0.4f, 0f, 0f));
            lable.Set("visible", false);
            lable.Set("horizontal_alignment", "Right");

            return lable;
        }

        public bool IsClicked()
        {
            return clicked;
        }

        public Node staticParent { get; set; }
    }
}
