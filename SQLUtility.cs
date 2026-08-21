using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace CPUFramework
{
    public class SQLUtility
    {
        public static string ConnectionString = "";
        public static DataTable GetDataTable(string sqlstatement)
        {
            DataTable dt = new();
            SqlConnection conn = new();
            conn.ConnectionString = ConnectionString;
            conn.Open();
            var cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlstatement;
            var dr = cmd.ExecuteReader();
            dt.Load(dr);
            SetAllColoumnsAllowNull(dt);
            return dt;
        }

        public static void ExecuteSQL(string sqlstatement)
        {
            GetDataTable(sqlstatement);
        }

        private static void SetAllColoumnsAllowNull(DataTable dt)
        {
            foreach(DataColumn c in dt.Columns)
            {
                c.AllowDBNull = true;
            }
        }
        
        public static void DebugPrintDataTable(DataTable dt)
        {
            foreach(DataRow r in dt.Rows)
            {
               foreach(DataColumn c in dt.Columns)
                {
                    Debug.Print(c.ColumnName + " = " + r[c.ColumnName].ToString());
                }
            }
        }

        public static int GetFirstColumnsFirstRowValueInt(string sql)
        {
          return Convert.ToInt32(GetFirstColumnsFirstRowValue(sql, "int"));
        }

        public static object GetFirstColumnsFirstRowValue(string sql, string typeofcharacters)
        {
            string s = "";
            int n = 0;
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
            {
                if (dt.Rows[0][0] != DBNull.Value)
                {
                    if (typeofcharacters == "string")
                    {
                        s = dt.Rows[0][0].ToString();
                    }
                    else
                    {
                        int.TryParse(dt.Rows[0][0].ToString(), out n);
                        return n;
                    }
                }
            }
            return s;
        }
    }
}