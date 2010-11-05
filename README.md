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

    var property = PropertyAccessFactory.Create(typeof(Foo), "Bar");
    var bar = property.GetValue(new Foo{Bar = "baz"});

Or strongly typed:

    var property = PropertyAccessFactory.Create<Foo, string>("Bar");
    var bar = property.GetValue(new Foo{Bar = "baz"});

Performance
-----------

Number of ticks for the given number of iterative gets:

<table>
<tr><th>Iterations</th><th>Reflection</th><th>Delegate</th></tr>
<tr><td>100</td><td>2453</td><td>355</td></tr>
<tr><td>200</td><td>4328</td><td>704</td></tr>
<tr><td>300</td><td>6428</td><td>1059</td></tr>
<tr><td>400</td><td>8239</td><td>1410</td></tr>
<tr><td>500</td><td>9994</td><td>1740</td></tr>
<tr><td>600</td><td>11751</td><td>2119</td></tr>
<tr><td>700</td><td>13538</td><td>2447</td></tr>
<tr><td>800</td><td>15342</td><td>2775</td></tr>
<tr><td>900</td><td>17101</td><td>3115</td></tr>
<table>

Number of ticks for the given number of iterative sets:

<table>
<tr><th>Iterations</th><th>Reflection</th><th>Delegate</th></tr>
<tr><td>100</td><td>3436</td><td>1637</td></tr>
<tr><td>200</td><td>5600</td><td>1997</td></tr>
<tr><td>300</td><td>7713</td><td>2359</td></tr>
<tr><td>400</td><td>9900</td><td>2777</td></tr>
<tr><td>500</td><td>11925</td><td>3138</td></tr>
<tr><td>600</td><td>13968</td><td>3497</td></tr>
<tr><td>700</td><td>16012</td><td>3859</td></tr>
<tr><td>800</td><td>18060</td><td>4223</td></tr>
<tr><td>900</td><td>20071</td><td>4606</td></tr>
</table>