namespace ProgrammaticallyBuilding.Model.Config
{
    public class V3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }

    public static class V3Helper
    {
        public static V3 ToV3(this Godot.Vector3 v)
        {
            return new V3 { X = v.X, Y = v.Y, Z = v.Z };
        }

        public static Godot.Vector3 ToVector3(this V3 v)
        {
            return new Godot.Vector3(v.X, v.Y, v.Z);
        }
    }
}
