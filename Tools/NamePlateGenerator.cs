﻿// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Tools.NamePlateGenerator
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using iTextSharp.text;
using iTextSharp.text.pdf;
using PrintTrafficBuddy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrintTrafficBuddy.Tools
{
  public class NamePlateGenerator : IFilmTool
  {
    public string OutputFilePath { get; set; }

    public void Execute(IList<FilmDetails> films)
    {
      using (List<string>.Enumerator enumerator = Enumerable.ToList<string>(Enumerable.Distinct<string>(Enumerable.Select<FilmDetails, string>((IEnumerable<FilmDetails>) films, (Func<FilmDetails, string>) (f => f.Genre)))).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string genre = enumerator.Current;
          if (!string.IsNullOrEmpty(genre))
          {
            Document document = new Document(PageSize.A4);
            PdfWriter instance = PdfWriter.GetInstance(document, (Stream) File.Open(this.OutputFilePath + genre.Trim() + ".pdf", FileMode.Create, FileAccess.Write));
            document.Open();
            PdfContentByte directContent = instance.DirectContent;
            NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>) films, (Func<FilmDetails, bool>) (f => f.Genre == genre))), "A");
            document.NewPage();
            NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>) films, (Func<FilmDetails, bool>) (f => f.Genre == genre))), "B");
            document.NewPage();
            NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>) films, (Func<FilmDetails, bool>) (f => f.Genre == genre))), "C");
            document.NewPage();
            NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>) films, (Func<FilmDetails, bool>) (f => f.Genre == genre))), "D");
            document.NewPage();
			NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>)films, (Func<FilmDetails, bool>)(f => f.Genre == genre))), "E");
			document.NewPage();
			NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>)films, (Func<FilmDetails, bool>)(f => f.Genre == genre))), "F");
			document.NewPage();
			NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>)films, (Func<FilmDetails, bool>)(f => f.Genre == genre))), "G");
			document.NewPage();
			NamePlateGenerator.Emit(document, directContent, Enumerable.ToList<FilmDetails>(Enumerable.Where<FilmDetails>((IEnumerable<FilmDetails>)films, (Func<FilmDetails, bool>)(f => f.Genre == genre))), "H");
			document.NewPage();
            document.Close();
          }
        }
      }
      List<FilmDetails> films1 = new List<FilmDetails>()
      {
        new FilmDetails()
        {
          Title = "Intermission",
          RunTime = 15
        },
        new FilmDetails()
        {
          Title = "Short TBC",
          RunTime = 15
        },
        new FilmDetails()
        {
          Title = "Film maker present",
          RunTime = 30
        },
        new FilmDetails()
        {
          Title = "Film maker present",
          RunTime = 40
        },
        new FilmDetails()
        {
          Title = "Intermission",
          RunTime = 15
        },
        new FilmDetails()
        {
          Title = "Short TBC",
          RunTime = 15
        },
        new FilmDetails()
        {
          Title = "Film maker present",
          RunTime = 30
        },
        new FilmDetails()
        {
          Title = "Film maker present",
          RunTime = 40
        }
      };
      Document document1 = new Document(PageSize.A4);
      PdfWriter instance1 = PdfWriter.GetInstance(document1, (Stream) File.Open(this.OutputFilePath + "Extras.pdf", FileMode.Create, FileAccess.Write));
      document1.Open();
      PdfContentByte directContent1 = instance1.DirectContent;
      for (int index = 0; index < 5; ++index)
      {
        NamePlateGenerator.Emit(document1, directContent1, films1, string.Empty);
        document1.NewPage();
      }
      document1.Close();
    }

    private static void Emit(Document document, PdfContentByte content, List<FilmDetails> films, string runName)
    {
      int count = 0;
      while (count < films.Count)
      {
        NamePlateGenerator.LayoutPage(content, Enumerable.ToList<FilmDetails>(Enumerable.Take<FilmDetails>(Enumerable.Skip<FilmDetails>((IEnumerable<FilmDetails>) films, count), 10)), runName);
        if (count + 10 < films.Count)
          document.NewPage();
        count += 10;
      }
    }

	private static String FormatTitle(String title, int runTime)
	{
		return title.Substring(0, Math.Min(30, title.Length));
	}

	private static iTextSharp.text.Font FormatTitleFont(int runTime) 
	{ 
	
		if (runTime > 60)
		{
			return FontFactory.GetFont("Arial", 11f, Font.BOLD);
		}
		return FontFactory.GetFont("Arial", 9f, Font.BOLD);
	}

	private static String FormatLanguage(String language, String country) 
	{
		if (language == null || country == null) { 
			return "";
		}
		if ((language.Length + country.Length) < 12) 
		{ 
			return language; //We ok!
		}
		if (language.Length > 7) { 
			return language.Substring(0, 7);
		}
		return language;
	}

	private static String FormatCountry(String language, String country)
	{
		if (language == null || country == null)
		{
			return "";
		}

		if ((language.Length + country.Length) < 12)
		{
			return country; //We ok!
		}
		if (country.Length > 6)
		{
			return country.Substring(0, 6);
		}
		return country;
	}

	private static Phrase FormatLanguageAndCountry(String language, String country, int runTime) 
	{ 
	
	
	if (runTime < 70) {
		return new Phrase(string.Format("{0}/{1}", language, country), FontFactory.GetFont("Arial", 6f));
	}

	return new Phrase(string.Format("{0}/{1}", language, country), FontFactory.GetFont("Arial", 8f));
	}

    private static void LayoutPage(PdfContentByte content, List<FilmDetails> pageFilms, string runName)
    {
      double num1 = 70.0;
      double num2 = 800.0;
      double num3 = 400.0;
      for (int index = 0; index < pageFilms.Count; ++index)
      {
        FilmDetails filmDetails = pageFilms[index];
        bool flag = false;
        if (filmDetails.RunTime == 0)
        {
          flag = true;
          filmDetails.RunTime = 120;
        }
        double num4 = 0.0491666666666667 * (double) filmDetails.RunTime * 30.0;
        double num5 = num1 + (double) (index % 5 * 90);
        double num6 = num5 + 90.0;
        double num7 = index >= 5 ? num3 : num2;
        double num8 = index >= 5 ? num3 - num4 : num2 - num4;
        Rectangle rectangle = new Rectangle((float) num5, (float) num8, (float) num6, (float) num7);
        rectangle.BorderColor = Color.BLACK;
        rectangle.BorderWidthBottom = 1f;
        rectangle.BorderWidthLeft = 1f;
        rectangle.BorderWidthRight = 1f;
        rectangle.BorderWidthTop = 1f;
        if (flag)
          rectangle.BackgroundColor = Color.RED;
        content.Rectangle(rectangle);
        ColumnText columnText1 = new ColumnText(content);
        columnText1.SetSimpleColumn((float) num5, (float) num8, (float) num6, (float) num7, 15f, 1);

		columnText1.AddText(new Phrase(FormatTitle(filmDetails.Title, filmDetails.RunTime), FormatTitleFont(filmDetails.RunTime)));
        columnText1.Go();
        columnText1.AddText(new Phrase(string.Format("{0} min", (object) filmDetails.RunTime), FontFactory.GetFont("Arial", 9f)));
        columnText1.Go();
        ColumnText columnText2 = columnText1;

		String l = FormatLanguage(filmDetails.Language, filmDetails.Country);
		String c = FormatCountry(filmDetails.Language, filmDetails.Country);
		
		columnText1.AddText(FormatLanguageAndCountry(l, c, filmDetails.RunTime));
		columnText1.Go();

		//Resolution/ratio
        columnText1.AddText(new Phrase(string.Format("{0} / {1}", (object) filmDetails.Resolution, (object) filmDetails.Ratio), FontFactory.GetFont("Arial", 8f)));
        columnText1.Go();

		columnText1.AddText(new Phrase(string.Format("{0} / {1}", (object)filmDetails.AucklandScreeningNo, (object)filmDetails.WellingtonScreeningNo), FontFactory.GetFont("Arial", 8f)));
		columnText1.Go();

        columnText1.AddText(new Phrase(runName, FontFactory.GetFont("Arial", 16f)));
        columnText1.Go();
      }
    }
  }
}
