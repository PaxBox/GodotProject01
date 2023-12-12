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
        int childNum;
        float transparency = 0.7f;
        Vector3 size;
        public StaticNode(Vector3 size)
        {
            this.size = size;
        }

        public override void _Ready()
        {
            InputEvent += OnInput;
            AddChild(CreateLable());
            AddChild(AddCollision(size));

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

        private void OnInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
        {
            if (@event is InputEventMouseButton && @event.IsPressed())
            {
                Node lable = GetChild(0);
                lable.Set("visible", lable.Get("visible").AsBool() == false);

                Node meshNode = GetParent().GetChild(0);

                if ((float)meshNode.Get("transparency") != 0)
                {
                    transparency = 0;
                }

                meshNode.Set("transparency", transparency);
                transparency = 0.7f;
            }            
        }
    }
}
