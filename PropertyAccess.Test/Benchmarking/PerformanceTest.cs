using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using PropertyAccess.Test.Support;

namespace PropertyAccess.Test.Benchmarking
{
    [TestFixture]
    public class PerformanceTest
    {
        [Test]
        public void TestGetPropertyPerformance()
        {
            var delegateTarget = new TestTarget();
            var reflectionTarget = new TestTarget();

            var delegatePropertyAccess = PropertyAccessFactory.CreateForClass(typeof(TestTarget), "Value");
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


        private void Run(IClassPropertyAccess access, object target, object value, Stopwatch getStopwatch, Stopwatch setStopwatch)
        {
            setStopwatch.Start();
            access.SetValue(target, value);
            setStopwatch.Stop();

            getStopwatch.Start();
            var actual = access.GetValue(target);
            getStopwatch.Stop();

            Assert.AreEqual(value, actual);
        }
    }
}
