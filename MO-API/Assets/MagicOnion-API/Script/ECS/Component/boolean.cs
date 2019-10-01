namespace Script.ECS.Component
{
    // ReSharper disable once InconsistentNaming
    public struct boolean
    {
        public byte Value;
        public boolean(bool value) => Value = (byte)(value ? 1 : 0);
        public static implicit operator bool(boolean value) => value.Value == 1;
        public static implicit operator boolean(bool value) => new boolean(value);
        public override string ToString() => Value == 1 ? "true" : "false";
    }
}