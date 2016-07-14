// Decompiled with JetBrains decompiler
// Type: DLL_test1._Mtd_FormatearMoneda
// Assembly: DLL_test1, Version=1.0.2725.14761, Culture=neutral, PublicKeyToken=null
// MVID: 81733F6D-ECB1-4873-9A91-D345ABACEC25
// Assembly location: C:\Users\asoftware8\Documents\fuentes\T3\sprint_1\bin\Debug\DLL_test1.dll

namespace DLL_test1
{
  public class _Mtd_FormatearMoneda
  {
    private string _g_str_numero;

    public _Mtd_FormatearMoneda(int _p_int_numero)
    {
      this._g_str_numero = _p_int_numero.ToString("C");
    }

    public _Mtd_FormatearMoneda(double _p_dbl_numero)
    {
      this._g_str_numero = _p_dbl_numero.ToString("C");
    }

    public string _Mtd_Formateado()
    {
      return this._g_str_numero;
    }
  }
}
