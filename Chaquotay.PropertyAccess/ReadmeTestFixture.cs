using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Chaquotay.PropertyAccess
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
            var property = PropertyAccessFactory.Create(typeof (Foo), "Bar");
            var bar = property.GetValue(new Foo {Bar = "baz"});

            Assert.AreEqual("baz", bar);
        }

        [Test]
        public void StronglyTypedGet()
        {
            var property = PropertyAccessFactory.Create<Foo, string>("Bar");
            var bar = property.GetValue(new Foo { Bar = "baz" });

            Assert.AreEqual("baz", bar);
        }

    }
}
