// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Tools.ShippingCalculator
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using PrintTrafficBuddy.Configuration;
using PrintTrafficBuddy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace PrintTrafficBuddy.Tools
{
  public class ShippingCalculator : IFilmTool
  {
    private List<TransitTime> _transitTable = new List<TransitTime>()
    {
      new TransitTime()
      {
        Origin = "ADL",
        Destination = "WLG",
        DaysInTransit = 1,
        ExpectedDelay = 0
      },
      new TransitTime()
      {
        Origin = "BNE",
        Destination = "WLG",
        DaysInTransit = 1,
        ExpectedDelay = 0
      },
      new TransitTime()
      {
        Origin = "MEL",
        Destination = "WLG",
        DaysInTransit = 1,
        ExpectedDelay = 0
      },
      new TransitTime()
      {
        Origin = "SYD",
        Destination = "WLG",
        DaysInTransit = 1,
        ExpectedDelay = 0
      },
      new TransitTime()
      {
        Origin = "AUSTRIA",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "CANADA",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "CZECH",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "DENMARK",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "FRANCE",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "GERMANY",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "ICELAND",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "ISRAEL",
        Destination = "WLG",
        DaysInTransit = 4,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "ITALY",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "MEXICO",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 3
      },
      new TransitTime()
      {
        Origin = "NETHERLANDS",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "KOREA",
        Destination = "WLG",
        DaysInTransit = 2,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "SPAIN",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "SWITZERLAND",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "TAIWAN",
        Destination = "WLG",
        DaysInTransit = 2,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "TURKEY",
        Destination = "WLG",
        DaysInTransit = 4,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "UK",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "NYC",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "LAX",
        Destination = "WLG",
        DaysInTransit = 3,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "USA",
        Destination = "WLG",
        DaysInTransit = 4,
        ExpectedDelay = 2
      },
      new TransitTime()
      {
        Origin = "AKL",
        Destination = "WLG",
        DaysInTransit = 1,
        ExpectedDelay = 0
      },
      new TransitTime()
      {
        Origin = "WLG",
        Destination = "WLG",
        DaysInTransit = 0,
        ExpectedDelay = 0
      }
    };

    public string OutputFilePath { get; set; }

    public void Execute(IList<FilmDetails> films)
    {
      this.TryLoadTransitTableByConfiguration();
      using (StreamWriter streamWriter = new StreamWriter(this.OutputFilePath + "transitTimes.csv"))
      {
        foreach (FilmDetails film in (IEnumerable<FilmDetails>) films)
          streamWriter.WriteLine(this.EmitTransitDetails(film));
        streamWriter.Close();
      }
    }

    private void TryLoadTransitTableByConfiguration()
    {
      string setting = ConfigurationHelper.GetSetting("TransitTableFile", Environment.CurrentDirectory + "\\TransitTable.xml");
      if (!File.Exists(setting))
        return;
      try
      {
        List<TransitTime> list = new XmlSerializer(typeof (List<TransitTime>)).Deserialize((Stream) File.Open(setting, FileMode.Open, FileAccess.Read)) as List<TransitTime>;
        if (list != null && list.Count > 0)
          this._transitTable = list;
      }
      catch (Exception ex)
      {
      }
    }

    private string EmitTransitDetails(FilmDetails film)
    {
      TransitTime transitTime1 = Enumerable.SingleOrDefault<TransitTime>((IEnumerable<TransitTime>) this._transitTable, (Func<TransitTime, bool>) (t => t.Origin == film.OriginatingRegion));
      int transitTime2 = 99;
      if (transitTime1 != null)
        transitTime2 = transitTime1.DaysInTransit + transitTime1.ExpectedDelay;
      DateTime dateTime = ShippingCalculator.CalculateTransitInWorkingDays(film.Leaving, transitTime2);
      return string.Format("\"{0}\",{1},{2}", (object) film.Title, (object) transitTime2, (object) dateTime.ToString("dd/MM/yyyy"));
    }

    public static DateTime CalculateTransitInWorkingDays(DateTime eta, int transitTime)
    {
      for (; transitTime > 0; --transitTime)
        eta = ShippingCalculator.SkipWeekends(eta).AddDays(1.0);
      return ShippingCalculator.SkipWeekends(eta);
    }

    private static DateTime SkipWeekends(DateTime eta)
    {
      while (eta.DayOfWeek == DayOfWeek.Saturday || eta.DayOfWeek == DayOfWeek.Sunday)
        eta = eta.AddDays(1.0);
      return eta;
    }
  }
}
