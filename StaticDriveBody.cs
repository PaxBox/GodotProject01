using System;
using Godot;
using ProgrammaticallyBuilding.ViewModel;

namespace ProgrammaticallyBuilding
{
    public partial class StaticDriveBody : StaticNode
    {
        Vector3 size;
        Vector3 objectPosition;
        Node parent;
        Node lable;

        public override void _Ready()
        {
            InputEvent += OnInput;
            base._Ready();
        }

        public StaticDriveBody(Vector3 size) : base(size)
        {
            this.size = size;
        }

        private void OnInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
        {
            if (@event is InputEventMouseButton && @event.IsPressed())
            {
                clicked = !clicked;

                parent = GetParent();
                lable = GetChild(0);

                lable.Set("visible", lable.Get("visible").AsBool() == false);

                objectPosition = (Vector3)parent.Get("position");

                if (clicked)
                {
                    objectPosition.Y += size.Y;
                }
                else
                {
                    objectPosition.Y -= size.Y;
                }

                parent.Set("position", objectPosition);
            }
        }
    }
}