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
    }  
}



