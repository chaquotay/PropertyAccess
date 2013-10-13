PropertyAccess
==============

Fast (compared to simple reflection), delegate-based property access in .NET, based on a [blog post](http://msmvps.com/blogs/jon_skeet/archive/2008/08/09/making-reflection-fly-and-exploring-delegates.aspx) by jskeet.

Usage
-----

Given a class Foo

    public class Foo {
	    public string Bar { get; set; }
	}

Retrieving an object's property:

    var property = PropertyAccessFactory.CreateForClass(typeof (Foo), "Bar");
    var bar = property.GetValue(new Foo {Bar = "baz"});

Or strongly typed:

    var property = PropertyAccessFactory.CreateForClass<Foo, string>("Bar");
    var bar = property.GetValue(new Foo { Bar = "baz" });

Performance
-----------

Number of ticks for the given number of iterative gets:

| Iterations | Reflection | Delegate | Direct |
|-----------:|-----------:|---------:|-------:|
|50.000|93.300|12.737|9.809|
|100.000|187.825|24.766|20.154|
|150.000|280.832|36.706|30.229|
|200.000|373.274|49.217|40.202|
|250.000|466.502|61.191|50.713|
|300.000|559.526|72.926|61.335|
|350.000|651.977|84.761|71.146|
|400.000|744.516|96.481|80.812|

Number of ticks for the given number of iterative sets:

| Iterations | Reflection | Delegate | Direct |
|-----------:|-----------:|---------:|-------:|
|50.000|125.054|17.042|8.219|
|100.000|249.435|34.015|15.924|
|150.000|371.886|50.324|23.561|
|200.000|494.656|66.284|30.941|
|250.000|617.795|82.240|38.294|
|300.000|741.123|98.500|45.612|
|350.000|862.767|114.564|53.392|
|400.000|985.296|130.476|60.916|
