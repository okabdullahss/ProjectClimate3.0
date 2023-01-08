namespace TeraClouds.Utils;

using TeraClouds.Enums;

public class PressureConverter
{
	public static Double converIntoKiloPascal(Double pressure, PressureUnits unit)
	{
		var kiloPa = unit switch
		{
			PressureUnits.pa => pressure * 0.001,
			PressureUnits.kPa => pressure * 1,
			PressureUnits.mPa => pressure * 1000,
			_ => throw new Exception("Invalid pressure unit: " + unit),
		};
		Console.WriteLine("Converted pressure: " + pressure + " " + unit + " = " + kiloPa + " kPa");
		return kiloPa;
	}
}