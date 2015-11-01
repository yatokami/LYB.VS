using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace LYB
{
    public static class SqlHelper
    {
        public static readonly string connstr = ConfigurationManager.ConnectionStrings["lybDB"].ConnectionString;
        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            return conn;
        }
        public static int ExecuteNonQuery(string cmdText,params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteNonQuery(conn, cmdText, parameters);
            }
        }
        public static int ExecuteNonQuery(SqlConnection conn,string cmdText,params SqlParameter[] parameters)
        {
            using(SqlCommand cmd=conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
        public static object ExecuteScalar(string cmdText, params SqlParameter[] parameters)
        {
            using(SqlConnection conn=new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteScalar(conn, cmdText, parameters);
            }
        }
        public static object ExecuteScalar(SqlConnection conn,string cmdText,params SqlParameter[] parameters)
        {
            using(SqlCommand cmd=conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }
        public static DataTable ExecuteDataTable(string cmdText,params SqlParameter[] parameters)
        {
            using(SqlConnection conn=new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteDataTable(conn, cmdText, parameters);
            }
        }
        public static DataTable ExecuteDataTable(SqlConnection conn,string cmdText,params SqlParameter[] parameters)
        {
            using(SqlCommand cmd=conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                using(SqlDataAdapter da=new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}