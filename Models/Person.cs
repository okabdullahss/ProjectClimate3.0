namespace TeraClouds.Models;

// PE: Simple PODs like Person should be immutable.
public class Person
{
	public String? Name { get; set; }
	public Int32 Age { get; set; }
	public String? Gender { get; set; }
	
	// PE: ToString method is evil and should usually not be overriden due to implicit conversions.
	public override String ToString() => (Name ?? "no name") + " " + Age + " " + (Gender ?? "no gender");
}