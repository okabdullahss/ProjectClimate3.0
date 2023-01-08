namespace TeraClouds.Implements;

using System;
using TeraClouds.Interfaces;

public class AnalogSensor: IAnalogSensor
{

	private Decimal pv;
	public Decimal Pv { 
		get{
			return this.pv;
	}}

	public void SetPv(Decimal pv){
  this.pv = pv;
	}
}