using DLL_test1.Encrypt_;

namespace DLL_test1
{
  public class _Cls_HashPassword
  {
    public string _Mtd_HashPass(string _P_Str_textPass)
    {
      try
      {
        devolverEncryp devolverEncryp = new devolverEncryp();
        for (int index = 0; index < 3; ++index)
          _P_Str_textPass = devolverEncryp.hasmio(_P_Str_textPass);
      }
      catch
      {
      }
      return _P_Str_textPass;
    }
  }
}
