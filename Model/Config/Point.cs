namespace ProgrammaticallyBuilding.Model.Config
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }

    public static class  PointHelper
    {
        public static Point ToPoint(this Godot.Vector3 v)
        {
            return new Point { X = v.X, Y = v.Y, Z = v.Z };
        }

        public static Godot.Vector3 ToVector3(this Point v)
        {
            return new Godot.Vector3(v.X, v.Y, v.Z);
        }
    }
}
