using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace everything_webapi11
{
    public static class DBhelper
    {

        public static DataSet GetDataSetForSql(string sql, DbParameter[] paras, CommandType type, string connetionString)
        {
            //connetionString = EncodeAndDecode.Decode(connetionString);
            DataSet dataSet = new DataSet();
            using (var command = GetCommand(sql, connetionString, type))
            {
                command.CommandTimeout = 60 * 180;
                if ((paras != null) && (paras.Length > 0))
                {
                    command.Parameters.AddRange(paras);
                }
                try
                {
                    using (var da = new SqlDataAdapter(command))
                    {
                        da.SelectCommand.CommandTimeout = 60 * 180;
                        da.Fill(dataSet);
                        if (command.Transaction != null)
                            command.Transaction.Commit();
                    }
                }
                catch (Exception exception)
                {
                   
                    throw new Exception("ExecuteReader出错!" + exception.Message, exception);
                }
                finally
                {
                    if ((command != null) && (command.Connection != null))
                    {

                        command.Connection.Close();
                        //logger.Info(string.Format("debug---GetDataSetForSql 方法数据连接已关闭"));

                        //('GetDataSetForSql 方法数据连接已关闭');
                    }
                }
            }
            return dataSet;
        }
        public static object ExecuteScalar(string sqlStr, string connetionString)
        {
            //connetionString = EncodeAndDecode.Decode(connetionString);
            //DbConnection connection = null;
            object obj2 = null;
            Exception exception;
            SqlCommand command = GetCommand(sqlStr, connetionString, CommandType.Text);
            try
            {
                obj2 = command.ExecuteScalar();
                if (command.Transaction != null)
                    command.Transaction.Commit();
            }
            catch (Exception exception2)
            {
                exception = exception2;

                throw new Exception("ExecuteScalar出错!", exception);
            }
            finally
            {
                //connection.Close();
                if ((command != null) && (command.Connection != null))
                {
                    command.Connection.Close();
                    //logger.Info(string.Format("debug---ExecuteScalar 方法数据连接已关闭"));
                }
            }
            return obj2;
        }

        private static SqlCommand GetCommand(string sqlStr, string conStr, CommandType type, string providerName = "System.Data.SqlClient")
        {
            SqlConnection connection = null;
            string str = providerName;
            if ((str == null) || (str != "System.Data.SqlClient"))
            {
                throw new Exception("暂不支持");
            }
            connection = new SqlConnection(conStr);
            SqlCommand command = connection.CreateCommand();
            try
            {
                connection.Open();
                //var transaction = connection.BeginTransaction();
                //command.Transaction = transaction;
            }
            catch (Exception exception)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                throw new Exception("打开数据库连接出错!", exception);
            }
            command.CommandText = sqlStr;
            command.CommandType = type;
            return command;
        }
    }
}