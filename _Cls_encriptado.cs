// Decompiled with JetBrains decompiler
// Type: DLL_test1._Cls_encriptado
// Assembly: DLL_test1, Version=1.0.2725.14761, Culture=neutral, PublicKeyToken=null
// MVID: 81733F6D-ECB1-4873-9A91-D345ABACEC25
// Assembly location: C:\Users\asoftware8\Documents\fuentes\T3\sprint_1\bin\Debug\DLL_test1.dll

using DLL_test1.Encrypt_;

namespace DLL_test1
{
  public class _Cls_encriptado
  {
    public static string _Mtd_encriptar(string _P_Str_Textencript)
    {
      try
      {
        for (int index = 0; index < 3; ++index)
          _P_Str_Textencript = encriptado.encriptar(_P_Str_Textencript);
      }
      catch
      {
      }
      return _P_Str_Textencript;
    }

    public static string _Mtd_desencriptar(string _P_Str_Textencript)
    {
      try
      {
        for (int index = 0; index < 3; ++index)
          _P_Str_Textencript = encriptado.desencriptado(_P_Str_Textencript);
      }
      catch
      {
      }
      return _P_Str_Textencript;
    }
  }
}
