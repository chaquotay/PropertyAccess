using NUnit.Framework;

namespace PropertyAccess.Test
{
    [TestFixture]
    public class ValuePropertyAccessTestFixture
    {
        public struct TestStruct
        {
            private int _value;

            public TestStruct(int value)
            {
                _value = value;
            }

            public int Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        private TestStruct _testTarget;

        [SetUp]
        public void SetUp()
        {
            _testTarget = new TestStruct(42);
        }

        [Test]
        public void TestGetProperty()
        {
            var sut = PropertyAccessFactory.CreateForValue(typeof(TestStruct), "Value");
            var actual = sut.GetValue(_testTarget);
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void TestGetPropertyGeneric()
        {
            var sut = PropertyAccessFactory.CreateForValue<TestStruct, int>("Value");
            var actual = sut.GetValue(_testTarget);
            Assert.AreEqual(42, actual);
        }

    }
}