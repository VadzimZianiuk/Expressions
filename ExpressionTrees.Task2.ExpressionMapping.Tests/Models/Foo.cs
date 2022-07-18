namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Foo
    {
        internal string Name { get; set; }

        internal Position Position { get; set; }

        public override bool Equals(object obj) => obj is Foo foo
                && foo?.Name == Name
                && foo?.Position?.X == Position?.X
                && foo?.Position?.Y == Position?.Y;

        public override int GetHashCode() => (Name ?? string.Empty).GetHashCode();
    }

    internal class Position
    {
        internal byte X { get; set; }

        internal byte Y { get; set; }
    }
}
