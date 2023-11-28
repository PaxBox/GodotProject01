using Godot;
using System;

public partial class TestMeshInstance3D : MeshInstance3D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    int pressed;
    public void _on_area_3d_input_event(InputEvent @event)
    {
        GD.Print("pressed");
        if (@event is InputEventMouseButton && @event.IsPressed())
        {
            //Cheking if the mesh object has been clicked 5 times and deleting it
            if (pressed >= 4)
            {
                QueueFree();
                GetTree().Quit();
            }

            //Creating random floats to set a random color when meshnode is pressed
            Random random = new Random();
            float randomFLoat1 = (float)random.NextDouble();
            float randomFLoat2 = (float)random.NextDouble();
            float randomFLoat3 = (float)random.NextDouble();
            float randomFLoat4 = (float)random.NextDouble();
            GD.Print("Pressed");

            //Creating a new material and assigning a color to meshnode
            StandardMaterial3D material = new StandardMaterial3D();
            material.AlbedoColor = new Color(randomFLoat1, randomFLoat2, randomFLoat3, randomFLoat4);
            Set("material_override", material);
            pressed++;
        }
    }
}
