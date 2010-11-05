using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace Chaquotay.PropertyAccess
{
    [TestFixture]
    public class PropertyAccessTestFixture
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
            var sut = PropertyAccessFactory.Create(typeof(TestTarget), "Value");
            var actual = sut.GetValue(_testTarget);
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void TestGetPropertyGeneric()
        {
            var sut = PropertyAccessFactory.Create<TestTarget, int>("Value");
            var actual = sut.GetValue(_testTarget);
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void TestSetPropert()
        {
            var sut = PropertyAccessFactory.Create(typeof(TestTarget), "Value");
            sut.SetValue(_testTarget, 47);
            Assert.AreEqual(47, _testTarget.Value);
        }

        [Test]
        public void TestSetPropertyGeneric()
        {
            var sut = PropertyAccessFactory.Create<TestTarget, int>("Value");
            sut.SetValue(_testTarget, 47);
            Assert.AreEqual(47, _testTarget.Value);
        }

        [Test]
        public void TestGetPropertyPerformance()
        {
            var delegateTarget = new TestTarget();
            var reflectionTarget = new TestTarget();

            var delegatePropertyAccess = PropertyAccessFactory.Create(typeof(TestTarget), "Value");
            var reflectionPropertyAccess = new ReflectionPropertyAccess(typeof(TestTarget), "Value");

            var delegateGetStopwatch = new Stopwatch();
            var delegateSetStopwatch = new Stopwatch();

            var reflectionGetStopwatch = new Stopwatch();
            var reflectionSetStopwatch = new Stopwatch();

            var getValues = new StringBuilder("Iterations,Reflection,Delegate\n");
            var setValues = new StringBuilder("Iterations,Reflection,Delegate\n");

            for (var i = 0; i < 1000; i++)
            {
                if (i % 100 == 0)
                {
                    getValues.AppendFormat("{0},{1},{2}\n", i, reflectionGetStopwatch.ElapsedTicks,
                                           delegateGetStopwatch.ElapsedTicks);
                    setValues.AppendFormat("{0},{1},{2}\n", i, reflectionSetStopwatch.ElapsedTicks,
                                           delegateSetStopwatch.ElapsedTicks);
                }

                Run(reflectionPropertyAccess, reflectionTarget, i, reflectionGetStopwatch, reflectionSetStopwatch);
                Run(delegatePropertyAccess, delegateTarget, i, delegateGetStopwatch, delegateSetStopwatch);
            }

            Console.WriteLine("GET:\n" + getValues.ToString());
            Console.WriteLine("SET:\n" + setValues.ToString());
        }

        private void Run(IPropertyAccess access, object target, object value, Stopwatch getStopwatch, Stopwatch setStopwatch)
        {
            setStopwatch.Start();
            access.SetValue(target, value);
            setStopwatch.Stop();

            getStopwatch.Start();
            var actual = access.GetValue(target);
            getStopwatch.Stop();

            Assert.AreEqual(value, actual);
        }

        private class TestTarget
        {
            private int _value;

            public int Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }
    }
}