using System.Collections.Generic;
using NUnit.Framework;

namespace PropertyAccess.Test
{
    [TestFixture]
    public class IndexedClassPropertyAccessTestFixture
    {
        private Dictionary<string, object> _testTarget;

        [SetUp]
        public void SetUp()
        {
            _testTarget = new Dictionary<string, object> { { "Foo", 42 } };
        }

        [Test]
        public void TestGetProperty()
        {
            var sut = PropertyAccessFactory.CreateClassIndexed(typeof(Dictionary<string, object>), typeof(string), "Item");
            var actual = sut.GetValue(_testTarget,"Foo");
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void TestGetPropertyGeneric()
        {
            var sut = PropertyAccessFactory.CreateClassIndexed<Dictionary<string, object>,string,object>("Item");
            var actual = sut.GetValue(_testTarget,"Foo");
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void TestSetProperty()
        {
            var sut = PropertyAccessFactory.CreateClassIndexed(typeof(Dictionary<string, object>), typeof(string), "Item");
            sut.SetValue(_testTarget, "Bar", 47);
            Assert.AreEqual(47, _testTarget["Bar"]);
        }

        [Test]
        public void TestSetPropertyGeneric()
        {
            var sut = PropertyAccessFactory.CreateClassIndexed<Dictionary<string, object>, string, object>("Item");
            sut.SetValue(_testTarget, "Bar", 47);
            Assert.AreEqual(47, _testTarget["Bar"]);
        }

        [Test]
        public void TestSetPropertyIndexerOverloaded()
        {
            var indexed = new OverloadedIndexer();
            var sut = PropertyAccessFactory.CreateClassIndexed(typeof(OverloadedIndexer), typeof(long), "Item");
            sut.SetValue(indexed, 42L, "Fortytwo");
            Assert.AreEqual("Fortytwo", indexed[42]);
        }

        [Test]
        public void TestGetPropertyIndexerOverloaded()
        {
            var indexed = new OverloadedIndexer();
            indexed[42] = "Fortytwo";
            var sut = PropertyAccessFactory.CreateClassIndexed(typeof(OverloadedIndexer), typeof(long), "Item");
            var actual = sut.GetValue(indexed, 42L);
            Assert.AreEqual("Fortytwo", actual);
        }

        [Test]
        public void TestSetPropertyGenericIndexerOverloaded()
        {
            var indexed = new OverloadedIndexer();
            var sut = PropertyAccessFactory.CreateClassIndexed<OverloadedIndexer, long, string>("Item");
            sut.SetValue(indexed, 42, "Fortytwo");
            Assert.AreEqual("Fortytwo", indexed[42]);
        }

        [Test]
        public void TestGetPropertyGenericIndexerOverloaded()
        {
            var indexed = new OverloadedIndexer();
            indexed[42] = "Fortytwo";
            var sut = PropertyAccessFactory.CreateClassIndexed<OverloadedIndexer, long, string>("Item");
            var actual = sut.GetValue(indexed, 42);
            Assert.AreEqual("Fortytwo", actual);
        }

        public class OverloadedIndexer
        {
            private readonly Dictionary<long, string> _values = new Dictionary<long, string>();

            public string this[int key]
            {
                get { return _values[key]; }
                set { _values[key] = value; }
            }

            public string this[long key]
            {
                get { return _values[key]; }
                set { _values[key] = value; }
            }
        }
    }
}