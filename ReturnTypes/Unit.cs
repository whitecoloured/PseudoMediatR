namespace PseudoMediatR.ReturnTypes
{
    /// <summary>
    /// Represents void return type.
    /// </summary>
    public struct Unit
    {
        private static int _value = 0;
        /// <summary>
        /// A value of Unit.
        /// </summary>
        public static Unit Value { get; } = new Unit();

        public static implicit operator int(Unit unit)
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
