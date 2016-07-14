// Decompiled with JetBrains decompiler
// Type: DLL_test1.Encrypt_.devolverEncryp
// Assembly: DLL_test1, Version=1.0.2725.14761, Culture=neutral, PublicKeyToken=null
// MVID: 81733F6D-ECB1-4873-9A91-D345ABACEC25
// Assembly location: C:\Users\asoftware8\Documents\fuentes\T3\sprint_1\bin\Debug\DLL_test1.dll

using System;
using System.Security.Cryptography;
using System.Text;

namespace DLL_test1.Encrypt_
{
  public class devolverEncryp
  {
    public string hasmio(string rr)
    {
      return BitConverter.ToString(((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(devolverEncryp.ConvertStringToByteArray(rr))).Replace("-", "");
    }

    public static byte[] ConvertStringToByteArray(string s)
    {
      return new UnicodeEncoding().GetBytes(s);
    }
  }
}
