namespace TeraClouds;

using Microsoft.Extensions.Configuration;
using ProjectClimate.Managements;
using TeraClouds.Enums;
using TeraClouds.Models;
using TeraClouds.Utils;

internal class Program
{
 public static void Main(String[] args)
 {
  //2.2 Traits sample implementation
  _ = PressureConverter.converIntoKiloPascal(1000, PressureUnits.pa);

  //2.3 Serialisation example
  var config = ConfigurationManagement.BuildConfig();
  var person = config.GetSection("Person").Get<Person>();

  Console.WriteLine(person);

  //2.4 HeatPump example
  PLC plc = new PLC();
  plc.Start();
  Console.ReadLine();
 }




}
/*
- Unacceptable amount of spaces for indentation (single space).
- Unnecessary amount of whitespace.
- Use of if statements for boolean assignments.
- Repetetive code that code easily be refactored into a local method.
- Lots of encapsulation violations. Classes are used PODs, but are still initialized with certain data.
- Some code do not correspond to the task descriptions.
- Code has not been tested.

Include tests
*/