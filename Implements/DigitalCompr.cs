namespace TeraClouds.Implements;

using System.Collections;
using TeraClouds.Interfaces;
using TeraClouds.Managements;

public class DigitalCompr : IDigitalCompr
{

 private Boolean running;
 public Boolean Running
 {
  get { return this.running; }
 }
 public void SetRunning(Boolean running)
 {
  this.running = running;
 }

 private Boolean enabled;
 public Boolean Enabled
 {
  get
  {
   return this.enabled;
  }
 }


 public void SetEnabled(Boolean enabled)
 {
  this.enabled = enabled;
  if (running && !enabled && (DateTime.Now - this.StartTime).TotalSeconds < ComprManagement.T_COMP_STOP_START)//2.4.3
  {
   Stop();
  }
 }

 public String Name { get; set; }


 private Boolean alarm;

 public Boolean Alarm
 {
  get
  {
   return this.alarm;
  }
 }

 public void SetAlarm(Boolean alarm)
 {
  this.alarm = alarm;

  //2.4.5.1
  if (this.Alarm)
  {
   this.Stop();
  }
 }

 public DateTime StartTime { get; private set; } = DateTime.MinValue;

 public DateTime StopTime { get; private set; } = DateTime.MinValue;

 public DigitalCompr()
 {
  this.enabled = true;
 }


 public void Start()
 {
  if (!this.running && enabled && !alarm)//2.4.5.1 = !alarm only
  {
   StartTime = DateTime.Now;
   this.running = true;
   Console.WriteLine($"{Name} DigitalCompressor has been started");
  }
 }

 public void Stop()
 {
  if (running)
  {
   StopTime = DateTime.Now;
   this.running = false;
   Console.WriteLine($"{Name} DigitalCompressor has been shut down");
  }
 }

 //2.4.5.3
 //this method keeps the number of elements in the array at most 3. since we need to check 3 times in an hour whether
 // the compr has stopped 3 times in an hour at task 2.4.5.3

 // TODO: this method is never called since we dont know what task12 refers to.
 public void AddStoppedTime(DateTime time)
 {
  lastStoppedTimesIn3.Push(time);
  if (lastStoppedTimesIn3.Count > 3)
  {
   lastStoppedTimesIn3.Pop();
  }
 }

 private Stack lastStoppedTimesIn3 = new Stack();

 public Stack LastStoppedTimesIn3
 {
  get
  {
   return lastStoppedTimesIn3;
  }
 }


}