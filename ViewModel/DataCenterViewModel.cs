using Godot;
using ProgrammaticallyBuilding.Model.Config;
using System;
using System.IO;

namespace ProgrammaticallyBuilding.ViewModel
{
    internal class DataCenterViewModel
    {
        public View.DataCenterView DataCenterView { get; set; } = null;

        public DataCenterViewModel()
        {
        }

        public static DataCenterViewModel Load(string path)
        {
            if (!File.Exists(path))
                return null;

            try
            {
                var result = new DataCenterViewModel();

                var json = File.ReadAllText(path);

                var room = System.Text.Json.JsonSerializer.Deserialize<DataCenterRoom>(json);

                GD.PrintErr(room.InitialCameraPosition);

                result = new DataCenterViewModel
                {
                    DataCenterView = new View.DataCenterView
                    {
                        Room = room
                    }
                };

                return result;
            }
            catch (Exception ex)
            {
                GD.PrintErr(ex.Message);
                return null;
            }
        }

        private void Create3DRoom(Node3D parent)
        {
            Node roomNode = new CsgBox3D();
            parent.AddChild(roomNode);

            roomNode.Set("name", "DataCenterRoom");
            roomNode.Set("flip_faces", true);
            roomNode.Set("scale", DataCenterView.RoomPosition);
            roomNode.Set("position", Vector3.Zero);
        }

        private Node CreateNode(string path, Vector3 size) //ERROR HANDLING
        {
            PackedScene nodeScene = GD.Load<PackedScene>(path);
            Node node = nodeScene.Instantiate();
            //node.Set("name", (string)GetParent().Get("name"));

            StaticNode staticNode = new(size);
            node.AddChild(staticNode);

            return node;
        }

        private static void AddText(Node node, String text)
        {
            node.GetChild(1).GetChild(0).Set("text", text);
        }

        public void RealizeViewModel(Node3D parent)
        {
            while (parent.GetChildCount() > 0)
                parent.RemoveChild(parent.GetChild(0));

            if (DataCenterView == null)
                return;

            Create3DRoom(parent);

            //Creating and adding Camera
            Node cameraNode = new CameraNode(DataCenterView.Room.InitialCameraPosition.ToVector3());

            // TODO : If loading a similar model, we should keep the existing camera position.
            parent.AddChild(cameraNode);

            //Creating Racks and Devices in the racks
            float rackWidth = 0.5f;
            Vector3 size;
            Vector3 position;

            foreach (Rack rack in DataCenterView.Room.Racks)
            {
                size = new(rackWidth, rack.RackUnits, rack.Depth);
                Node rackNode = CreateNode("res://Blender/rack.blend", size);
                parent.AddChild(rackNode);

                //Adding Attributes to node
                position = new(rack.Position.X, 0.5f - (DataCenterView.Room.RoomSize.Height / 2), rack.Position.Z);
                //rackNode.GetChild(1).Set("scale", size);
                rackNode.Set("position", position);

                AddText(rackNode, "Rackunits: " + rack.RackUnits);

                //Adding devices to the racks
                float space = ((float)1 / ((float)rack.RackUnits + 1) - (1f / 2f));
                int rackChild = 0;

                foreach (Device device in rack.Devices)
                {
                    size = new(rackWidth, 0.2f, device.DeviceDepth);
                    Node deviceNode = CreateNode("res://Blender/rackunit.blend", size);
                    rackNode.AddChild(deviceNode);
                    rackChild++;

                    //Adding Attributes to node
                    position = new(0, space, 0);
                    //deviceNode.GetChild(1).Set("scale", size);
                    deviceNode.Set("position", position);

                    AddText(deviceNode, "Device Type: " + device.DeviceType);

                    space += ((float)1 / ((float)rack.RackUnits + 1));
                }
            }

        }
    }
}
