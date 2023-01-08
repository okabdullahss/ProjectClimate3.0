namespace TeraClouds.Implements;

using TeraClouds.Interfaces;

public class Circuit : ICircuit
{
 // PE: Encapsulation violation. Compr interface exposes functionality that should not be exposed.
 private IDigitalCompr ca;
 public IDigitalCompr CA { get { return ca; } }
 private IDigitalCompr cb;
 public IDigitalCompr CB { get { return cb; } }
 private IAnalogSensor cx_pt1;
 public IAnalogSensor Cx_PT1 { get { return cx_pt1; } }
 private Boolean alarm;
 public Boolean Alarm { get { return alarm; } }
 public void SetAlarm(Boolean alarm)
 {
  this.alarm = alarm;
  if (alarm)
  {//2.4.5.2
   this.StopCircuit();
  }
 }

 public void StopCircuit()
 {
  ca.Stop();
  cb.Stop();
 }

 public Decimal Pressure { get { return cx_pt1.Pv; } }
 private DateTime ceilingChangeTime;
 public DateTime CeilingChangeTime
 {
  get
  {
   return ceilingChangeTime;
  }
 }

 private Int16 ceilingMaxRunningComprNumberInCircuit;
 public Int16 CeilingMaxRunningComprNumberInCircuit
 {
  get
  {
   return ceilingMaxRunningComprNumberInCircuit;
  }
 }

 public void SetCeilingMaxRunningComprNumberInCircuit(Int16 value)
 {
  this.ceilingMaxRunningComprNumberInCircuit = value;
  this.ceilingChangeTime = DateTime.Now;
 }

 public Circuit(IDigitalCompr compr1, IDigitalCompr compr2, IAnalogSensor sensor1)
 {
  ca = compr1;
  cb = compr2;
  cx_pt1 = sensor1;
 }
}