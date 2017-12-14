// Decompiled with JetBrains decompiler
// Type: PrintTrafficBuddy.TypeExtensions
// Assembly: PrintTrafficBuddy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7F4AD34A-B9C5-4A78-A31C-29C9FA9D2DDE
// Assembly location: C:\Users\Jimbo\Desktop\FF\Print Traffic Buddy\PrintTrafficBuddy.exe

using System;

namespace PrintTrafficBuddy
{
  public static class TypeExtensions
  {
    public static int AsSafeInt32(this object value)
    {
      try
      {
        return int.Parse(value.ToString());
      }
      catch (Exception ex)
      {
        return 0;
      }
    }

    public static DateTime AsSafeDateTime(this object value)
    {
      try
      {
        return DateTime.Parse(value.ToString());
      }
      catch (Exception ex)
      {
        return DateTime.MinValue;
      }
    }

    public static DateTime? AsSafeNullableDateTime(this object value)
    {
      try
      {
        return new DateTime?(DateTime.Parse(value.ToString()));
      }
      catch (Exception ex)
      {
        return new DateTime?();
      }
    }
  }
}
