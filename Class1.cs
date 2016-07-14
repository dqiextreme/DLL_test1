using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;

namespace DLL_test1
{
    public class Class1
    {
        //todo va en una clase
        DataSet _Ds = new DataSet();
        SqlDataReader reader1;
        Object[] SQL_fields;
        Object[] Ds_Fields;
        string SQL_print;

        public void DS_remove()
        {
            while (_Ds.Tables.Count > 0)
            {
                DataTable table = _Ds.Tables[0];
                if (_Ds.Tables.CanRemove(table))
                {
                    _Ds.Tables.Remove(table);
                }
            }
            _Ds.Tables.Add();
        }

        public DataSet trans_dataset(string sqlq, string sqlc)
        {
            //retorno dataset directamete
            //para cargarlo a un datagridview
            //dataGridView1.DataSource = trans_dataset(sqlq, sqlc).Tables[0];
            
            DS_remove();

            SqlConnection conn2 = new SqlConnection(sqlc);
            conn2.Open();
            SqlTransaction trans = conn2.BeginTransaction("test");
            SqlCommand cmd2 = new SqlCommand(sqlq, conn2, trans);

            try
            {
                reader1 = cmd2.ExecuteReader();

                //for (int i = 0; i < reader1.VisibleFieldCount; i++)
                //for (int i = 0; i < reader1.FieldCount; i++){
                //    _ds.Tables[0].Columns.Add();}

                for (int i = 0; i < reader1.FieldCount; i++)
                {
                    _Ds.Tables[0].Columns.Add(reader1.GetName(i).ToString());
                }

                SQL_fields = new Object[reader1.FieldCount];
                while (reader1.Read())
                {
                    reader1.GetSqlValues(SQL_fields);
                    _Ds.Tables[0].Rows.Add(SQL_fields);
                }
                reader1.Close(); trans.Commit(); conn2.Close();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                conn2.Close();
            }
            return _Ds;
        }

        public string sql_transaction(string sqlq, string sqlc)
        {
            SqlConnection conn2 = new SqlConnection(sqlc);
            conn2.Open();
            SqlTransaction trans = conn2.BeginTransaction("test");
            SqlCommand cmd2 = new SqlCommand(sqlq, conn2, trans);

            try
            {
                conn2.InfoMessage += (object obj, SqlInfoMessageEventArgs e) => SQL_print = e.Message;
                var res = cmd2.ExecuteNonQuery();
                trans.Commit();
                conn2.Close();
                return SQL_print;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                conn2.Close();
                return "";
            }
        }

        public string SQL_obtener_schem(string NOMBRE_TABLA)
        {
            string M_query2 =
            "DECLARE @table_name SYSNAME\n" +
            "SELECT @table_name = 'dbo." + NOMBRE_TABLA + "'\n" +

            "DECLARE \n" +
            "@object_name SYSNAME\n" +
            ", @object_id INT\n" +

            "SELECT \n" +
            "@object_name = '[' + s.name + '].[' + o.name + ']'\n" +
            ", @object_id = o.[object_id]\n" +
            "FROM sys.objects o WITH (NOWAIT)\n" +
            "JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]\n" +
            "WHERE s.name + '.' + o.name = @table_name\n" +
            "AND o.[type] = 'U'\n" +
            "AND o.is_ms_shipped = 0\n" +

            "DECLARE @SQL NVARCHAR(MAX) = ''\n" +

            ";WITH index_column AS \n" +
            "(\n" +
            "SELECT \n" +
            "ic.[object_id]\n" +
            ", ic.index_id\n" +
            ", ic.is_descending_key\n" +
            ", ic.is_included_column\n" +
            ", c.name\n" +
            "FROM sys.index_columns ic WITH (NOWAIT)\n" +
            "JOIN sys.columns c WITH (NOWAIT) ON ic.[object_id] = c.[object_id] AND ic.column_id = c.column_id\n" +
            "WHERE ic.[object_id] = @object_id\n" +
            "),\n" +
            "fk_columns AS \n" +
            "(\n" +
            "SELECT \n" +
            "k.constraint_object_id\n" +
            ", cname = c.name\n" +
            ", rcname = rc.name\n" +
            "FROM sys.foreign_key_columns k WITH (NOWAIT)\n" +
            "JOIN sys.columns rc WITH (NOWAIT) ON rc.[object_id] = k.referenced_object_id AND rc.column_id = k.referenced_column_id \n" +
            "JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id AND c.column_id = k.parent_column_id\n" +
            "WHERE k.parent_object_id = @object_id\n" +
            ")\n" +
            "SELECT @SQL = 'CREATE TABLE ' + @object_name + CHAR(13) + '(' + CHAR(13) + STUFF((\n" +
            "SELECT CHAR(9) + ', [' + c.name + '] ' + \n" +
            "CASE WHEN c.is_computed = 1\n" +
            "THEN 'AS ' + cc.[definition] \n" +
            "ELSE UPPER(tp.name) + \n" +
            "CASE WHEN tp.name IN ('varchar', 'char', 'varbinary', 'binary', 'text')\n" +
            "        THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR(5)) END + ')'\n" +
            "        WHEN tp.name IN ('nvarchar', 'nchar', 'ntext')\n" +
            "        THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length / 2 AS VARCHAR(5)) END + ')'\n" +
            "        WHEN tp.name IN ('datetime2', 'time2', 'datetimeoffset') \n" +
            "        THEN '(' + CAST(c.scale AS VARCHAR(5)) + ')'\n" +
            "        WHEN tp.name = 'decimal' \n" +
            "        THEN '(' + CAST(c.[precision] AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'\n" +
            "    ELSE ''\n" +
            "END +\n" +
            "CASE WHEN c.collation_name IS NOT NULL THEN ' COLLATE ' + c.collation_name ELSE '' END +\n" +
            "CASE WHEN c.is_nullable = 1 THEN ' NULL' ELSE ' NOT NULL' END +\n" +
            "CASE WHEN dc.[definition] IS NOT NULL THEN ' DEFAULT' + dc.[definition] ELSE '' END + \n" +
            "CASE WHEN ic.is_identity = 1 THEN ' IDENTITY(' + CAST(ISNULL(ic.seed_value, '0') AS CHAR(1)) + ',' + CAST(ISNULL(ic.increment_value, '1') AS CHAR(1)) + ')' ELSE '' END \n" +
            "END + CHAR(13)\n" +
            "FROM sys.columns c WITH (NOWAIT)\n" +
            "JOIN sys.types tp WITH (NOWAIT) ON c.user_type_id = tp.user_type_id\n" +
            "LEFT JOIN sys.computed_columns cc WITH (NOWAIT) ON c.[object_id] = cc.[object_id] AND c.column_id = cc.column_id\n" +
            "LEFT JOIN sys.default_constraints dc WITH (NOWAIT) ON c.default_object_id != 0 AND c.[object_id] = dc.parent_object_id AND c.column_id = dc.parent_column_id\n" +
            "LEFT JOIN sys.identity_columns ic WITH (NOWAIT) ON c.is_identity = 1 AND c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id\n" +
            "WHERE c.[object_id] = @object_id\n" +
            "ORDER BY c.column_id\n" +
            "FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, CHAR(9) + ' ')\n" +
            "+ ISNULL((SELECT CHAR(9) + ', CONSTRAINT [' + k.name + '] PRIMARY KEY (' + \n" +
            "    (SELECT STUFF((\n" +
            "            SELECT ', [' + c.name + '] ' + CASE WHEN ic.is_descending_key = 1 THEN 'DESC' ELSE 'ASC' END\n" +
            "            FROM sys.index_columns ic WITH (NOWAIT)\n" +
            "            JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id\n" +
            "            WHERE ic.is_included_column = 0\n" +
            "                AND ic.[object_id] = k.parent_object_id \n" +
            "                AND ic.index_id = k.unique_index_id     \n" +
            "            FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''))\n" +
            "+ ')' + CHAR(13)\n" +
            "FROM sys.key_constraints k WITH (NOWAIT)\n" +
            "WHERE k.parent_object_id = @object_id \n" +
            "AND k.[type] = 'PK'), '') + ')'  + CHAR(13)\n" +
            "+ ISNULL((SELECT (\n" +
            "SELECT CHAR(13) +\n" +
            "'ALTER TABLE ' + @object_name + ' WITH' \n" +
            "+ CASE WHEN fk.is_not_trusted = 1\n" +
            "THEN ' NOCHECK' \n" +
            "ELSE ' CHECK' \n" +
            "END + \n" +
            "' ADD CONSTRAINT [' + fk.name  + '] FOREIGN KEY(' \n" +
            "+ STUFF((\n" +
            "SELECT ', [' + k.cname + ']'\n" +
            "FROM fk_columns k\n" +
            "WHERE k.constraint_object_id = fk.[object_id]\n" +
            "FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')\n" +
            "+ ')' +\n" +
            "' REFERENCES [' + SCHEMA_NAME(ro.[schema_id]) + '].[' + ro.name + '] ('\n" +
            "+ STUFF((\n" +
            "SELECT ', [' + k.rcname + ']'\n" +
            "FROM fk_columns k\n" +
            "WHERE k.constraint_object_id = fk.[object_id]\n" +
            "FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')\n" +
            "+ ')'\n" +
            "+ CASE\n" +
            "WHEN fk.delete_referential_action = 1 THEN ' ON DELETE CASCADE' \n" +
            "WHEN fk.delete_referential_action = 2 THEN ' ON DELETE SET NULL'\n" +
            "WHEN fk.delete_referential_action = 3 THEN ' ON DELETE SET DEFAULT' \n" +
            "ELSE '' \n" +
            "END\n" +
            "+ CASE\n" +
            "WHEN fk.update_referential_action = 1 THEN ' ON UPDATE CASCADE'\n" +
            "WHEN fk.update_referential_action = 2 THEN ' ON UPDATE SET NULL'\n" +
            "WHEN fk.update_referential_action = 3 THEN ' ON UPDATE SET DEFAULT'  \n" +
            "ELSE '' \n" +
            "END \n" +
            "+ CHAR(13) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + fk.name  + ']' + CHAR(13)\n" +
            "FROM sys.foreign_keys fk WITH (NOWAIT)\n" +
            "JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = fk.referenced_object_id\n" +
            "WHERE fk.parent_object_id = @object_id\n" +
            "FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)')), '')\n" +
            "+ ISNULL(((SELECT\n" +
            "CHAR(13) + 'CREATE' + CASE WHEN i.is_unique = 1 THEN ' UNIQUE' ELSE '' END \n" +
            "+ ' NONCLUSTERED INDEX [' + i.name + '] ON ' + @object_name + ' (' +\n" +
            "STUFF((\n" +
            "SELECT ', [' + c.name + ']' + CASE WHEN c.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END\n" +
            "FROM index_column c\n" +
            "WHERE c.is_included_column = 0\n" +
            "    AND c.index_id = i.index_id\n" +
            "FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')'  \n" +
            "+ ISNULL(CHAR(13) + 'INCLUDE (' + \n" +
            "    STUFF((\n" +
            "    SELECT ', [' + c.name + ']'\n" +
            "    FROM index_column c\n" +
            "    WHERE c.is_included_column = 1\n" +
            "        AND c.index_id = i.index_id\n" +
            "    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')', '')  + CHAR(13)\n" +
            "FROM sys.indexes i WITH (NOWAIT)\n" +
            "WHERE i.[object_id] = @object_id\n" +
            "AND i.is_primary_key = 0\n" +
            "AND i.[type] = 2\n" +
            "FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')\n" +
            "), '')\n" +

            "PRINT @SQL\n";
            return M_query2;
        }

        public void Recorrer_Dataset(DataSet Ds)
        {
            foreach (DataRow item in Ds.Tables[0].Rows)
            {
                Ds_Fields = new Object[Convert.ToInt32(item.ItemArray.Count())];
                Ds_Fields = item.ItemArray;
                var a = Ds_Fields[0].ToString();
            }
        }

        public string RemoverCaracteresEspeciales(string pCadena)
        {
            string[] replacement =
                {
                    "a", "a", "a", "a", "a", "a", "c", "e", "e", "e", "e", "i", "i", "i", "i", "n", "o",
                    "o", "o", "o", "o", "u", "u", "u", "u", "y", "y"
                };
            string[] accents =
                {
                    "à", "á", "â", "ã", "ä", "å", "ç", "é", "è", "ê", "ë", "ì", "í", "î", "ï", "ñ", "ò", 
                    "ó", "ô", "ö", "õ", "ù", "ú", "û", "ü", "ý", "ÿ"
                };
            for (var i = 0; i < accents.Length; i++)
            {
                pCadena = pCadena.ToLower().Replace(accents[i], replacement[i]);
            }
            return pCadena.ToUpper().Trim();
        }

        public bool EjecutarSP(string nombreSP, SqlParameter[] parametroSP, string sqlc)
        {
            SqlConnection conn2 = new SqlConnection(sqlc);
            try
            {
                if (conn2.State != ConnectionState.Open)
                {
                    conn2.Open();
                }
                SqlCommand cmd2 = new SqlCommand(nombreSP, conn2);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlParameter[] parametrossp = new SqlParameter[parametroSP.Length];
                for (int i = 0; i < parametroSP.Length; i++)
                {
                    parametrossp[i] = parametroSP[i];
                    cmd2.Parameters.Add(parametrossp[i]);
                }
                cmd2.ExecuteNonQuery();
                conn2.Close();
                return true;
            }
            catch (Exception ex)
            {
                conn2.Close();
                return false;
            }
        }

        public DataSet EjecutarSP_dataset(string nombreSP, SqlParameter[] parametroSP, string sqlc)
        {
            DS_remove();
            SqlConnection cone = new SqlConnection(sqlc);
            try
            {
                if (cone.State != ConnectionState.Open)
                {
                    cone.Open();
                }
                SqlCommand cmd2 = new SqlCommand(nombreSP, cone);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Dt = new SqlDataAdapter();
                Dt = new SqlDataAdapter(cmd2);
                SqlParameter[] parametrossp = new SqlParameter[parametroSP.Length];
                for (int i = 0; i < parametroSP.Length; i++)
                {
                    parametrossp[i] = parametroSP[i];
                    cmd2.Parameters.Add(parametrossp[i]);
                }
                cone.Close(); Dt.Fill(_Ds);
                return _Ds;
            }
            catch (Exception ex)
            {
                cone.Close(); return _Ds;
            }
        }

        public class CBL
        {
            public string value { get; set; }
            public string text { get; set; }
        }

        public List<CBL> SQL_ComboBox(DataSet _Ds, string VALUE1, string VALUE2)
        {
            List<CBL> ListCB = new List<CBL>();
            foreach (DataRow _DRow in _Ds.Tables[0].Rows)
            {
                //if (_DRow[VALUE1].ToString().TrimEnd() != "" && _DRow[VALUE2].ToString().TrimEnd() != ""){
                ListCB.Add(new CBL
                {
                    value = _DRow[VALUE2].ToString(),
                    text = _DRow[VALUE1].ToString()
                });
                //}
            }
            return ListCB;
        }

        public DateTime Server_Time(string sqlc)
        {
            DS_remove();
            string sqlq = "SELECT GETDATE()";
            _Ds = trans_dataset(sqlq, sqlc);
            return Convert.ToDateTime(_Ds.Tables[0].Rows[0][0]);
        }

        //using System.Security.Cryptography;
        public string cifradoMD5(string pass)
        {
            byte[] hash = String_ByteArray(pass);
            byte[] valorhash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(hash);
            string cod = BitConverter.ToString(valorhash);
            string cod2 = cod.Replace("-", "");
            return cod2;
        }

        public Byte[] String_ByteArray(String _Pr_Str_Valor)
        {
            return (new UnicodeEncoding()).GetBytes(_Pr_Str_Valor);
        }
    }
}
