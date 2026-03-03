using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//namespace CodeLearningAcademy.App_Code
//{
//    public class DBHelper
//    {
//    }
//}
public static class DBHelper
{
  /// <summary>
  ///   Deez nuts
  /// </summary>
    private static string constr =>
        ConfigurationManager.ConnectionStrings["assignment"].ConnectionString;
    // Execute insert, update, delete stuff and shows rows affected

    public static int ExecuteNonQuery(string sql, params SqlParameter[] prms)
    {
        using (var con = new SqlConnection(constr))
        using (var cmd = new SqlCommand(sql, con))
        {
            cmd.Parameters.AddRange(prms);
            con.Open();
            return cmd.ExecuteNonQuery();
        }
    }
    public static object ExecuteScalar(string sql, params SqlParameter[] prms)
    {
        using (var con = new SqlConnection(constr))
        using (var cmd = new SqlCommand(sql, con))
        {
            cmd.Parameters.AddRange(prms);
            con.Open();
            return cmd.ExecuteScalar();
        }
    }

    // Get data from database and return as datatable(this grabs our stuff)

    public static DataTable GetDataTable(string sql, params SqlParameter[] prms)
    {
        using (var con = new SqlConnection(constr))
        using (var cmd = new SqlCommand(sql, con))
        using (var da = new SqlDataAdapter(cmd))
        {
            cmd.Parameters.AddRange(prms);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    //Gets data from rows and returns as a list of objects
    public static DataRow GetDataRow(string sql, params SqlParameter[] prms)
    {
        var dt = GetDataTable(sql, prms);
        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
    }

    public static SqlParameter P(string name, object value)
        => new SqlParameter(name, value ?? DBNull.Value);

    public static SqlParameter PInt(string name, int value)
    => new SqlParameter(name, SqlDbType.Int) { Value = value};

    public static SqlParameter PVarchar(string name, string value,int size = 255)
    => new SqlParameter(name, SqlDbType.VarChar) { Value = (object)value ?? DBNull.Value };

    public static SqlParameter PBit(string name, bool value)
    => new SqlParameter(name, SqlDbType.Bit) { Value = value };
}