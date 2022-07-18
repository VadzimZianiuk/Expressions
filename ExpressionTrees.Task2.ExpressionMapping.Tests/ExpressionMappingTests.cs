using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void FooToBarRule1()
        {
            var expected = new Bar { Id = 100, PositionX = 10, PositionY = 50 };
            var source = new Foo { Name = "100", Position = new Position { X = 10, Y = 50 } };

            var rules = new RuleSet<Foo, Bar>()
                .Add(f => f.Name, b => b.Id, name => int.Parse(name))
                .Add(f => f.Position, b => b.PositionX, pos => pos.X)
                .Add(f => f.Position, b => b.PositionY, pos => pos.Y);

            var mapper = new MappingGenerator().Generate(rules);
            var actual = mapper.Map(source);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(ReferenceEquals(expected, actual));
        }

        [TestMethod]
        public void FooToBarRule2()
        {
            var expected = new Bar { Id = -100, PositionX = byte.MinValue, PositionY = byte.MaxValue };
            var source = new Foo { Name = "100", Position = new Position { X = 10, Y = 50 } };

            var rules = new RuleSet<Foo, Bar>()
                .Add(f => f.Name, b => b.Id, name => -int.Parse(name))
                .Add(f => f.Position, b => b.PositionX, pos => pos.X >= 20 ? byte.MaxValue : byte.MinValue)
                .Add(f => f.Position, b => b.PositionY, pos => pos.Y >= 20 ? byte.MaxValue : byte.MinValue);

            var mapper = new MappingGenerator().Generate(rules);
            var actual = mapper.Map(source);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(ReferenceEquals(expected, actual));
        }

        [TestMethod]
        public void BarToFooRule1()
        {
            var expected = new Foo { Name = "100", Position = new Position { X = 10, Y = 50 } };
            var source = new Bar { Id = 100, PositionX = 10, PositionY = 50 };

            var rules = new RuleSet<Bar, Foo>()
                .Add(b => b.Id, f => f.Name, name => name.ToString())
                .Add(b => b, f => f.Position, bar => new Position { X = bar.PositionX, Y = bar.PositionY });

            var mapper = new MappingGenerator().Generate(rules);
            var actual = mapper.Map(source);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(ReferenceEquals(expected, actual));
        }

        [TestMethod]
        public void BarToFooRule2()
        {
            var source = new Bar { Id = 100, PositionX = 10, PositionY = 50 };
            var expected = new Foo { Name = "-100", Position = new Position { X = byte.MinValue, Y = byte.MaxValue } };

            var rules = new RuleSet<Bar, Foo>()
                .Add(b => b.Id, f => f.Name, name => (-name).ToString())
                .Add(b => b, f => f.Position, bar => new Position 
                { 
                    X = bar.PositionX >= 20 ? byte.MaxValue : byte.MinValue , 
                    Y = bar.PositionY >= 20 ? byte.MaxValue : byte.MinValue
                });

            var mapper = new MappingGenerator().Generate(rules);
            var actual = mapper.Map(source);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(ReferenceEquals(expected, actual));
        }

        [TestMethod]
        public void BarToFooOnlyIdToName()
        {
            var source = new Bar { Id = 100, PositionX = 10, PositionY = 50 };
            var expected = new Foo { Name = "100" };

            var rules = new RuleSet<Bar, Foo>()
                .Add(b => b.Id, f => f.Name, name => name.ToString());

            var mapper = new MappingGenerator().Generate(rules);
            var actual = mapper.Map(source);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(ReferenceEquals(expected, actual));
        }

        [TestMethod]
        public void BarToFooDefault()
        {
            var source = new Bar { Id = 100, PositionX = 10, PositionY = 50 };
            var expected = new Foo();

            var rules = new RuleSet<Bar, Foo>();

            var mapper = new MappingGenerator().Generate(rules);
            var actual = mapper.Map(source);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(ReferenceEquals(expected, actual));
        }
    }
}
