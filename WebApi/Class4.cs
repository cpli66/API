using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebApplication1.API
{
    /// <summary>
    /// MyHandler 的摘要说明
    /// </summary>
    public class MyHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            HandlerResultModel mHandlerResult = null;
            string sActionType = context.Request.Params.Get("ActionType") == null ? "" : context.Request["ActionType"].Trim();
            string callback = HttpContext.Current.Request["jsoncallback"];
            try
            {
                //接口1
                if (sActionType.Equals("Query"))
                {
                    string sDate = context.Request.Params.Get("Date") == null ? "" : context.Request["Date"].Trim();
                    DataSet ds = Query(sDate);
                    mHandlerResult = new HandlerResultModel("T", ds, "success");
                }
                //接口2
                else if (sActionType.Equals("Test"))
                {
                    mHandlerResult = new HandlerResultModel("T", "", "success");
                }

            }
            catch (Exception ex)
            {
                mHandlerResult = new HandlerResultModel("F", "", ex.Message);
            }

            //转换为Json格式
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            string sJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(mHandlerResult);
            context.Response.Write(callback + "(" + sJsonResult + ")");
            context.Response.End();
        }


        /// <summary>
        /// 数据库查询
        /// </summary>
        /// <param name="Date">日期</param>
        /// <returns></returns>
        public DataSet Query(string Date)
        {
            string sConnection = "Server={sqlIP};Integrated Security=no;User Id={sqlID};PWD={sqlPassword};initial catalog = {sqlTablename}; Connect Timeout = 60000;";
            string sql = string.Format($@"SELECT MachineName,StationNo,RunningTime,PowerOnTime,PowerOffTime,MoNoCount,OEE FROM EquFarming
                                        WHERE CreateDate>='{Date}'");
            //连接数据库查询数据并获取记录
            DataSet ds = SqlHelper.ExecuteDataset(sConnection, CommandType.Text, sql);
            if (ds.Tables.Count < 1)
            {
                throw new Exception("获取数据库出错");
            }
            return ds;
        }

    }



