// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Tools.ExcelExtractor
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using Microsoft.Office.Interop.Excel;
using PrintTrafficBuddy;
using PrintTrafficBuddy.Configuration;
using PrintTrafficBuddy.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace PrintTrafficBuddy.Tools
{
  public class ExcelExtractor : IFilmExtractor
  {
    public string SpreadsheetLocation { get; set; }

    public string Password { get; set; }

    public string TitleCell { get; set; }

    public string GenreCell { get; set; }

    public string GaugeCell { get; set; }

    public string RatioCell { get; set; }

    public string RunTimeCell { get; set; }

    public string InCell { get; set; }

    public string OutCell { get; set; }

    public string LeavingCell { get; set; }

    public string OriginCell { get; set; }

    public ExcelExtractor()
    {
      this.TitleCell = ConfigurationHelper.GetSetting("ExcelTitleColumn", "C");
      this.GenreCell = ConfigurationHelper.GetSetting("ExcelGenreColumn", "DU");
      this.GaugeCell = ConfigurationHelper.GetSetting("ExcelGaugeColumn", "N");
      this.RatioCell = ConfigurationHelper.GetSetting("ExcelRatioColumn", "O");
      this.RunTimeCell = ConfigurationHelper.GetSetting("ExcelRunTimeColumn", "K");
      this.InCell = ConfigurationHelper.GetSetting("ExcelInColumn", "BB");
      this.OutCell = ConfigurationHelper.GetSetting("ExcelOutColumn", "BI");
      this.LeavingCell = ConfigurationHelper.GetSetting("ExcelLeavingColumn", "AZ");
      this.OriginCell = ConfigurationHelper.GetSetting("ExcelOriginColumn", "AT");
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    public IList<FilmDetails> ExtractFilms()
    {
      List<FilmDetails> list = new List<FilmDetails>();
      Application application = new Excel.ApplicationClass();
      application.Workbooks.Open(this.SpreadsheetLocation, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value, (object)Missing.Value);
      Worksheet worksheet = application.ActiveWorkbook.Worksheets[(object) 1] as Worksheet;
      for (int index = 2; index < worksheet.Rows.Count; ++index)
      {
        Range range = worksheet.get_Range((object) (this.TitleCell + (object) index), (object) Missing.Value);
        if (!string.IsNullOrEmpty(range.Text.ToString()))
          list.Add(new FilmDetails()
          {
            Title = range.Text.ToString(),
            Genre = worksheet.get_Range((object) (this.GenreCell + (object) index), (object) Missing.Value).Text.ToString(),
            Gauge = worksheet.get_Range((object) (this.GaugeCell + (object) index), (object) Missing.Value).Text.ToString(),
            Ratio = worksheet.get_Range((object) (this.RatioCell + (object) index), (object) Missing.Value).Text.ToString(),
            RunTime = TypeExtensions.AsSafeInt32(worksheet.get_Range((object) (this.RunTimeCell + (object) index), (object) Missing.Value).Value2),
            In = TypeExtensions.AsSafeNullableDateTime(worksheet.get_Range((object) (this.InCell + (object) index), (object) Missing.Value).Text),
            InSuspect = (bool) worksheet.get_Range((object) (this.InCell + (object) index), (object) Missing.Value).Font.Italic,
            Out = TypeExtensions.AsSafeNullableDateTime(worksheet.get_Range((object) (this.OutCell + (object) index), (object) Missing.Value).Text),
            OutSuspect = (bool) worksheet.get_Range((object) (this.OutCell + (object) index), (object) Missing.Value).Font.Italic,
            Leaving = TypeExtensions.AsSafeDateTime(worksheet.get_Range((object) (this.LeavingCell + (object) index), (object) Missing.Value).Text),
            OriginatingRegion = worksheet.get_Range((object) (this.OriginCell + (object) index), (object) Missing.Value).Text.ToString().Trim().ToUpper()
          });
        else
          break;
      }
      IntPtr hwnd = new IntPtr(application.Parent.Hwnd);
      application.ActiveWorkbook.Close((object) false, (object) this.SpreadsheetLocation, (object) Missing.Value);
      application.Quit();
      this.CheckAndForceKill(hwnd);
      return (IList<FilmDetails>) list;
    }

    private void CheckAndForceKill(IntPtr hwnd)
    {
      uint lpdwProcessId;
      int num = (int) ExcelExtractor.GetWindowThreadProcessId(hwnd, out lpdwProcessId);
      if (lpdwProcessId <= 0U)
        return;
      Process.GetProcessById((int) lpdwProcessId).Kill();
    }
  }
}
