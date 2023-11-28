using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammaticallyBuilding
{
    public partial class CameraNode : Camera3D
    {
        public CameraNode(float x, float y, float z)
        {
            Set("position", new Vector3(x, y, z));
        }

        public CameraNode(Vector3 initialPosition)
        {
            Set("position", initialPosition);
        }

        float acceleration = 25.0f; //Key acceleration speed
        float moveSpeed = 20.0f; //Key movment speed
        float mouseSpeed = 300.0f; //Rotation movement speed
        Vector3 velocity = Godot.Vector3.Zero;
        Vector2 lookAngles = Godot.Vector2.Zero;

        public override void _Ready()
        {
            //Set("current", true);
            //Set("top_level", true);
            //Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        float min = (float)Math.PI / -2;
        float max = (float)Math.PI / 2;
        public override void _Process(double delta)
        {
            lookAngles.Y = Math.Clamp(lookAngles.Y, min, max);
            Set("rotation", new Vector3(lookAngles.Y, lookAngles.X, 0));

            Vector3 direction = UpdateDirection();

            if (direction.LengthSquared() > 0)
            {
                velocity += direction * acceleration * (float)delta;
            }
            if (velocity.Length() > moveSpeed)
            {
                velocity = velocity.Normalized() * moveSpeed;
            }

            Translate(velocity * (float)delta);
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion mouseMotion)
            {
                lookAngles -= mouseMotion.Relative / mouseSpeed;
            }
        }

        private Vector3 UpdateDirection()
        {
            Vector3 direction = new();
            if (Input.IsActionPressed("moveForwards"))
            {
                direction = new Vector3(0, 0, -1);
            }
            if (Input.IsActionPressed("moveBackwards"))
            {
                direction = new Vector3(0, 0, 1);
            }
            if (Input.IsActionPressed("moveRight"))
            {
                direction = new Vector3(1, 0, 0);
            }
            if (Input.IsActionPressed("moveLeft"))
            {
                direction = new Vector3(-1, 0, 0);
            }
            if (Input.IsActionPressed("moveUp"))
            {
                direction = new Vector3(0, 1, 0);
            }
            if (Input.IsActionPressed("moveDown"))
            {
                direction = new Vector3(0, -1, 0);
            }
            if (direction == Vector3.Zero)
            {
                velocity = Vector3.Zero;
            }
            return direction.Normalized();
        }
    }
}
