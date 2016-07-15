using System;
using System.Data;
using System.Data.SqlClient;

namespace DLL_test1
{
  public class _Cls_claseconexion
  {
    private SqlConnection _sqc_conex = new SqlConnection();
    public SqlConnection _SQL_Conector = new SqlConnection();
    private string _g_Str_stringconex;
    private SqlDataReader _Dr_Datareader;

    public string _g_Str_Stringconex
    {
      get
      {
        return this._g_Str_stringconex;
      }
      set
      {
        this._g_Str_stringconex = value;
      }
    }

    public bool _Mtd_ExistenRegistros(string _p_Str_SentenciaSQL)
    {
      bool flag = false;
      try
      {
        if (this._Mtd_RetornarDataset(_p_Str_SentenciaSQL).Tables[0].Rows.Count > 0)
          flag = true;
      }
      catch
      {
      }
      return flag;
    }

    public void _Mtd_Insertar(string _p_Str_tabla, string _p_Str_campos, string _p_Str_valores)
    {
      this._Mtd_EjecutarSentencia("insert into " + _p_Str_tabla + "(" + _p_Str_campos + ")values(" + _p_Str_valores + ")");
    }

    public void _Mtd_modificar(string _p_Str_tabla, string _p_Str_valores, string _p_Str_where)
    {
      if (_p_Str_where == "")
        _p_Str_where = "0=0";
      this._Mtd_EjecutarSentencia("update " + _p_Str_tabla + " set " + _p_Str_valores + " where " + _p_Str_where);
    }

    public DataSet _Mtd_RetornarDataset(string _p_Str_SentenciaSQL)
    {
      try
      {
        DataSet dataSet = new DataSet();
        SqlCommand sqlCommand = new SqlCommand(_p_Str_SentenciaSQL, this._Mtd_conexion());
        sqlCommand.CommandTimeout = 10800;
        new SqlDataAdapter()
        {
          SelectCommand = sqlCommand
        }.Fill(dataSet);
        return dataSet;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this._SQL_Conector.Close();
        this._SQL_Conector = (SqlConnection) null;
      }
    }

    public void _Mtd_CerrarConexion()
    {
      try
      {
        this._SQL_Conector.Close();
      }
      catch
      {
      }
    }

    public void _Mtd_EjecutarSentencia(string _p_Str_SentenciaSQL)
    {
      try
      {
        SqlCommand sqlCommand1 = new SqlCommand();
        SqlCommand sqlCommand2 = new SqlCommand(_p_Str_SentenciaSQL, this._Mtd_conexion());
        sqlCommand2.CommandTimeout = 10800;
        sqlCommand2.Connection = this._SQL_Conector;
        sqlCommand2.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this._SQL_Conector.Close();
        this._SQL_Conector = (SqlConnection) null;
      }
    }

    public SqlDataReader _Mtd_RetornarDatareader(string _p_Str_SentenciaSQL)
    {
      try
      {
        SqlCommand sqlCommand = new SqlCommand(_p_Str_SentenciaSQL, this._Mtd_conexion());
        sqlCommand.CommandTimeout = 10800;
        sqlCommand.Connection = this._SQL_Conector;
        this._Dr_Datareader = sqlCommand.ExecuteReader();
        return this._Dr_Datareader;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this._Dr_Datareader.Close();
        this._Dr_Datareader = (SqlDataReader) null;
        this._SQL_Conector.Close();
        this._SQL_Conector = (SqlConnection) null;
      }
    }

    public SqlConnection _Mtd_conexion()
    {
      try
      {
        if (this._SQL_Conector.State != ConnectionState.Open)
        {
          this._SQL_Conector.ConnectionString = this._g_Str_Stringconex;
          this._SQL_Conector.Open();
        }
        return this._SQL_Conector;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
