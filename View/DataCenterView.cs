namespace ProgrammaticallyBuilding.View
{
    internal class DataCenterView
    {
        public Model.Config.DataCenterRoom Room { get; set; } = null;

        public Godot.Vector3 RoomPosition => new Godot.Vector3(Room.RoomSize.Width, Room.RoomSize.Height, Room.RoomSize.Depth);
    }
}
