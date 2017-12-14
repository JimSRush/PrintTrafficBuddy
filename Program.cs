// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Program
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using System;
using System.Windows.Forms;

namespace PrintTrafficBuddy
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainForm());
    }
  }
}
