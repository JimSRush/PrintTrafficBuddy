// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.Configuration.ConfigurationHelper
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using System;
using System.Configuration;

namespace PrintTrafficBuddy.Configuration
{
  public static class ConfigurationHelper
  {
    public static string GetSetting(string name, string defaultValue)
    {
      try
      {
        string str = ConfigurationSettings.AppSettings[name];
        if (string.IsNullOrEmpty(str))
          return defaultValue;
        return str;
      }
      catch (Exception ex)
      {
        return defaultValue;
      }
    }
  }
}
