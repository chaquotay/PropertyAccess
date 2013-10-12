using NUnit.Framework;

namespace PropertyAccess
{

    [TestFixture]
    public class ReadmeTestFixture
    {
        public class Foo
        {
            public string Bar { get; set; }
        }

        [Test]
        public void WeaklyTypedGet()
        {
            var property = PropertyAccessFactory.CreateRead(typeof (Foo), "Bar");
            var bar = property.GetValue(new Foo {Bar = "baz"});

            Assert.AreEqual("baz", bar);
        }

        [Test]
        public void StronglyTypedGet()
        {
            var property = PropertyAccessFactory.CreateForClass<Foo, string>("Bar");
            var bar = property.GetValue(new Foo { Bar = "baz" });

            Assert.AreEqual("baz", bar);
        }

    }
}
