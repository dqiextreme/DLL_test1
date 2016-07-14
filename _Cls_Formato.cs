// Decompiled with JetBrains decompiler
// Type: DLL_test1._Cls_Formato
// Assembly: DLL_test1, Version=1.0.2725.14761, Culture=neutral, PublicKeyToken=null
// MVID: 81733F6D-ECB1-4873-9A91-D345ABACEC25
// Assembly location: C:\Users\asoftware8\Documents\fuentes\T3\sprint_1\bin\Debug\DLL_test1.dll

using System;
using System.Globalization;
using System.Threading;

namespace DLL_test1
{
  public class _Cls_Formato
  {
    public _Cls_Formato(string _P_Str_CulturaName)
    {
      this._Mtd_CambiarCultura(_P_Str_CulturaName);
    }

    public void _Mtd_CambiarCultura(string _P_Str_CulturaName)
    {
      Thread.CurrentThread.CurrentUICulture = new CultureInfo(_P_Str_CulturaName);
    }

    public string _Mtd_fecha(DateTime _p_dat_fecha)
    {
      string str = "";
      try
      {
        str = _p_dat_fecha.ToString(Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern);
      }
      catch (Exception ex)
      {
        _Cls_ManejoExcepcion._Mtd_ManejoErrores(ex);
      }
      return str;
    }

    public string _Mtd_fechaSQL(DateTime _p_dat_fecha, _Cls_Formato.tipoFecha _tip_fecha)
    {
      string str = "";
      try
      {
        if (_tip_fecha.Equals((object) _Cls_Formato.tipoFecha.English))
          str = _p_dat_fecha.ToString("MM-dd-yyyy");
        if (_tip_fecha.Equals((object) _Cls_Formato.tipoFecha.Spanish))
          str = _p_dat_fecha.ToString("dd-MM-yyyy");
      }
      catch (Exception ex)
      {
        _Cls_ManejoExcepcion._Mtd_ManejoErrores(ex);
      }
      return str;
    }

    public enum tipoFecha
    {
      English,
      Spanish,
    }
  }
}
