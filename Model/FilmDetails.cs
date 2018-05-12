// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Model.FilmDetails
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using System;
using System.Collections.Generic;

namespace PrintTrafficBuddy.Model
{
  public class FilmDetails
  {
    public string Title { get; set; }

    public string Genre { get; set; }

    public string Gauge { get; set; }

    public string Ratio { get; set; }

    public int RunTime { get; set; }

    public string OriginatingRegion { get; set; }

    public DateTime Leaving { get; set; }

    public DateTime? In { get; set; }

    public DateTime? Out { get; set; }

	public string Language { get; set; }

	public string Country { get; set; }


    public bool InSuspect { get; set; }

    public bool OutSuspect { get; set; }

    public List<ScheduleInfo> ScheduledTimes { get; set; }

    public FilmDetails()
    {
      this.ScheduledTimes = new List<ScheduleInfo>();
    }
  }
}
