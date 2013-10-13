using NUnit.Framework;

namespace PropertyAccess.Test
{
    [TestFixture]
    public class IndexedValuePropertyAccessTestFixture
    {
        private IndexedValue _testTarget;

        [SetUp]
        public void SetUp()
        {
            _testTarget = new IndexedValue("Foo");
        }

        
        [Test]
        public void TestGetIndexProperty()
        {
            var sut = PropertyAccessFactory.CreateValueIndexed(typeof(IndexedValue), typeof(string), "Item");
            var actual = sut.GetValue(_testTarget, "bar");
            Assert.AreEqual("Foobar", actual);
        }

        [Test]
        public void TestGetIndexPropertyGeneric()
        {
            var sut = PropertyAccessFactory.CreateValueIndexed<IndexedValue, string, object>("Item");
            var actual = sut.GetValue(_testTarget, "bar");
            Assert.AreEqual("Foobar", actual);
        }

        private struct IndexedValue
        {
            private readonly string _prefix;

            public IndexedValue(string prefix)
            {
                _prefix = prefix;
            }

            public object this[string index]
            {
                get { return _prefix + index; }
            }
        }

    }
}