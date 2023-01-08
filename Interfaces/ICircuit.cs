namespace TeraClouds.Interfaces;


public interface ICircuit
{
 IDigitalCompr CA { get; }
 IDigitalCompr CB { get; }
 IAnalogSensor Cx_PT1 { get; }
 Boolean Alarm { get; }
 Decimal Pressure { get; }
 DateTime CeilingChangeTime { get; }
 Int16 CeilingMaxRunningComprNumberInCircuit { get; }

 void SetAlarm(Boolean alarm);

 void SetCeilingMaxRunningComprNumberInCircuit(Int16 value);

}