using Godot;
using System;
using System.IO;
using ProgrammaticallyBuilding.Model.Config;
using ProgrammaticallyBuilding.ViewModel;

namespace ProgrammaticallyBuilding
{
    public partial class Start : Node3D
    {
        Vector3 roomPos = new Vector3(0, 0, 0);

        DataCenterViewModel ViewModel { get; set; } = null;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Setting window size
            /*
            Window window = GetWindow();
            window.Size = new Vector2I(1920, 1080);*/

            base._Ready();

            ViewModel = DataCenterViewModel.Load("dataServerRoom.json");

            if (ViewModel == null)
            {
                foreach (Node child in GetChildren())
                {
                    RemoveChild(child);
                    child.QueueFree();
                }

                return;
            }

            ViewModel.RealizeViewModel(this);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }


        //Makes a Point object of the Point class in ServerManager
        private static Point MakePoint(float x, float y, float z)
        {
            Point point = new()
            {
                X = x,
                Y = y,
                Z = z
            };
            return point;
        }

        //Makes a Vector object of the Vector class in ServerManager
        private static Vector3 MakeVector(float x, float y, float z)
        {
            Vector3 vector = new()
            {
                X = x,
                Y = y,
                Z = z
            };
            return vector;
        }

        //Makes a Size object of the Size class in ServerManager
        private static Size MakeSize(float x, float y, float z)
        {
            Size size = new()
            {
                Width = x,
                Height = y,
                Depth = z
            };
            return size;
        }
    }  
}



