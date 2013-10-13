using System;
using NUnit.Framework;
using PropertyAccess.Test.Support;

namespace PropertyAccess.Test
{
    [TestFixture]
    public class ClassPropertyAccessTestFixture
    {
        private TestTarget _testTarget;

        [SetUp]
        public void SetUp()
        {
            _testTarget = new TestTarget { Value = 42 };
        }

        [Test]
        public void TestGetProperty()
        {
            var sut = PropertyAccessFactory.CreateForClass(typeof(TestTarget), "Value");
            var actual = sut.GetValue(_testTarget);
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void TestGetPropertyGeneric()
        {
            var sut = PropertyAccessFactory.CreateForClass<TestTarget, int>("Value");
            var actual = sut.GetValue(_testTarget);
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void TestSetProperty()
        {
            var sut = PropertyAccessFactory.CreateForClass(typeof(TestTarget), "Value");
            sut.SetValue(_testTarget, 47);
            Assert.AreEqual(47, _testTarget.Value);
        }

        [Test]
        public void TestSetPropertyGeneric()
        {
            var sut = PropertyAccessFactory.CreateForClass<TestTarget, int>("Value");
            sut.SetValue(_testTarget, 47);
            Assert.AreEqual(47, _testTarget.Value);
        }

    }
}