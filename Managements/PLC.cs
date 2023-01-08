using TeraClouds.Managements;
using TeraClouds.Interfaces;
using TeraClouds.Implements;

namespace ProjectClimate.Managements
{

 public class PLC
 {
  private readonly Int16 HS1_TS2_Sp = 35;
  CancellationTokenSource? source = null;
  private ComprManagement comprManagement;


  public PLC()
  {
   IDigitalCompr c1a = new DigitalCompr() { Name = "c1a" };
   IDigitalCompr c1b = new DigitalCompr() { Name = "c1b" };
   IAnalogSensor cx_tp1 = new AnalogSensor();
   ICircuit c1 = new Circuit(c1a, c1b, cx_tp1);

   IDigitalCompr c2a = new DigitalCompr() { Name = "c2a" };
   IDigitalCompr c2b = new DigitalCompr() { Name = "c2b" };
   IAnalogSensor cx_tp2 = new AnalogSensor();
   ICircuit c2 = new Circuit(c2a, c2b, cx_tp2);
   comprManagement = new ComprManagement(c1, c2);

   comprManagement.StartMachine();
  }
  public Int32 GetHS1_TS2_Value()
  {
   Random rnd = new Random();
   return rnd.Next(30, 40);

  }

  public Int32 GetCS1_TS2_Value()
  {
   Random rnd = new Random();
   return rnd.Next(-10, 100);

  }

  public void Start()
  {
   if (source != null && !source.IsCancellationRequested)
   {
    return;
   }
   source = new CancellationTokenSource();
   var token = source.Token;

   var task = Task.Run(() => CheckLoop(token), token);//running on additional thread

  }

  public void Stop()
  {
   if (source != null)
   {
    source.Cancel();
   }

  }


  private async Task CheckLoop(CancellationToken token)
  {

   Int32 currentTemperature;
   Int32 currentCS1_TS2;

   while (!token.IsCancellationRequested)
   {
    currentTemperature = GetHS1_TS2_Value();
    Console.WriteLine("current Temperature is: " + currentTemperature);
    currentCS1_TS2 = GetCS1_TS2_Value();        //system is reading curent values from outside
    comprManagement.SetCS1_TS2(currentCS1_TS2); //system is reading curent values from outside
    Console.WriteLine("current CS1_TS2 is: " + currentCS1_TS2);


    if ((currentTemperature < HS1_TS2_Sp - 1 || currentTemperature > HS1_TS2_Sp + 1) && (
     comprManagement.CanStartComprC1CA() || comprManagement.CanStartComprC1CB() ||
     comprManagement.CanStartComprC2CA() || comprManagement.CanStartComprC2CB()
    ))
    {
     Console.WriteLine("Compressors are being started for temperature");
     comprManagement.StartCompr();
    }
    else if ((currentTemperature >= HS1_TS2_Sp - 1 && currentTemperature <= HS1_TS2_Sp + 1) && (
     comprManagement.CanStopComprC1CA() || comprManagement.CanStopComprC1CB() ||
     comprManagement.CanStopComprC2CA() || comprManagement.CanStopComprC2CB()
    ))
    {
     Console.WriteLine("Compressors are being stopped for temperature");
     comprManagement.StopCompr();
    }
    comprManagement.CheckBalanceCircuits();//2.4.2
    comprManagement.CheckHighPressurePrevent();//2.4.4.2
    comprManagement.CheckFrostPrevent();//2.4.4.3
    comprManagement.CheckComprHighPressureAlarm();//2.4.5.3
    comprManagement.CheckFrostAlarm();//2.4.5.4
    Console.WriteLine($"c1a.running:{comprManagement.C1.CA.Running} c1b.running:{comprManagement.C1.CB.Running}");
    Console.WriteLine($"c2a.running:{comprManagement.C2.CA.Running} c2b.running:{comprManagement.C2.CB.Running}");
    await Task.Delay(3000);
   }
  }




 }
}