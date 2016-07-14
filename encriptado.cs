using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DLL_test1.Encrypt_
{
  public class encriptado
  {
    public static string encriptar(string variable)
    {
      return Convert.ToBase64String(Encoding.UTF7.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(variable)))).Replace("=", "SGv");
    }

    public static string desencriptado(string variable2)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF7.GetString(Convert.FromBase64String(variable2.Replace("SGv", "=")))));
    }

    public static void guardar_imagen(string nombreimagen, Page pagina, int ancho_, int largo_, HtmlInputFile imagen, string path, string[] extension)
    {
      int index = 0;
      foreach (string str1 in extension)
      {
        foreach (FileSystemInfo file in new DirectoryInfo(pagina.Server.MapPath(path)).GetFiles())
        {
          string name = file.Name;
          try
          {
            if (name.IndexOf("pqthunder") == -1)
              File.Delete(pagina.Server.MapPath(path + name));
          }
          catch
          {
          }
        }
        string str2 = nombreimagen + "pqthunder" + extension[index];
        string str3 = nombreimagen + extension[index];
        int num = 0;
        for (; File.Exists(pagina.Server.MapPath(path + str3)); str3 = nombreimagen + num.ToString() + extension[index])
          ++num;
        for (; File.Exists(pagina.Server.MapPath(path + str2)); str2 = nombreimagen + "pqthunder" + num.ToString() + extension[index])
          ++num;
        if (Path.GetExtension(imagen.PostedFile.FileName).ToLower() == extension[index])
        {
          imagen.PostedFile.SaveAs(pagina.Server.MapPath(path + str3));
          new Bitmap(Image.FromFile(pagina.Server.MapPath(path + str3)), ancho_, largo_).Save(pagina.Server.MapPath(path + str2));
        }
        ++index;
      }
    }

    public static string cadena_url(string url)
    {
      try
      {
        if (url.IndexOf(".ASPX") != -1)
        {
          foreach (char ch in url)
          {
            if (ch.ToString().IndexOf("/") != -1)
              url = url.Remove(0, url.IndexOf("/") + 1);
            url = url.Insert(url.IndexOf(".ASPX", 0) + 5, "|");
            url = url.Remove(url.IndexOf("|", 0), url.Length - url.IndexOf("|", 0));
          }
        }
        else
        {
          foreach (char ch in url)
          {
            if (ch.ToString().IndexOf("/") != -1)
              url = url.Remove(0, url.IndexOf("/") + 1);
            url = url.Insert(url.IndexOf(".aspx", 0) + 5, "|");
            url = url.Remove(url.IndexOf("|", 0), url.Length - url.IndexOf("|", 0));
          }
        }
      }
      catch
      {
      }
      return url;
    }
  }
}
