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
            var directTarget = new TestTarget();

            var delegatePropertyAccess = PropertyAccessFactory.CreateForClass(typeof(TestTarget), "Value");
            var reflectionPropertyAccess = new ReflectionPropertyAccess(typeof(TestTarget), "Value");
            var directPropertyAccess = new DirectPropertyAccess();

            var delegateGetStopwatch = new Stopwatch();
            var delegateSetStopwatch = new Stopwatch();

            var reflectionGetStopwatch = new Stopwatch();
            var reflectionSetStopwatch = new Stopwatch();

            var directGetStopwatch = new Stopwatch();
            var directSetStopwatch = new Stopwatch();

            var getValues = new StringBuilder();
            getValues.AppendLine("| Iterations | Reflection | Delegate | Direct |");
            getValues.AppendLine("|-----------:|-----------:|---------:|-------:|");
            var setValues = new StringBuilder();
            setValues.AppendLine("| Iterations | Reflection | Delegate | Direct |");
            setValues.AppendLine("|-----------:|-----------:|---------:|-------:|");

            for (var i = 1; i <= 400000; i++)
            {
                if (i % 50000 == 0)
                {
                    getValues.AppendFormat("|{0:#,##0}|{1:#,##0}|{2:#,##0}|{3:#,##0}|\n", i, reflectionGetStopwatch.ElapsedTicks,
                                           delegateGetStopwatch.ElapsedTicks, directGetStopwatch.ElapsedTicks);
                    setValues.AppendFormat("|{0:#,##0}|{1:#,##0}|{2:#,##0}|{3:#,##0}|\n", i, reflectionSetStopwatch.ElapsedTicks,
                                           delegateSetStopwatch.ElapsedTicks, directSetStopwatch.ElapsedTicks);
                }

                Run(reflectionPropertyAccess, reflectionTarget, i, reflectionGetStopwatch, reflectionSetStopwatch);
                Run(delegatePropertyAccess, delegateTarget, i, delegateGetStopwatch, delegateSetStopwatch);
                Run(directPropertyAccess, directTarget, i, directGetStopwatch, directSetStopwatch);
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
