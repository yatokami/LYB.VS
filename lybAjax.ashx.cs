using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace LYB
{
    /// <summary>
    /// lybAjax 的摘要说明
    /// </summary>
    public class lybAjax : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"];
            if (action == "lia")
            {
                DataTable da = SqlHelper.ExecuteDataTable("select * from Lyb");
                int count=1;
                if (da.Rows.Count % 10 != 0)
                {
                     count = da.Rows.Count / 10 + 1;
                }
                else if (da.Rows.Count % 10 == 0)
                {
                     count = da.Rows.Count / 10;
                }
                context.Response.Write(count);
            }
            else if(action=="page")
            {
                int index = Convert.ToInt32(context.Request["index"].ToString());
                DataTable dt = SqlHelper.ExecuteDataTable("select * from Lyb");
                object[] comments = new object[10];
                for (int i = dt.Rows.Count - (index - 1) * 10-1, j = 0; j < 10&&i>=0;j++,i-- )
                {
                    DataRow row = dt.Rows[i];
                    DateTime createDT = (DateTime)row["date"];
                    comments[j] = new
                    {
                        name = row["name"],
                        Title = row["title"],
                        msg = row["msg"],
                        Time = createDT.ToString(),
                        IpAddress = row["IPAddress"],
                        img = row["img"]
                    };
                }
                string json = new JavaScriptSerializer().Serialize(comments);
                context.Response.Write(json);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}