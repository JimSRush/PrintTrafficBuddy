// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Model.ScheduleInfo
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using System;

namespace PrintTrafficBuddy.Model
{
  public class ScheduleInfo
  {
    public FilmDetails Film { get; set; }

    public DateTime ScheduledTime { get; set; }

    public string Venue { get; set; }

    public string City { get; set; }
  }
}
