using Godot;
using ProgrammaticallyBuilding.Model.Config;
using System;
using System.IO;
using Color = Godot.Color;

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

        private Node3D Create3DRoom(Node3D parent)
        {
            Vector3 scale = new Vector3();
            Vector3 position = new Vector3();

            scale.X = DataCenterView.RoomPosition.X;
            scale.Y = DataCenterView.RoomPosition.Z;
            scale.Z = 1;

            Node3D floor = CreateWall(new Vector3(-90, 0, 0), scale, position, parent);

            scale.X = DataCenterView.RoomPosition.Z;
            scale.Y = DataCenterView.RoomPosition.Y;
            scale.Z = 1;

            position.X = DataCenterView.RoomPosition.X/2;
            position.Y = DataCenterView.RoomPosition.Y/2;

            Node3D rightWall = CreateWall(new Vector3(0, -90, 0), scale, position, parent);

            scale.X = DataCenterView.RoomPosition.Z;
            scale.Y = DataCenterView.RoomPosition.Y;
            scale.Z = 1;

            position.X = -DataCenterView.RoomPosition.X / 2;

            Node3D leftWall = CreateWall(new Vector3(0, 90, 0), scale, position, parent);

            scale.X = DataCenterView.RoomPosition.X;
            scale.Y = DataCenterView.RoomPosition.Y;
            scale.Z = 1;

            position.X = 0;
            position.Z = -DataCenterView.RoomPosition.Z / 2;

            Node3D backWall = CreateWall(new Vector3(0, 0, 0), scale, position, parent);

            scale.X = DataCenterView.RoomPosition.X;
            scale.Y = DataCenterView.RoomPosition.Y;
            scale.Z = 1;

            position.Z = DataCenterView.RoomPosition.Z / 2;

            Node3D frontWall = CreateWall(new Vector3(-180, 0, 0), scale, position, parent);

            scale.X = DataCenterView.RoomPosition.X;
            scale.Y = DataCenterView.RoomPosition.Z;
            scale.Z = 1;

            position.X = 0;
            position.Y = DataCenterView.RoomPosition.Y;
            position.Z = 0;

            Node3D roof = CreateWall(new Vector3(90, 0, 0), scale, position, parent);


            /*Node3D roomNode = new CsgBox3D();
            parent.AddChild(roomNode);

            roomNode.Set("name", "DataCenterRoom");
            roomNode.Set("flip_faces", true);
            roomNode.Set("scale", DataCenterView.RoomPosition);
            roomNode.Set("position", Vector3.Zero);*/

            return backWall;
        }
        private Node3D CreateWall(Vector3 rotation, Vector3 scale, Vector3 position, Node parent)
        {
            Node3D roomWall = new MeshInstance3D();

            roomWall.Set("mesh", new QuadMesh());
            roomWall.RotationDegrees = rotation;
            roomWall.Scale = scale;
            roomWall.Position = position;
            
            parent.AddChild(roomWall);

            return roomWall;
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

        private Node CreateNode(string path, Vector3 size, bool collision, int type)
        {
            PackedScene nodeScene = GD.Load<PackedScene>(path);
            Node node = nodeScene.Instantiate();

            SetColor(node, new Color(0.07f, 0.11f, 0.11f, 1));

            node.GetChild(0).Set("scale", size);

            if (collision)
            {
                switch (type)
                {
                    case 1://DeviceNode
                        StaticDeviceBody staticDeviceBody = new(size);
                        node.AddChild(staticDeviceBody);
                        break;
                    case 2://DeviceLidNode
                        StaticLidBody staticLidBody = new(size);
                        node.AddChild(staticLidBody);
                        break;
                    case 3://DriveNode
                        StaticDriveBody staticDriveBody = new(size);
                        node.AddChild(staticDriveBody);
                        break;
                    case 4: //MemoryNode
                        StaticMemoryBody staticMemoryBody = new(size);
                        node.AddChild(staticMemoryBody);
                        break;
                }                
            }
            return node;
        }

        private void SetColor(Node node, Color color)
        {
            Material material = new StandardMaterial3D();

            material.Set("albedo_color", color);
            node.GetChild(0).Set("material_override", material);
        }

        private static void AddText(Node node, String text)
        {
            node.GetChild(1).GetChild(0).Set("text", text);
        }

        //Creating Racks and Devices in the racks
        private Node AddRack(Rack rack, int rackIndex)
        {
            Vector3 size;
            Vector3 position;
            Node rackNode;
            
            float rackWidth = 0.5f;
            float rackHeight = 1f;

            rack.Height = rackHeight;
            rack.Width = rackWidth;
            size = new(rackWidth, rackHeight, rack.Depth);
            rackNode = CreateNode("res://Blender/rack.blend", size, false, 0);

            position.X = rack.Position.X;
            position.Y = rackHeight / 2;//0.5f - (DataCenterView.Room.RoomSize.Height / 2);
            position.Z = rack.Position.Z;

            rackNode.Set("position", position);
            rackNode.Name = "Rack" + rackIndex;//godot node name

            rack.RackNode = rackNode;
            return rackNode;
        }

        //Adding devices to the racks
        private Node AddDevice(Rack rack, Device device, float space, int deviceIndex)
        {
            Vector3 size;
            Vector3 position;
            Node deviceNode;            

            float deviceWidth = rack.Width - 0.04f;
            float deviceHeight = 0.1f;

            device.DeviceWidth = deviceWidth;
            device.DeviceHeight = deviceHeight;
            size = new(deviceWidth, deviceHeight, device.DeviceDepth);

            //Creating Godot Node and adding it as child to Rack
            deviceNode = CreateNode("res://Blender/deviceBody.blend", size, true, 1);
            device.DeviceNode = deviceNode;

            //Adding Attributes to node
            position = new(0, space, 0);
            deviceNode.Set("position", position);
            deviceNode.Name = rack.RackNode.Name + "Device" + deviceIndex; //godot node name

            deviceNode.AddChild(AddLid(deviceNode, size, deviceHeight));

            AddText(deviceNode, "Device Type: " + device.DeviceType);

            return deviceNode;
        }

        private Node AddLid(Node parent, Vector3 size, float parentHeight)
        {
            Vector3 position;
            Node lid;            

            //Creating lid
            size.Y = 0.002f;
            lid = CreateNode("res://Blender/rackUnit.blend", size, true, 2);

            //Adding Attributes to Lid
            position = new(0, (parentHeight / 2) + 0.002f, 0);
            lid.Set("position", position);
            lid.Name = parent.Name + "Lid";

            return lid;
        }

        private Node AddDrive(Server server, Drive drive, int driveIndex)
        {
            Vector3 size;
            Vector3 position;
            Color color = new Color(0.04f, 0.03f, 0.33f, 1);
            Node driveNode;

            float driveDivider = 0.01f;
            float driveWidth = ((server.DeviceWidth / server.Drives.Count) / 2) - driveDivider;
            float driveHeight = server.DeviceHeight / 2 - driveDivider;
            float driveDepth = 0.15f;

            size = new(driveWidth, driveHeight, driveDepth); //Physical size of drive

            //Creating Godot Node and adding it as child to Device
            driveNode = CreateNode("res://Blender/rackUnit.blend", size, true, 3);
            drive.DriveNode = driveNode;

            //Adding Color
            SetColor(driveNode, color);

            //Setting position
            int horizontalIndex = ((driveIndex + driveIndex % 2) / 2) - 1;
            float horizontalMovement = ((driveDivider + driveWidth) * horizontalIndex);

            position.X = -(server.DeviceWidth / 2) + (driveWidth / 2) + driveDivider + horizontalMovement;
            position.Y = server.DeviceHeight / 2 - driveHeight / 2;
            position.Z = server.DeviceDepth / 2 - driveDepth / 2;

            //Checking if odd or even to determine placement
            if (driveIndex % 2 == 0)
            {
                position.Y = -driveHeight / 2;
            }

            driveNode.Set("position", position);
            driveNode.Name = server.DeviceNode.Name + "Drive" + driveIndex;//godot node name

            AddText(driveNode, driveNode.Name);

            return driveNode;
        }

        private Node AddMemoryModule(Server server, MemoryModule memoryModule, int memoryIndex, float driveDepth)
        {
            Vector3 size;
            Vector3 position;
            Color color = new Color(0.2f, 0.03f, 0.33f, 1);
            Node memoryNode;

            float memoryDivider = 0.01f;
            float memoryWidth = server.DeviceWidth / 4 - 0.01f;
            float memoryHeight = 0.03f;
            float memoryDepth = 0.001f;

            size = new(memoryWidth, memoryHeight, memoryDepth); //Physcal size of memory module

            //Creating Godot Node and adding it as child to Device
            memoryNode = CreateNode("res://Blender/rackUnit.blend", size, false, 4);
            memoryModule.MemoryNode = memoryNode;

            //Adding Color
            SetColor(memoryNode, color);

            //Setting position
            int depthIndex = ((memoryIndex + memoryIndex % 2) / 2) - 1;
            float depthMovement = (memoryDivider + memoryDepth) * depthIndex;
            position.X = -(server.DeviceWidth / 2) + (memoryWidth / 2);
            position.Y = 0;
            position.Z = server.DeviceDepth / 2 - (driveDepth) - memoryDivider - depthMovement;

            //Checking if odd or even to determine next placement
            if (memoryIndex % 2 == 0)
            {
                position.X = memoryWidth / 2;
            }

            memoryNode.Set("position", position);
            memoryNode.Name = server.DeviceNode.Name + "Memory" + memoryIndex;//godot node name

            return memoryNode;
        }

        public void RealizeViewModel(Node3D parent)
        {
            Node rackNode;
            Node deviceNode;
            Node driveNode;
            Node memoryNode;

            while (parent.GetChildCount() > 0)
                parent.RemoveChild(parent.GetChild(0));

            if (DataCenterView == null)
                return;
                        
            CreateCamera(parent);
            CreateLight(parent);
            CreateEnvironment(parent);

            Create3DRoom(parent);

            int rackIndex = 1;
            foreach (Rack rack in DataCenterView.Room.Racks)
            {
                rackNode = AddRack(rack, rackIndex);
                parent.AddChild(rackNode);

                float space = 1 / ((float)rack.RackUnits + 1) - (1f / 2f);
                int deviceIndex = 1;
                foreach (Device device in rack.Devices)
                {
                    deviceNode = AddDevice(rack, device, space, deviceIndex);
                    rackNode.AddChild(deviceNode);

                    if (device is Server)
                    {
                        Server server = (Server)device;
                        int driveIndex = 1;
                        foreach (Drive drive in server.Drives)
                        {
                            driveNode = AddDrive(server, drive, driveIndex);
                            deviceNode.AddChild(driveNode);
                            driveIndex++;
                        }                            
                        float driveDepth = 0.15f;/////TODO: Find solution
                        int memoryIndex = 1;
                        foreach (MemoryModule memMod in server.MemoryModules)
                        {
                            memoryNode = AddMemoryModule(server, memMod, memoryIndex, driveDepth);
                            deviceNode.AddChild(memoryNode);
                            memoryIndex++;
                        }     
                    }
                    space += ((float)1 / ((float)rack.RackUnits + 1));
                    deviceIndex++;
                }                    
                rackIndex++;
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
