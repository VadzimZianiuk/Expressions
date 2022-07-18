namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Bar
    {
        internal int Id { get; set; }

        internal byte PositionX { get; set; }

        internal byte PositionY { get; set; }

        public override bool Equals(object obj) => obj is Bar bar
                && bar?.Id == Id
                && bar?.PositionX == PositionX
                && bar?.PositionY == PositionY;

        public override int GetHashCode() => Id.GetHashCode();
    }
}
