using System.Collections;

namespace TeraClouds.Interfaces;

// PE: Same issues as DigitalCompr impl
public interface IDigitalCompr
{
 Boolean Running { get; }
 Boolean Enabled { get; }
 Boolean Alarm { get; }

 DateTime StartTime { get; }
 DateTime StopTime { get; }
 Stack LastStoppedTimesIn3 { get; }

 String Name { get; set; }

 void AddStoppedTime(DateTime time);

 void Start();
 void Stop();



}