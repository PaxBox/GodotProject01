using System;
using Godot;
using ProgrammaticallyBuilding.ViewModel;

namespace ProgrammaticallyBuilding
{
    public partial class StaticMemoryBody : StaticNode
    {
        Vector3 size;
        Vector3 objectPosition;
        Node parent;
        Node lable;

        public StaticMemoryBody(Vector3 size) : base(size)
        {
            this.size = size;
        }

        public override void _Ready()
        {
            InputEvent += OnInput;
            base._Ready();
        }

        private void OnInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
        {
            if (@event is InputEventMouseButton && @event.IsPressed())
            {
                clicked = !clicked;

                parent = GetParent();

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