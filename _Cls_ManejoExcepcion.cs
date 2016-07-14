// Decompiled with JetBrains decompiler
// Type: DLL_test1._Cls_ManejoExcepcion
// Assembly: DLL_test1, Version=1.0.2725.14761, Culture=neutral, PublicKeyToken=null
// MVID: 81733F6D-ECB1-4873-9A91-D345ABACEC25
// Assembly location: C:\Users\asoftware8\Documents\fuentes\T3\sprint_1\bin\Debug\DLL_test1.dll

using System;
using System.Windows.Forms;

namespace DLL_test1
{
  public class _Cls_ManejoExcepcion
  {
    public static void _Mtd_ManejoErrores(Exception ou)
    {
      int num = (int) MessageBox.Show(ou.Message.ToString());
    }
  }
}
