// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Tools.ScheduleExtractor
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using Microsoft.Office.Interop.Excel;
using PrintTrafficBuddy.Configuration;
using PrintTrafficBuddy.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PrintTrafficBuddy.Tools
{
  public class ScheduleExtractor : IScheduleExtractor
  {
    public string SpreadsheetLocation { get; set; }

    public string Password { get; set; }

    public string TitleCell { get; set; }

    public string DateCell { get; set; }

    public string TimeCell { get; set; }

    public string VenueCell { get; set; }

    public string CityCell { get; set; }

    public ScheduleExtractor()
    {
      this.Password = ConfigurationHelper.GetSetting("ExcelPassword", "elaine");
      this.TitleCell = ConfigurationHelper.GetSetting("ScheduleTitleColumn", "D");
      this.DateCell = ConfigurationHelper.GetSetting("ScheduleDateColumn", "G");
      this.TimeCell = ConfigurationHelper.GetSetting("ScheduleTimeColumn", "H");
      this.VenueCell = ConfigurationHelper.GetSetting("ExcelRatioColumn", "J");
      this.CityCell = ConfigurationHelper.GetSetting("ExcelRunTimeColumn", "K");
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    public IList<ScheduleInfo> ExtractSchedule(IList<FilmDetails> films)
    {
      Application application = (Application) new ApplicationClass();
      application.Workbooks.Open(this.SpreadsheetLocation, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) this.Password, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value);
      Worksheet worksheet = application.ActiveWorkbook.Worksheets[(object) 2] as Worksheet;
      List<ScheduleInfo> list = new List<ScheduleInfo>();
      for (int index = 3; index < worksheet.Rows.Count; ++index)
      {
        Range cell = worksheet.get_Range((object) (this.TitleCell + (object) index), (object) Missing.Value);
        if (!string.IsNullOrEmpty(cell.Text.ToString()))
        {
          FilmDetails filmDetails = Enumerable.FirstOrDefault<FilmDetails>((IEnumerable<FilmDetails>) films, (Func<FilmDetails, bool>) (f => f.Title.ToLowerInvariant() == cell.Text.ToString().ToLowerInvariant()));
          if (filmDetails != null)
          {
            ScheduleInfo scheduleInfo = new ScheduleInfo()
            {
              Film = filmDetails,
              ScheduledTime = DateTime.Parse((worksheet.get_Range((object) (this.DateCell + (object) index), (object) Missing.Value).Text.ToString() + " " + worksheet.get_Range((object) (this.TimeCell + (object) index), (object) Missing.Value).Text.ToString()).Trim()),
              Venue = worksheet.get_Range((object) (this.VenueCell + (object) index), (object) Missing.Value).Text.ToString(),
              City = worksheet.get_Range((object) (this.CityCell + (object) index), (object) Missing.Value).Text.ToString()
            };
            list.Add(scheduleInfo);
            filmDetails.ScheduledTimes.Add(scheduleInfo);
          }
        }
        else
          break;
      }
      IntPtr hwnd = new IntPtr(application.Parent.Hwnd);
      application.ActiveWorkbook.Close((object) false, (object) this.SpreadsheetLocation, (object) Missing.Value);
      application.Quit();
      this.CheckAndForceKill(hwnd);
      return (IList<ScheduleInfo>) list;
    }

    private void CheckAndForceKill(IntPtr hwnd)
    {
      uint lpdwProcessId;
      int num = (int) ScheduleExtractor.GetWindowThreadProcessId(hwnd, out lpdwProcessId);
      if (lpdwProcessId <= 0U)
        return;
      Process.GetProcessById((int) lpdwProcessId).Kill();
    }
  }
}
