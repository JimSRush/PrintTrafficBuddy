// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Model.TransitTime
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

namespace PrintTrafficBuddy.Model
{
  public class TransitTime
  {
    public string Origin { get; set; }

    public string Destination { get; set; }

    public int DaysInTransit { get; set; }

    public int ExpectedDelay { get; set; }
  }
}
