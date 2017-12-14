// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Tools.IFilmTool
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using PrintTrafficBuddy.Model;
using System.Collections.Generic;

namespace PrintTrafficBuddy.Tools
{
  public interface IFilmTool
  {
    void Execute(IList<FilmDetails> films);
  }
}
