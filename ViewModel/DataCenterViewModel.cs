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

        private void CreateCamera(Node parent)
        {
            Node cameraNode = new CameraNode(DataCenterView.Room.InitialCameraPosition.ToVector3());

            // TODO : If loading a similar model, we should keep the existing camera position.
            parent.AddChild(cameraNode);
        }

        private void CreateLight(Node parent)
        {
            Node lightNode = new DirectionalLight3D();
            lightNode.Set("position", new Vector3(2, 2, 2));
            lightNode.Set("sky_mode", 0);
            parent.AddChild(lightNode);
        }

        private void CreateEnvironment(Node parent)
        {
            Node environmentNode = new WorldEnvironment();

            Sky sky = new Sky();
            sky.SkyMaterial = new ProceduralSkyMaterial();

            Godot.Environment environment = new Godot.Environment();
            environment.BackgroundMode = Godot.Environment.BGMode.Sky;
            environment.BackgroundEnergyMultiplier = 1;
            environment.Set("sky", sky);

            environmentNode.Set("environment", environment);
            parent.AddChild(environmentNode);
        }

        private Node CreateNode(string path, Vector3 size, bool collision)
        {
            PackedScene nodeScene = GD.Load<PackedScene>(path);
            Node node = nodeScene.Instantiate();
            Material material = new StandardMaterial3D();
            material.Set("albedo_color", new Color(0.07f,0.11f,0.11f,1));
            Node child = node.GetChild(0);
            child.Set("material_override", material);
            child.Set("scale", size);

            if (collision)
            {
                StaticNode staticNode = new(size);
                node.AddChild(staticNode);
            }

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
            CreateCamera(parent);
            CreateLight(parent);
            CreateEnvironment(parent);

            //Creating Racks and Devices in the racks
            float rackWidth = 0.5f;
            Vector3 size;
            Vector3 position;

            foreach (Rack rack in DataCenterView.Room.Racks)
            {
                size = new(rackWidth, 1, rack.Depth);
                Node rackNode = CreateNode("res://Blender/rack.blend", size, false);
                parent.AddChild(rackNode);

                position = new(rack.Position.X, 
                                    0.5f - (DataCenterView.Room.RoomSize.Height / 2), 
                                    rack.Position.Z);
                rackNode.Set("position", position);

                //AddText(rackNode, "Rackunits: " + rack.RackUnits);

                //Adding devices to the racks
                float space = 1 / ((float)rack.RackUnits + 1) - (1f/2f);
                int rackChild = 0;

                foreach (Device device in rack.Devices)
                {
                    size = new(rackWidth, 0.1f, device.DeviceDepth);
                    Node deviceNode = CreateNode("res://Blender/rackunit.blend", size, true);
                    rackNode.AddChild(deviceNode);
                    rackChild++;

                    //Adding Attributes to node
                    position = new(0, space, 0);
                    deviceNode.Set("position", position);

                    AddText(deviceNode, "Device Type: " + device.DeviceType);

                    space += ((float)1 / ((float)rack.RackUnits + 1));
                    GD.Print(device.DeviceType.Equals("Server"));
                    //https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism?pivots=dotnet-8-0

                    if (device is Server)
                    {
                        Server server = (Server)device;
                        int numDrives = 4;
                        float driveSpace = ((float)rack.Depth/2)/3; 
                        int driveCount = 1;
                        foreach (Drive drive in server.Drives)
                        {
                            size = new(rackWidth/2 - 0.01f, 0.1f, device.DeviceDepth/12);
                            Node driveNode = CreateNode("res://Blender/rackunit.blend", size, false);
                            deviceNode.AddChild(driveNode);
                            if(driveCount <= 4)
                            {
                                position = new(-((rackWidth / 2) - (size.X / 2)), 0, driveSpace);
                                GD.Print("driveSpace " + driveSpace);
                                                             
                            }
                            else if(driveCount == 5)
                            {
                                driveSpace = ((float)rack.Depth / 2) / 3;
                                position = new(((rackWidth / 2) - (size.X / 2)), 0, driveSpace);
                            }else
                            {
                                position = new(((rackWidth / 2) - (size.X / 2)), 0, driveSpace);
                            }
                            driveSpace -= (2 * ((float)rack.Depth / 2)) / (numDrives + 1); //S = L / (N + 1)   
                            driveNode.Set("position", position);
                            driveCount++;
                        }
                        int numMemory = 4;
                        int memoryCount = 1;
                        float memorySpace = ((float)rack.Depth / 2) -0.01f;
                        foreach (MemoryModule memMod in server.MemoryModules)
                        {
                            size = new(rackWidth / 2 - 0.01f, 0.1f, 0.001f);
                            Node memoryNode = CreateNode("res://Blender/rackunit.blend", size, false);
                            deviceNode.AddChild(memoryNode);
                            if (memoryCount <= 2)
                            {
                                position = new(-((rackWidth / 2) - (size.X / 2)), 0, memorySpace);
                                GD.Print("driveSpace " + memorySpace);

                            }
                            else if (memoryCount == 3)
                            {
                                memorySpace = ((float)rack.Depth / 2) - 0.01f;
                                position = new(((rackWidth / 2) - (size.X / 2)), 0, memorySpace);
                            }
                            else
                            {
                                position = new(((rackWidth / 2) - (size.X / 2)), 0, memorySpace);
                            }
                            memorySpace -= ((float)rack.Depth / 2) / (numMemory + 1); //S = L / (N + 1)   
                            memoryNode.Set("position", position);
                            memoryCount++;
                        }
                    }
                }
            }
        }

        private void SetWindowSize(Node parent, int x, int y)
        {
            // Setting window size
            Window window = parent.GetWindow();
            window.Size = new Vector2I(x, y);
        }
    }
}
