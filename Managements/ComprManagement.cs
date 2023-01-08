namespace TeraClouds.Managements;

using TeraClouds.Enums;
using TeraClouds.Interfaces;


public class ComprManagement
{
 private readonly Int16 T_COMPR_MIN_RUNTIME = 10;
 private readonly Int16 T_COMPR_COOLDOWN = 10;
 private readonly Int16 T_COMPR_START = 10;
 private readonly Int16 T_COMPR_STOP = 10;
 private readonly Int16 T_COMP_START_STOP = 10;
 public static readonly Int16 T_COMP_STOP_START = 10;
 private readonly Int16 T_SHUTDOWN_BETWEEN_STOPS = 2;
 private readonly Int32 CX_PT1_HIGH_PRESSURE_START_SP = 10;
 private readonly Int32 CX_PT1_HIGH_PRESSURE_STOP_SP = 5;
 private readonly Int32 CX_PT1_HIGH_PREVENT_ALARM_START_SP = 12;
 private readonly Int32 CX_PT1_HIGH_PREVENT_ALARM_STOP_SP = 11;
 private readonly Int16 T_HIGH_PRESSURE_PREVENT = 2;
 private readonly Int32 CS1_TS2_FROST_PREVENT_START_SP = 5;
 private readonly Int32 CS1_TS2_FROST_PREVENT_STOP_SP = 20;
 private readonly Int16 T_FROST_PREVENT = 2;
 private readonly Int16 CS1_TS2_FROST_ALARM_SP = 1;

 private Int32 cs1_ts2;
 public Int32 CS1_TS2
 {
  get
  {
   return cs1_ts2;
  }
 }

 public void SetCS1_TS2(Int32 value)
 {
  cs1_ts2 = value;
 }

 private Boolean IsMachineRunning = false;

 public Boolean isMachineRunning
 {
  get
  {
   return IsMachineRunning;
  }
 }

 private ICircuit c1; //field
 public ICircuit C1
 {
  get
  {
   return c1;
  }
 }

 private ICircuit c2;
 public ICircuit C2
 {
  get
  {
   return c2;
  }
 }

 public ComprManagement(ICircuit c1, ICircuit c2)
 {
  this.c1 = c1;
  this.c2 = c2;
 }

 public Boolean StartCompr()
 {
  Int16 runningComprInC1 = 0;
  Int16 runningComprInC2 = 0;
  if (C1.CA.Running)
  {
   runningComprInC1++;
  }

  if (C1.CB.Running)
  {
   runningComprInC1++;
  }
  if (C2.CB.Running)
  {
   runningComprInC2++;
  }
  if (C2.CA.Running)
  {
   runningComprInC2++;
  }

  if (runningComprInC1 > runningComprInC2)
  {
   if (CanStartComprC2CA())
   {
    c2.CA.Start();
    return true;
   }
   else if (CanStartComprC2CB())
   {
    c2.CB.Start();
    return true;
   }
  }

  else if (runningComprInC1 < runningComprInC2)
  {
   if (CanStartComprC1CA())
   {
    c1.CA.Start();
    return true;
   }
   else if (CanStartComprC1CB())
   {
    c1.CB.Start();
    return true;
   }
  }

  else if (runningComprInC1 < 2)
  {
   if (CanStartComprC1CA())
   {
    c1.CA.Start();
    return true;
   }
   else if (CanStartComprC1CB())
   {
    c1.CB.Start();
    return true;
   }
   else if (CanStartComprC2CA())
   {
    c2.CA.Start();
    return true;
   }
   else if (CanStartComprC2CB())
   {
    c2.CB.Start();
    return true;
   }
  }
  else
  {
   Console.WriteLine("All compressors are already running in all circuits");
  }
  return false;
 }

 //2.4.1.7
 public Boolean StopCompr()
 {
  Int16 runningComprInC1 = 0;
  Int16 runningComprInC2 = 0;
  if (C1.CA.Running)
  {
   runningComprInC1++;
  }

  if (C1.CB.Running)
  {
   runningComprInC1++;
  }
  if (C2.CB.Running)
  {
   runningComprInC2++;
  }
  if (C2.CA.Running)
  {
   runningComprInC2++;
  }

  if (runningComprInC1 > runningComprInC2)
  {
   if (CanStopComprC1CA())
   {
    c1.CA.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
   else if (CanStopComprC1CB())
   {
    c1.CB.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
  }

  else if (runningComprInC1 < runningComprInC2)
  {
   if (CanStopComprC2CA())
   {
    c2.CA.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
   else if (CanStopComprC2CB())
   {
    c2.CB.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
  }

  else if (runningComprInC1 > 0)
  {
   if (CanStopComprC1CA())
   {
    c1.CA.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
   else if (CanStopComprC1CB())
   {
    c1.CB.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
   else if (CanStopComprC2CA())
   {
    c2.CA.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
   else if (CanStopComprC2CB())
   {
    c2.CB.Stop();
    _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
    return true;
   }
  }
  else
  {
   Console.WriteLine("All compressors are already stopped in all circuits");
  }
  return false;



  //   Boolean result = false;

  //   C1.CA.Stop();
  //   _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
  //   C1.CB.Stop();
  //   _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
  //   C2.CA.Stop();
  //   _ = Task.Delay(T_SHUTDOWN_BETWEEN_STOPS);
  //   C2.CB.Stop();

  //   return result;
 }

 //2.4.2 Unbalanced Circuits. Used it in the PLC, at the end of control loop
 public void CheckBalanceCircuits()
 {
  if (((!C1.CA.Running && !C1.CB.Running && C2.CA.Running && C2.CB.Running) ||
    (!C2.CA.Running && !C2.CB.Running && C1.CA.Running && C1.CB.Running)) && (
      CanStartComprC1CA() || CanStartComprC1CB() || CanStartComprC2CA() || CanStartComprC2CB()
       )
    )
  {
   Console.WriteLine("Compressors starting for balance in circuits");
   _ = StartCompr();
  }
 }

 //2.4.1.7
 public void StopMachine()
 {
  StopCompr();
  IsMachineRunning = false;
 }

 public void StartMachine()
 {
  if (!IsMachineRunning)
  {
   IsMachineRunning = true;
  }
 }

 public Boolean CanStartComprC1CA()
 {

  if (!IsMachineRunning)
   return false;

  //2.4.4.1
  if (HasHighPressureGuard(CircuitType.C1))
  {
   return false;
  }
  if (C1.CA.Enabled &&
    !C1.CA.Running &&
    !C1.CA.Alarm && //2.4.5.1
    !C1.Alarm && //2.4.4.2

     (DateTime.Now - C1.CA.StopTime).TotalSeconds > T_COMPR_COOLDOWN &&

     //2.4.1.3
     (DateTime.Now - getMaxTime(C1.CB.StartTime, C2.CA.StartTime, C2.CB.StartTime)).TotalSeconds > T_COMPR_START &&

    //2.4.1.6
    (DateTime.Now - getMaxTime(C1.CA.StopTime, C1.CB.StopTime)).TotalSeconds > T_COMP_START_STOP)
  {
   return true;
  }
  return false;
 }

 public Boolean CanStartComprC1CB()
 {

  if (!IsMachineRunning)
   return false;

  //2.4.4.1
  if (HasHighPressureGuard(CircuitType.C1))
  {
   return false;
  }
  if (C1.CB.Enabled &&
 !C1.CB.Running &&
 !C1.CB.Alarm && //2.4.5.1
 !C1.Alarm && //2.4.4.2
       (DateTime.Now - C1.CB.StopTime).TotalSeconds > T_COMPR_COOLDOWN &&

       //2.4.1.3
       (DateTime.Now - getMaxTime(C1.CA.StartTime, C2.CA.StartTime, C2.CB.StartTime)).TotalSeconds > T_COMPR_START &&

      //2.4.1.6
      (DateTime.Now - getMaxTime(C1.CA.StopTime, C1.CB.StopTime)).TotalSeconds > T_COMP_START_STOP)
  {
   return true;
  }
  return false;
 }

 public Boolean CanStartComprC2CA()
 {

  if (!IsMachineRunning)
   return false;

  //2.4.4.1
  if (HasHighPressureGuard(CircuitType.C2))
  {
   return false;
  }
  if (C2.CA.Enabled &&
        !C2.CA.Running &&
        !C2.CA.Alarm && //2.4.5.1
        !C2.Alarm && //2.4.4.2
        (DateTime.Now - C2.CA.StopTime).TotalSeconds > T_COMPR_COOLDOWN &&

        //2.4.1.3
        (DateTime.Now - getMaxTime(C1.CA.StartTime, C1.CB.StartTime, C2.CB.StartTime)).TotalSeconds > T_COMPR_START &&

       //2.4.1.6
       (DateTime.Now - getMaxTime(C2.CA.StopTime, C2.CB.StopTime)).TotalSeconds > T_COMP_START_STOP)
  {
   return true;
  }
  return false;
 }

 public Boolean CanStartComprC2CB()
 {

  if (!IsMachineRunning)
   return false;

  //2.4.4.1
  if (HasHighPressureGuard(CircuitType.C2))
  {
   return false;
  }
  if (C2.CB.Enabled &&
!C2.CB.Running &&
!C2.CB.Alarm && //2.4.5.1
!C2.Alarm && //2.4.4.2
      (DateTime.Now - C2.CB.StopTime).TotalSeconds > T_COMPR_COOLDOWN &&

      //2.4.1.3
      (DateTime.Now - getMaxTime(C1.CB.StartTime, C1.CA.StartTime, C2.CA.StartTime)).TotalSeconds > T_COMPR_START &&

      //2.4.1.6
      (DateTime.Now - getMaxTime(C2.CA.StopTime, C2.CB.StopTime)).TotalSeconds > T_COMP_START_STOP)
  {
   return true;
  }
  return false;
 }

 public Boolean CanStopComprC1CA()
 {
  if (C1.CA.Enabled &&
       C1.CA.Running &&
       (DateTime.Now - C1.CA.StartTime).TotalSeconds > T_COMPR_MIN_RUNTIME && //2.4.1.1

        //2.4.1.4
        (DateTime.Now - getMaxTime(C1.CB.StopTime, C2.CA.StopTime, C2.CB.StopTime)).TotalSeconds > T_COMPR_STOP &&

       //2.4.1.5
       (DateTime.Now - getMaxTime(C1.CA.StartTime, C1.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
  {
   return true;
  }
  return false;
 }
 public Boolean CanStopComprC1CB()
 {
  if (C1.CB.Enabled &&
      C1.CB.Running &&
      (DateTime.Now - C1.CB.StartTime).TotalSeconds > T_COMPR_MIN_RUNTIME &&

       //2.4.1.4
       (DateTime.Now - getMaxTime(C1.CA.StopTime, C2.CA.StopTime, C2.CB.StopTime)).TotalSeconds > T_COMPR_STOP &&

      //2.4.1.5
      (DateTime.Now - getMaxTime(C1.CA.StartTime, C1.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
  {
   return true;
  }
  return false;
 }
 public Boolean CanStopComprC2CA()
 {
  if (C2.CA.Enabled &&
     C2.CA.Running &&
     (DateTime.Now - C2.CA.StartTime).TotalSeconds > T_COMPR_MIN_RUNTIME &&

      //2.4.1.4
      (DateTime.Now - getMaxTime(C2.CB.StopTime, C1.CA.StopTime, C1.CB.StopTime)).TotalSeconds > T_COMPR_STOP &&

     //2.4.1.5
     (DateTime.Now - getMaxTime(C2.CA.StartTime, C2.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
  {
   return true;
  }
  return false;
 }
 public Boolean CanStopComprC2CB()
 {
  if (C2.CA.Enabled &&
     C2.CA.Running &&
     (DateTime.Now - C2.CA.StartTime).TotalSeconds > T_COMPR_MIN_RUNTIME &&

      //2.4.1.4
      (DateTime.Now - getMaxTime(C2.CB.StopTime, C1.CA.StopTime, C1.CB.StopTime)).TotalSeconds > T_COMPR_STOP &&

     //2.4.1.5
     (DateTime.Now - getMaxTime(C2.CA.StartTime, C2.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
  {
   return true;
  }
  return false;
 }
 public Boolean MachineAlarmActive() => false;

 //2.4.1.5 
 public DateTime getMaxTime(DateTime time1, DateTime time2, DateTime time3)
 {

  var maxresult = new[] { time1, time2, time3 }.Max();

  return maxresult;

 }
 public DateTime getMaxTime(DateTime time1, DateTime time2)
 {

  var maxresult = new[] { time1, time2 }.Max();

  return maxresult;

 }

 //2.4.4.1
 public Boolean HasHighPressureGuard(CircuitType circuit)
 {
  Boolean result = false;
  switch (circuit)
  {
   case CircuitType.C1:
    if (C1.Pressure > CX_PT1_HIGH_PRESSURE_STOP_SP && C1.Pressure < CX_PT1_HIGH_PRESSURE_START_SP)
    {
     result = true;
    }
    break;
   case CircuitType.C2:
    if (C2.Pressure > CX_PT1_HIGH_PRESSURE_STOP_SP && C2.Pressure < CX_PT1_HIGH_PRESSURE_START_SP)
    {
     result = true;
    }
    break;
   default:
    throw new Exception("Compressor not available with name: " + circuit);
  }
  return result;
 }

 //2.4.4.2
 public void CheckHighPressurePrevent()
 {

  //2.4.4.2
  if (C1.Pressure > CX_PT1_HIGH_PREVENT_ALARM_START_SP)
  {
   if (C1.CeilingMaxRunningComprNumberInCircuit > 0 &&
    (DateTime.Now - C1.CeilingChangeTime).TotalSeconds > T_HIGH_PRESSURE_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being reduced for C1 due to the high pressure prevent ");
    C1.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C1.CeilingMaxRunningComprNumberInCircuit - 1));
   }
   ReduceRunningComprByCeiling(C1);
  }
  else if (C1.Pressure < CX_PT1_HIGH_PREVENT_ALARM_STOP_SP)
  {
   if (C1.CeilingMaxRunningComprNumberInCircuit < 2 &&
    (DateTime.Now - C1.CeilingChangeTime).TotalSeconds > T_HIGH_PRESSURE_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being increased for C1 due to the high pressure prevent");
    C1.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C1.CeilingMaxRunningComprNumberInCircuit + 1));
   };
  }
  //2.4.4.2
  if (C2.Pressure > CX_PT1_HIGH_PREVENT_ALARM_START_SP)
  {
   if (C2.CeilingMaxRunningComprNumberInCircuit > 0 &&
    (DateTime.Now - C2.CeilingChangeTime).TotalSeconds > T_HIGH_PRESSURE_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being reduced for C2 due to the high pressure prevent");
    C2.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C2.CeilingMaxRunningComprNumberInCircuit - 1));
   }
   ReduceRunningComprByCeiling(C2);
  }
  else if (C2.Pressure < CX_PT1_HIGH_PREVENT_ALARM_STOP_SP)
  {
   if (C2.CeilingMaxRunningComprNumberInCircuit < 2 &&
    (DateTime.Now - C1.CeilingChangeTime).TotalSeconds > T_HIGH_PRESSURE_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being increased for C2 due to the high pressure prevent ");
    C2.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C2.CeilingMaxRunningComprNumberInCircuit + 1));
   };
  }
 }

 //2.4.4.3
 public void CheckFrostPrevent()
 {
  //2.4.4.3
  if (C1.Pressure > CS1_TS2_FROST_PREVENT_START_SP)
  {
   if (C1.CeilingMaxRunningComprNumberInCircuit > 0 &&
    (DateTime.Now - C1.CeilingChangeTime).TotalSeconds > T_FROST_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being reduced for C1 due to the frost prevention");
    C1.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C1.CeilingMaxRunningComprNumberInCircuit - 1));
   }
   ReduceRunningComprByCeiling(C1);
  }
  else if (C1.Pressure < CS1_TS2_FROST_PREVENT_STOP_SP)
  {
   if (C1.CeilingMaxRunningComprNumberInCircuit < 2 &&
    (DateTime.Now - C1.CeilingChangeTime).TotalSeconds > T_FROST_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being increased for C1 due to the frost prevention");
    C1.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C1.CeilingMaxRunningComprNumberInCircuit + 1));
   };
  }
  //2.4.4.2
  if (C2.Pressure > CS1_TS2_FROST_PREVENT_START_SP)
  {
   if (C2.CeilingMaxRunningComprNumberInCircuit > 0 &&
    (DateTime.Now - C2.CeilingChangeTime).TotalSeconds > T_FROST_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being reduced for C2 due to the frost prevention");
    C2.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C2.CeilingMaxRunningComprNumberInCircuit - 1));
   }
   ReduceRunningComprByCeiling(C2);
  }
  else if (C2.Pressure < CS1_TS2_FROST_PREVENT_STOP_SP)
  {
   if (C2.CeilingMaxRunningComprNumberInCircuit < 2 &&
    (DateTime.Now - C1.CeilingChangeTime).TotalSeconds > T_FROST_PREVENT)
   {
    Console.WriteLine("Ceiling max number running compr is being increased for C2 due to the frost prevention");
    C2.SetCeilingMaxRunningComprNumberInCircuit((Int16)(C2.CeilingMaxRunningComprNumberInCircuit + 1));
   };
  }
 }

 //2.4.4.2
 private void ReduceRunningComprByCeiling(ICircuit circuit)
 {

  if (circuit.CeilingMaxRunningComprNumberInCircuit == 0)
  {
   //2.4.1.5
   if ((DateTime.Now - getMaxTime(C1.CA.StartTime, C1.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
   {
    circuit.CA.Stop();
   }
   if ((DateTime.Now - getMaxTime(C1.CA.StartTime, C1.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
   {
    circuit.CB.Stop();
   }
  }

  else if (circuit.CeilingMaxRunningComprNumberInCircuit == 1)
  {
   if (circuit.CA.Running && circuit.CB.Running)
   {
    //2.4.1.5
    if ((DateTime.Now - getMaxTime(C1.CA.StartTime, C1.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
    {
     circuit.CA.Stop();
    }
    else if ((DateTime.Now - getMaxTime(C1.CA.StartTime, C1.CB.StartTime)).TotalSeconds > T_COMP_STOP_START)
    {
     circuit.CB.Stop();
    }
   }
  }
 }

 //2.4.5.3 TODO here 
 public void CheckComprHighPressureAlarm()
 {

  Int16 c1caStopCountInHour = 0;
  foreach (DateTime stopTime in C1.CA.LastStoppedTimesIn3)
  {
   if ((DateTime.Now - stopTime).TotalMinutes <= 60)
   {
    c1caStopCountInHour++;
   }
  }

  Int16 c1cbStopCountInHour = 0;
  foreach (DateTime stopTime in C1.CB.LastStoppedTimesIn3)
  {
   if ((DateTime.Now - stopTime).TotalMinutes <= 60)
   {
    c1cbStopCountInHour++;
   }
  }
  if (c1cbStopCountInHour == 3 || c1caStopCountInHour == 3)
  {
   Console.WriteLine("C1 alarm is generated due to high pressure alarm");
   C1.SetAlarm(true);
  }

  Int16 c2caStopCountInHour = 0;
  foreach (DateTime stopTime in C2.CA.LastStoppedTimesIn3)
  {
   if ((DateTime.Now - stopTime).TotalMinutes <= 60)
   {
    c2caStopCountInHour++;
   }
  }

  Int16 c2cbStopCountInHour = 0;
  foreach (DateTime stopTime in C2.CB.LastStoppedTimesIn3)
  {
   if ((DateTime.Now - stopTime).TotalMinutes <= 60)
   {
    c2cbStopCountInHour++;
   }
  }
  if (c2cbStopCountInHour == 3 || c2caStopCountInHour == 3)
  {
   Console.WriteLine("C2 alarm is generated due to high pressure alarm");
   C2.SetAlarm(true);
  }
 }

 //2.4.5.4
 public void CheckFrostAlarm()
 {
  if (CS1_TS2 < CS1_TS2_FROST_ALARM_SP)
  {
   Console.WriteLine("C1 & C2 alarm is generated due to Frost alarm");
   C1.SetAlarm(true);
   C2.SetAlarm(true);
  }
 }


}