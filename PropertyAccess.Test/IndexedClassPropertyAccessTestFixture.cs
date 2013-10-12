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
    }
}