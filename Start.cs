using Godot;
using System;
using System.IO;
using ProgrammaticallyBuilding.Model;
using Newtonsoft.Json;

namespace ProgrammaticallyBuilding
{
    public partial class Start : Node3D
    {
        Vector3 roomPos = new Vector3(0, 0, 0);

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Setting window size
            /*
            Window window = GetWindow();
            window.Size = new Vector2I(1920, 1080);*/

            base._Ready();

            //Reading Json file and Creating room
            DataCenterRoom room = JsonConvert.DeserializeObject<DataCenterRoom>(File.ReadAllText("dataServerRoom.json"));
            Create3DRoom(room, roomPos);

            //Creating and adding Camera
            Node cameraNode = new CameraNode(room.InitialCameraPosition.X, 
                                             room.InitialCameraPosition.Y, 
                                             room.InitialCameraPosition.Z);
            AddChild(cameraNode);

            //Creating Racks and Devices in the racks
            float rackWidth = 0.5f;
            Vector3 size;
            Vector3 position;

            foreach (Rack rack in room.Racks)
            {
                size = new(rackWidth, rack.RackUnits, rack.Depth);
                Node rackNode = CreateNode("res://Blender/rack.blend", size);
                AddChild(rackNode);

                //Adding Attributes to node
                position = new(rack.Position.X, 0.5f - (room.RoomSize.Height / 2), rack.Position.Z);
                //rackNode.GetChild(1).Set("scale", size);
                rackNode.Set("position", position);

                AddText(rackNode, "Rackunits: " + rack.RackUnits);

                //Adding devices to the racks
                float space = ((float)1 / ((float)rack.RackUnits + 1) -(1f/2f));
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

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        private void Create3DRoom(DataCenterRoom room, Vector3 roomPos)
        {
            Node roomNode = new CsgBox3D();
            AddChild(roomNode);

            roomNode.Set("name", "DataCenterRoom");
            roomNode.Set("flip_faces", true);
            roomNode.Set("scale", new Vector3(room.RoomSize.Width, room.RoomSize.Height, room.RoomSize.Depth));
            roomNode.Set("position", roomPos);
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



