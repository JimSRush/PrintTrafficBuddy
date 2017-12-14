// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.PrintTrafficReport
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using PrintTrafficBuddy.Model;
using PrintTrafficBuddy.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PrintTrafficBuddy
{
  public class PrintTrafficReport : Form
  {
    private Mutex _lock = new Mutex();
    private IContainer components = (IContainer) null;
    private string _infoDatabaseLocation;
    private IList<FilmDetails> _films;
    private IList<ScheduleInfo> _schedule;
    private DateTimePicker fromDate;
    private DateTimePicker toDate;
    private ComboBox typeComboBox;
    private ComboBox filmsComboBox;
    private Label label1;
    private Label label2;
    private Button generateButton;
    private Label statusLabel;

    public PrintTrafficReport(string infoDatabaseLocation)
    {
      this.InitializeComponent();
      this._infoDatabaseLocation = infoDatabaseLocation;
      new Thread((ThreadStart) (() => this.ExtractData())).Start();
    }

    private void ExtractData()
    {
      this._lock.WaitOne();
      this.Invoke((Delegate) new SetStatusLabelDelegate(this.SetStatusLabel), (object) "Loading films..");
      Application.DoEvents();
      this._films = new ExcelExtractor()
      {
        SpreadsheetLocation = this._infoDatabaseLocation
      }.ExtractFilms();
      this._films = (IList<FilmDetails>) Enumerable.ToList<FilmDetails>((IEnumerable<FilmDetails>) Enumerable.OrderBy<FilmDetails, string>((IEnumerable<FilmDetails>) this._films, (Func<FilmDetails, string>) (f => f.Title)));
      this.Invoke((Delegate) new SetStatusLabelDelegate(this.SetStatusLabel), (object) "Loading schedule..");
      Application.DoEvents();
      this._schedule = new ScheduleExtractor()
      {
        SpreadsheetLocation = this._infoDatabaseLocation
      }.ExtractSchedule(this._films);
      this.Invoke((Delegate) new SetStatusLabelDelegate(this.SetStatusLabel), (object) string.Empty);
      this.Invoke((Delegate) new BindFilmsDelegate(this.BindFilms));
      this._lock.ReleaseMutex();
    }

    private void BindFilms()
    {
      this.filmsComboBox.DataSource = (object) this._films;
      this.filmsComboBox.Enabled = true;
      this.filmsComboBox.SelectedIndex = 0;
      this.generateButton.Enabled = true;
    }

    private void SetStatusLabel(string text)
    {
      this.statusLabel.Text = text;
    }

    private void generateButton_Click(object sender, EventArgs e)
    {
      this._lock.WaitOne();
      this._lock.ReleaseMutex();
      if (this._films == null)
        this.ExtractData();
      string str = Environment.CurrentDirectory + "\\printTrafficReport.txt";
      using (StreamWriter writer = new StreamWriter((Stream) File.Open(str, FileMode.Create, FileAccess.Write)))
      {
        switch (this.typeComboBox.SelectedItem.ToString())
        {
          case "By Date":
            this.GenerateDateReport(writer, DateTime.Parse(this.fromDate.Text), DateTime.Parse(this.toDate.Text));
            break;
          case "By Film":
            this.GenerateFilmReport(writer, (IList<FilmDetails>) new List<FilmDetails>()
            {
              this.filmsComboBox.SelectedItem as FilmDetails
            });
            break;
          default:
            this.GenerateFilmReport(writer, this._films);
            break;
        }
        writer.Close();
      }
      Process.Start(new ProcessStartInfo("C:\\Windows\\System32\\Notepad.exe", str));
    }

    private void GenerateFilmReport(StreamWriter writer, IList<FilmDetails> films)
    {
      foreach (FilmDetails filmDetails in (IEnumerable<FilmDetails>) films)
      {
        ScheduleInfo scheduleInfo1 = (ScheduleInfo) null;
        writer.WriteLine("Schedule for: {0}", (object) filmDetails.Title);
        writer.WriteLine(string.Empty);
        foreach (ScheduleInfo scheduleInfo2 in filmDetails.ScheduledTimes)
        {
          if (!(scheduleInfo2.ScheduledTime == new DateTime(2009, 1, 1)) && !(scheduleInfo2.ScheduledTime == new DateTime(2009, 12, 31)))
          {
            if (scheduleInfo1 == null)
            {
              DateTime? @in = scheduleInfo2.Film.In;
              if (@in.HasValue)
              {
                StreamWriter streamWriter = writer;
                string format = "{0} - Arriving in {2} from {1}";
                @in = scheduleInfo2.Film.In;
                string str = @in.Value.ToString("ddd dd/MM/yyyy");
                string originatingRegion = scheduleInfo2.Film.OriginatingRegion;
                string city = scheduleInfo2.City;
                streamWriter.WriteLine(format, (object) str, (object) originatingRegion, (object) city);
              }
              else
                writer.WriteLine("{0} - Must have arrived in {1} by this date", (object) scheduleInfo2.ScheduledTime.AddDays(-1.0).Date.ToString("ddd dd/MM/yyyy"), (object) scheduleInfo2.City);
              writer.WriteLine("{0} - First screening in {1}", (object) scheduleInfo2.ScheduledTime.ToString("ddd dd/MM/yyyy HH:mm"), (object) scheduleInfo2.City);
            }
            else if (scheduleInfo1.City != scheduleInfo2.City)
            {
              writer.WriteLine("{0} - Last screening in {1}", (object) scheduleInfo1.ScheduledTime.ToString("ddd dd/MM/yyyy HH:mm"), (object) scheduleInfo1.City);
              StreamWriter streamWriter1 = writer;
              string format1 = "{0} - Transit from {1} to {2}";
              DateTime dateTime = scheduleInfo1.ScheduledTime.AddDays(1.0);
              dateTime = dateTime.Date;
              string str1 = dateTime.ToString("ddd dd/MM/yyyy");
              string city1 = scheduleInfo1.City;
              string city2 = scheduleInfo2.City;
              streamWriter1.WriteLine(format1, (object) str1, (object) city1, (object) city2);
              StreamWriter streamWriter2 = writer;
              string format2 = "{0} - First screening in {1}";
              dateTime = scheduleInfo2.ScheduledTime;
              string str2 = dateTime.ToString("ddd dd/MM/yyyy HH:mm");
              string city3 = scheduleInfo2.City;
              streamWriter2.WriteLine(format2, (object) str2, (object) city3);
            }
            if (scheduleInfo2.Film.ScheduledTimes.IndexOf(scheduleInfo2) == scheduleInfo2.Film.ScheduledTimes.Count - 2)
              writer.WriteLine("{0} - Last screening in {1}", (object) scheduleInfo2.ScheduledTime.ToString("ddd dd/MM/yyyy HH:mm"), (object) scheduleInfo2.City);
            scheduleInfo1 = scheduleInfo2;
          }
        }
        writer.WriteLine(string.Empty);
        writer.WriteLine(string.Empty);
      }
    }

    private void GenerateDateReport(StreamWriter writer, DateTime from, DateTime to)
    {
      for (int index = 0; index < to.Subtract(from).Days; ++index)
      {
        StreamWriter streamWriter = writer;
        string format = "Print Traffic for {0}";
        DateTime dateTime = from.AddDays((double) index);
        dateTime = dateTime.Date;
        string str = dateTime.ToString("ddd dd/MM/yyyy");
        streamWriter.WriteLine(format, (object) str);
        writer.WriteLine(string.Empty);
        this.EmitInternationalArrivals(writer, this._films, from.AddDays((double) index).Date);
        this.EmitLocalMovements(writer, this._schedule, from.AddDays((double) index).Date);
        writer.WriteLine(string.Empty);
        this.EmitDomesticMovements(writer, this._schedule, from.AddDays((double) index).Date);
        writer.WriteLine(string.Empty);
        writer.WriteLine(string.Empty);
      }
    }

    private void EmitLocalMovements(StreamWriter writer, IList<ScheduleInfo> _schedule, DateTime day)
    {
      IList<ScheduleInfo> movements1 = (IList<ScheduleInfo>) Enumerable.ToList<ScheduleInfo>(Enumerable.Where<ScheduleInfo>((IEnumerable<ScheduleInfo>) _schedule, (Func<ScheduleInfo, bool>) (s =>
      {
        DateTime dateTime = s.ScheduledTime;
        DateTime date1 = dateTime.Date;
        dateTime = day.AddDays(-1.0);
        DateTime date2 = dateTime.Date;
        return date1 == date2 && s.Film.ScheduledTimes.IndexOf(s) < s.Film.ScheduledTimes.Count - 2 && s.Film.ScheduledTimes[s.Film.ScheduledTimes.IndexOf(s) + 1].City == s.City && s.Film.ScheduledTimes[s.Film.ScheduledTimes.IndexOf(s) + 1].Venue != s.Venue;
      })));
      IList<ScheduleInfo> movements2 = (IList<ScheduleInfo>) Enumerable.ToList<ScheduleInfo>(Enumerable.Where<ScheduleInfo>((IEnumerable<ScheduleInfo>) _schedule, (Func<ScheduleInfo, bool>) (s => s.ScheduledTime.Date == day.Date && (s.ScheduledTime.Hour < 13 && s.Film.ScheduledTimes.IndexOf(s) < s.Film.ScheduledTimes.Count - 2 && s.Film.ScheduledTimes[s.Film.ScheduledTimes.IndexOf(s) + 1].City == s.City) && s.Film.ScheduledTimes[s.Film.ScheduledTimes.IndexOf(s) + 1].Venue != s.Venue)));
      if (movements1.Count > 0)
      {
        writer.WriteLine("LOCAL MOVEMENTS (AM):");
        this.EmitMovements(writer, movements1);
      }
      else
        writer.WriteLine("Note: No Local AM Movements");
      writer.WriteLine(string.Empty);
      if (movements2.Count > 0)
      {
        writer.WriteLine("LOCAL MOVEMENTS (PM):");
        this.EmitMovements(writer, movements2);
      }
      else
        writer.WriteLine("Note: No Local PM Movements");
    }

    private void EmitMovements(StreamWriter writer, IList<ScheduleInfo> movements)
    {
      foreach (IGrouping<string, ScheduleInfo> grouping in Enumerable.GroupBy<ScheduleInfo, string>((IEnumerable<ScheduleInfo>) movements, (Func<ScheduleInfo, string>) (s => s.City)))
      {
        writer.WriteLine("{0}:", (object) grouping.Key);
        foreach (ScheduleInfo scheduleInfo in (IEnumerable<ScheduleInfo>) grouping)
          writer.WriteLine("{0} from {1} to {2} for {3}", (object) scheduleInfo.Film.Title, (object) scheduleInfo.Venue, (object) scheduleInfo.Film.ScheduledTimes[scheduleInfo.Film.ScheduledTimes.IndexOf(scheduleInfo) + 1].Venue, (object) scheduleInfo.Film.ScheduledTimes[scheduleInfo.Film.ScheduledTimes.IndexOf(scheduleInfo) + 1].ScheduledTime.ToString("ddd dd/MM/yyyy HH:mm"));
        writer.WriteLine(string.Empty);
      }
    }

    private void EmitDomesticMovements(StreamWriter writer, IList<ScheduleInfo> _schedule, DateTime day)
    {
      IList<ScheduleInfo> list = (IList<ScheduleInfo>) Enumerable.ToList<ScheduleInfo>(Enumerable.Where<ScheduleInfo>((IEnumerable<ScheduleInfo>) _schedule, (Func<ScheduleInfo, bool>) (s =>
      {
        DateTime dateTime = s.ScheduledTime;
        DateTime date1 = dateTime.Date;
        dateTime = day.AddDays(-1.0);
        DateTime date2 = dateTime.Date;
        return date1 == date2 && s.Film.ScheduledTimes.IndexOf(s) < s.Film.ScheduledTimes.Count - 2 && s.Film.ScheduledTimes[s.Film.ScheduledTimes.IndexOf(s) + 1].City != s.City;
      })));
      if (list.Count > 0)
      {
        writer.WriteLine("DOMESTIC MOVEMENTS:");
        foreach (IEnumerable<ScheduleInfo> enumerable in Enumerable.GroupBy<ScheduleInfo, string>((IEnumerable<ScheduleInfo>) list, (Func<ScheduleInfo, string>) (s => s.City)))
        {
          foreach (ScheduleInfo scheduleInfo in enumerable)
            writer.WriteLine("{0} from {1} to {2} for {3} at {4}", (object) scheduleInfo.Film.Title, (object) scheduleInfo.City, (object) scheduleInfo.Film.ScheduledTimes[scheduleInfo.Film.ScheduledTimes.IndexOf(scheduleInfo) + 1].City, (object) scheduleInfo.Film.ScheduledTimes[scheduleInfo.Film.ScheduledTimes.IndexOf(scheduleInfo) + 1].ScheduledTime.ToString("ddd dd/MM/yyyy HH:mm"), (object) scheduleInfo.Film.ScheduledTimes[scheduleInfo.Film.ScheduledTimes.IndexOf(scheduleInfo) + 1].Venue);
        }
      }
      else
        writer.WriteLine("Note: No Domestic Movements");
      writer.WriteLine(string.Empty);
    }

    private void EmitInternationalArrivals(StreamWriter writer, IList<FilmDetails> _films, DateTime day)
    {
      IList<FilmDetails> list = (IList<FilmDetails>) Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>) _films, (Func<FilmDetails, bool>) (f => f.In.HasValue && f.In.Value.Date == day.Date || !f.In.HasValue && f.ScheduledTimes.Count > 1 && f.ScheduledTimes[1].ScheduledTime.Date == day.Date.AddDays(-1.0))));
      if (list.Count > 0)
      {
        writer.WriteLine("ARRIVALS:");
        foreach (FilmDetails filmDetails in (IEnumerable<FilmDetails>) list)
        {
          if (filmDetails.ScheduledTimes.Count > 2)
          {
            if (filmDetails.In.HasValue)
              writer.WriteLine("{0} - Arriving from {1} for {2}", (object) filmDetails.Title, (object) filmDetails.OriginatingRegion, (object) filmDetails.ScheduledTimes[1].ScheduledTime.ToString("ddd dd/MM/yyyy HH:mm"));
            else
              writer.WriteLine("{0} - Must have arrived in {1} by this date for {2}", (object) filmDetails.Title, (object) filmDetails.ScheduledTimes[1].City, (object) filmDetails.ScheduledTimes[1].ScheduledTime.ToString("ddd dd/MM/yyyy HH:mm"));
          }
        }
      }
      else
        writer.WriteLine("Note: No arrivals due today");
      writer.WriteLine(string.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.fromDate = new DateTimePicker();
      this.toDate = new DateTimePicker();
      this.typeComboBox = new ComboBox();
      this.filmsComboBox = new ComboBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.generateButton = new Button();
      this.statusLabel = new Label();
      this.SuspendLayout();
      this.fromDate.Location = new Point(66, 65);
      this.fromDate.Name = "fromDate";
      this.fromDate.Size = new Size(200, 20);
      this.fromDate.TabIndex = 0;
      this.toDate.Location = new Point(343, 65);
      this.toDate.Name = "toDate";
      this.toDate.Size = new Size(200, 20);
      this.toDate.TabIndex = 1;
      this.typeComboBox.FormattingEnabled = true;
      this.typeComboBox.Items.AddRange(new object[3]
      {
        (object) "By Date",
        (object) "By Film",
        (object) "By All Films"
      });
      this.typeComboBox.Location = new Point(30, 25);
      this.typeComboBox.Name = "typeComboBox";
      this.typeComboBox.Size = new Size(121, 21);
      this.typeComboBox.TabIndex = 2;
      this.typeComboBox.Text = "By Date";
      this.filmsComboBox.DisplayMember = "Title";
      this.filmsComboBox.Enabled = false;
      this.filmsComboBox.FormattingEnabled = true;
      this.filmsComboBox.Location = new Point(175, 25);
      this.filmsComboBox.Name = "filmsComboBox";
      this.filmsComboBox.Size = new Size(368, 21);
      this.filmsComboBox.TabIndex = 3;
      this.filmsComboBox.Text = "Please wait for data to load..";
      this.filmsComboBox.ValueMember = "Title";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(27, 71);
      this.label1.Name = "label1";
      this.label1.Size = new Size(33, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "From:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(314, 71);
      this.label2.Name = "label2";
      this.label2.Size = new Size(23, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "To:";
      this.generateButton.Enabled = false;
      this.generateButton.Location = new Point(452, 105);
      this.generateButton.Name = "generateButton";
      this.generateButton.Size = new Size(91, 32);
      this.generateButton.TabIndex = 6;
      this.generateButton.Text = "Generate";
      this.generateButton.UseVisualStyleBackColor = true;
      this.generateButton.Click += new EventHandler(this.generateButton_Click);
      this.statusLabel.AutoSize = true;
      this.statusLabel.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.statusLabel.Location = new Point(27, 115);
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new Size(0, 18);
      this.statusLabel.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(577, 163);
      this.Controls.Add((Control) this.statusLabel);
      this.Controls.Add((Control) this.generateButton);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.filmsComboBox);
      this.Controls.Add((Control) this.typeComboBox);
      this.Controls.Add((Control) this.toDate);
      this.Controls.Add((Control) this.fromDate);
      this.Name = "PrintTrafficReport";
      this.Text = "Generate Print Traffic Reports";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
