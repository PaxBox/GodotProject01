using Godot;

namespace ProgrammaticallyBuilding
{
    public partial class StaticLidBody : StaticNode
    {
        Vector3 size;
        Vector3 objectPosition;
        Node parent;

        public override void _Ready()
        {
            InputEvent += OnInput;
            base._Ready();
        }

        public StaticLidBody(Vector3 size) : base(size)
        {
            this.size = size;
        }

        private void OnInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
        {
            if (@event is InputEventMouseButton && @event.IsPressed())
            {
                parent = GetParent();

                StaticNode staticNode = null;

                if (parent.GetParent().GetChild(1) is StaticNode)
                {
                    staticNode = (StaticNode)parent.GetParent().GetChild(1);
                    //GD.Print("parent is clicked " + staticNode.clicked);
                    //GD.Print("I am clicked " + clicked);
                    //GD.Print("parent.GetParent().GetChild(1).Name: " + parent.GetParent().GetChild(1).Name);
                }
                
                if(staticNode.IsClicked()) 
                {
                    GD.Print("parent is clicked " + staticNode.IsClicked());
                    objectPosition = (Vector3)parent.Get("position");

                    if (clicked)
                    {
                        GD.Print("objectPosition: ", objectPosition);
                        objectPosition.X += size.X;
                        GD.Print("objectPosition += size.X: ", objectPosition);
                    }
                    else
                    {
                        GD.Print("objectPosition: ", objectPosition);
                        objectPosition.X -= size.X;
                        GD.Print("objectPosition -= size.X: ", objectPosition);
                    }
                    parent.Set("position", objectPosition);
                    clicked = !clicked;
                }         
            }
        }
    }
}

