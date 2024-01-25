using Godot;

namespace ProgrammaticallyBuilding
{
    public partial class StaticDeviceBody : StaticNode
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

        public StaticDeviceBody(Vector3 size) : base(size)
        {
            this.size = size;
        }

        private void OnInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
        {
            if (@event is InputEventMouseButton && @event.IsPressed())
            {
                parent = GetParent();
                lable = GetChild(0);

                clicked = !clicked;
                
                lable.Set("visible", lable.Get("visible").AsBool() == false);

                objectPosition = (Vector3)parent.Get("position");

                if (clicked)
                {
                    objectPosition.Z += size.Z;
                }
                else
                {
                    objectPosition.Z -= size.Z;
                }

                parent.Set("position", objectPosition);
            }
        }
    }
}

