using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Threading;
using DMC.Models;
using System.Collections.Generic;


namespace DMC.Helper
{
    /// <summary>
    /// 对SQLite操作帮助类
    /// </summary>
    public partial class SQLiteDataBase
    {
        // 获取当前程序的安装路径  
        private static string installPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        // 修改连接字符串的创建方式
        public static string DMC_SQLiteDBFilePath = Path.Combine(installPath, "DMC_SQLiteDB.db");
        // 数据库文件路径及名称
        //public static readonly string SQLiteConnectionString = $"Data Source={DMC_SQLiteDBFilePath};Version=3;New=False;Compress=True;Journal Mode=WAL;Synchronous=Normal;Locking Mode=Normal;";

        // 添加这个方法来获取SQLite连接字符串
        public static string GetConnectionString()
        {
            return $"Data Source={DMC_SQLiteDBFilePath};Version=3;Journal Mode=WAL;Synchronous=Normal;Locking Mode=Normal;";
        }

        //使用Builder写语句，实现分段
        //与数据库连接的信息
        public static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
        {
            //数据库连接时的用户名，可以用pid
            UserID = "root",
            //数据库连接时的密码，可以用pwd
            Password = "123456",
            //数据库连接时的服务器地址         
            Server = AppGlobalModel.ServiceAddress,
            Port = 3306,
            //要连接的数据库
            Database = "file-manager"
        };

        public static MySqlConnectionStringBuilder dmcBuilder = new MySqlConnectionStringBuilder()
        {
            UserID = "dmcremote",
            Password = "dmcremote123",
            Server = AppGlobalModel.ServiceAddress,
            Port = 3306,
            Database = "file-manager",
            ConnectionTimeout = 30,
            DefaultCommandTimeout = 30
        };

        /// <summary>
        /// 存储ApplyListModel对象的列表                                                                               
        /// </summary>
        private static ApplyListModel applyItemTemp = new ApplyListModel();

        /// <summary>
        /// 存储ApplyListModel对象的列表                                                                               
        /// </summary>
        private static List<ApplyListModel> applyListTemp = new List<ApplyListModel>();

        /// <summary>
        /// 数据库文件名
        /// </summary>
        private string _DMC_SQLiteDBFilePath = "";
        /// <summary>
        /// 连接对象
        /// </summary>
        //private SQLiteConnection _SQLiteConn = null;
        /// <summary>
        /// 事务对象
        /// </summary>
        private SQLiteTransaction _SQLiteTrans = null;
        /// <summary>
        /// 事务运行标识
        /// </summary>
        private bool _IsRunTrans = false;
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string _SQLiteConnString = null;
        /// <summary>
        /// 事务自动提交标识
        /// </summary>
        //private bool _AutoCommit = false;
        /// <summary>
        /// 对SQLite操作帮助类： 所有成员函数都是静态的，构造函数定义为私有
        /// </summary>
        /// <param name="dbPath">数据库文件路径及名称</param>
        public SQLiteDataBase()
        {
            this._DMC_SQLiteDBFilePath = DMC_SQLiteDBFilePath;
            this._SQLiteConnString = "Data Source=" + DMC_SQLiteDBFilePath;

            //后台运行
            Thread thread = new Thread(() =>
            {
                // 检查SQLite文件是否存在  
                if (!File.Exists(DMC_SQLiteDBFilePath))
                {
                    //创建一个数据库db文件
                    SQLiteDataBase.NewDbFile(DMC_SQLiteDBFilePath);
                }
                // 连接MySQL并从qz_user表中拿到数据给变量；  
                DataTable mysqlqz_userData = GetDataFromMysql("qz_user");
                CreateTable("qz_user", mysqlqz_userData);

                //连接MySQL并从qz_approval表中拿到数据给变量；  
                DataTable mysql_qz_approvalList_Data = GetDataFromMysql("qz_approval");
                CreateTable("qz_approval", mysql_qz_approvalList_Data);

                //连接MySQL并从qz_major表中拿到数据给变量；  
                DataTable mysql_qz_major_Data = GetDataFromMysql("qz_major");
                CreateTable("qz_major", mysql_qz_major_Data);

                //连接MySQL并从qz_role表中拿到数据给变量；  
                DataTable mysql_qz_role_Data = GetDataFromMysql("qz_role");
                CreateTable("qz_role", mysql_qz_role_Data);

                //连接MySQL并从qz_dept表中拿到数据给变量；  
                DataTable mysql_qz_dept_Data = GetDataFromMysql("qz_dept");
                CreateTable("qz_dept", mysql_qz_dept_Data);

                //连接MySQL并从qz_user_dept_post表中拿到数据给变量；  
                DataTable mysql_qz_user_dept_post_Data = GetDataFromMysql("qz_user_dept_post");
                CreateTable("qz_user_dept_post", mysql_qz_user_dept_post_Data);

                //连接MySQL并从qz_approval_node表中拿到数据给变量；  
                DataTable mysql_qz_approval_node_Data = GetDataFromMysql("qz_approval_node");
                CreateTable("qz_approval_node", mysql_qz_dept_Data);

                //连接MySQL并从qz_project_user_role表中拿到数据给变量；  
                DataTable mysql_qz_project_user_role_Data = GetDataFromMysql("qz_project_user_role");
                CreateTable("qz_project_user_role", mysql_qz_project_user_role_Data);

                ////连接MySQL并从qz_approval_apply_node_result表中拿到数据给变量；  
                //DataTable mysql_qz_approval_apply_node_result_Data = GetDataFromMysql("qz_approval_apply_node_result");
                //CreateTable("qz_approval_apply_node_result", mysql_qz_project_user_role_Data);

                ////连接MySQL并从qz_project_user表中拿到数据给变量；
                //DataTable mysqlqz_project_userData = GetDataFromMysql(builder.ConnectionString, "qz_project_user");qz_approval_apply_node_result
                //CreateTable(DMC_SQLiteDBFilePath, "qz_project_user", mysqlqz_project_userData);

                ////连接MySQL并从qz_project_user_role表中拿到数据给变量；
                //DataTable mysqlqz_project_user_roleData = GetDataFromMysql(builder.ConnectionString, "qz_project_user_role");
                //CreateTable(DMC_SQLiteDBFilePath, "qz_project_user_role", mysqlqz_project_user_roleData);

                ////连接MySQL并从qz_project_user_role表中拿到数据给变量；
                //DataTable mysqlqz_project_user_roleData = GetDataFromMysql(builder.ConnectionString, "qz_user");
                //CreateTable(DMC_SQLiteDBFilePath, "qz_user", mysqlqz_project_user_roleData);

            });
            thread.Start();

        }

        /// <summary>
        /// 连接Mysql数据库并取回数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>返回的DataTable数据表</returns>
        public static DataTable GetDataFromMysql(string tableName)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                try
                {
                    //打开通道，建立连接
                    connection.Open();
                    Console.WriteLine("连接成功！");
                }
                catch (MySqlException ex)
                {
                    //有异常，打印错误信息到控制台
                    MessageBox.Show($"{ex.Message}");
                }

                try
                {
                    //搜索数据库
                    var adapter = new MySqlDataAdapter($"SELECT * FROM {tableName}", connection);
                    //填入数据
                    adapter.Fill(dataTable);
                    connection.Close();
                }
                catch (MySqlException ex)
                {
                    //有异常，打印错误信息到控制台
                    MessageBox.Show($"{ex.Message}");
                }

            }
            return dataTable;
        }

        /// <summary>
        /// 连接Mysql数据库并取回数据（一般为qz_project表）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="typeNum">0：项目名称、1：阶段、2：专业、3：子项、4：文件夹、5：文件</param>
        /// <returns>返回查询的数据表</returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, int typeNum)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            // 创建MySQL连接
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                //打开链接
                connection.Open();
                // 创建SQL查询
                MySqlCommand mySqlCommand = connection.CreateCommand();//MySqlCommand mySqlCommand = new MySqlCommand(); mySqlCommand.Connection = connection;
                //赋值发送给数据库命令行文字；
                mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = '{typeNum}' ";
                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    adapter.Fill(dataTable);//填充数据
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 连接Mysql数据库并取回数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="searchStr">查询字符串</param>
        /// <returns>返回的DataTable数据表</returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, string searchStr)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();//MySqlCommand mySqlCommand = new MySqlCommand(); mySqlCommand.Connection = connection;
                //赋值发送给数据库命令行文字；
                mySqlCommand.CommandText = $"SELECT * FROM `{tableName}` WHERE `{columnName}` = '{searchStr}'";
                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    adapter.Fill(dataTable);//填充数据
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 连接数据库并取回数据
        /// </summary>
        /// <param name="tableName">链接的数据库表名</param>
        /// <param name="columnName">表的列名</param>
        /// <param name="startDateTime">查询的开始时间</param>
        /// <param name="endDateTime">查询的结束时间</param>
        /// <returns>返回的查询结果</returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, string startDateTime, string endDateTime)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();//MySqlCommand mySqlCommand = new MySqlCommand(); mySqlCommand.Connection = connection;
                //赋值发送给数据库命令行文字；
                mySqlCommand.CommandText = $"SELECT * FROM `{tableName}` WHERE `{columnName}` BETWEEN '{startDateTime}' AND '{endDateTime}'";

                #region Mysql的查询语句几种方法
                //查找在 2023 年 1 月创建的所有用户
                //SELECT * FROM user_table WHERE create_time >= '2023-01-01 00:00:00' AND create_time <= '2023-01-31 23:59:59';
                //查找在 2023 年 7 月创建的所有用户
                //SELECT * FROM user_table WHERE create_time BETWEEN '2023-07-01 00:00:00' AND '2023-07-31 23:59:59';
                #endregion

                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    adapter.Fill(dataTable);//填充数据
                }
                connection.Close();//关闭链接；
            }

            //applyItemTemp
            applyListTemp.Add(applyItemTemp);

            return dataTable;//返回数据

        }

        /// <summary>
        /// 连接Mysql数据库并取回数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="searchStr">查询字符串</param>
        /// <returns>返回的DataTable数据表</returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, string searchStr, string columnName2, string searchStr2)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();
                //赋值发送给数据库命令行文字；
                mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = '{searchStr}' AND {columnName2} = '{searchStr2}'";
                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    adapter.Fill(dataTable);//填充数据
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 连接Mysql数据库并取回数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列1名</param>
        /// <param name="ISNOTNULL">0：内容不为空，1：内空为空</param>
        /// <param name="columnName2">列2名</param>
        /// <param name="searchStr">要查询的字符串</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns>返回DataTable数据表</returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, string columnName2, string ISNOTNULL, string startDateTime, string endDateTime)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                var intISNOTNULL = Convert.ToInt32(ISNOTNULL);
                string isNotNull = null;
                if (intISNOTNULL == 0)
                {
                    isNotNull = "<> ''";
                }
                else
                {
                    isNotNull = "IS NULL";
                }
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();
                //赋值发送给数据库命令行文字；
                //mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} {isNotNull}  AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}'";

                mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} {isNotNull}  AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}' AND is_show = '0'";
                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    adapter.Fill(dataTable);//填充数据
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 连接Mysql数据库并取回数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列1名</param>
        /// <param name="ISNOTNULL">0：内容不为空，1：内空为空</param>
        /// <param name="columnName2">列2名</param>
        /// <param name="searchStr">要查询的字符串</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns>返回DataTable数据表</returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, string columnName2, string projectId, string ISNOTNULL, string startDateTime, string endDateTime, string isShow)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            // 创建MySQL连接
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                // 创建SQL查询
                var intISNOTNULL = Convert.ToInt32(ISNOTNULL);
                // 判断是否为空
                string isNotNull = null;
                if (intISNOTNULL == 0)
                {
                    isNotNull = "<> ''";
                }
                else
                {
                    isNotNull = "IS NULL";
                }
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();
                //赋值发送给数据库命令行文字；
                //mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} {isNotNull}  AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}'";
                //赋值发送给数据库命令行文字:带时间范围\列is_show = 0
                //mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} {isNotNull}  AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}' AND is_show = '0'";
                mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE project_id = '{projectId}' AND {columnName} {isNotNull}  AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}' AND is_show = '{isShow}' ";
                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    //dataTable 就包含了从数据库查询到的所有数据。
                    adapter.Fill(dataTable);
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 连接Mysql数据库并取回数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列1名</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="columnName2">列2名</param>
        /// <param name="isFileType">文件类型</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns>返回DataTable数据表</returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, string columnName2, string projectId, string isFileType, string startDateTime, string endDateTime)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            // 创建MySQL连接
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {

                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();
                //赋值发送给数据库命令行文字；                

                //mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE project_id = '{projectId}' AND '{columnName}' LIKE '%.{isFileType}' AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}' ";
                mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE project_id = '{projectId}' AND {columnName} LIKE '%.{isFileType}' AND status = '0' ";

                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    //dataTable 就包含了从数据库查询到的所有数据。
                    adapter.Fill(dataTable);
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 获取MySql数据库内数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName1">列名</param>
        /// <param name="searchStr1">查找字符串</param>
        /// <param name="columnName2">列名2</param>
        /// <param name="ISNOTNULL">是空还是所有</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns></returns>
        public static DataTable GetDataFromMysql(string tableName, string columnName, string columnName2, string searchStr, int ISNOTNULL, string startDateTime, string endDateTime)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                string isNotNull = null;
                if (ISNOTNULL == 0)
                {
                    isNotNull = "<> ''";
                }
                else
                {
                    isNotNull = "IS NULL";
                }
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();//MySqlCommand mySqlCommand = new MySqlCommand(); mySqlCommand.Connection = connection;
                //赋值发送给数据库命令行文字；
                mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} ='{searchStr}' AND {columnName2} {isNotNull} AND create_time BETWEEN '{startDateTime}' AND '{endDateTime}'";

                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    adapter.Fill(dataTable);//填充数据
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 连接Mysql数据库并取回数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列1名</param>
        /// <param name="ISNOTNULL">0：内容不为空，1：内空为空</param>
        /// <param name="columnName2">列2名</param>
        /// <param name="searchStr">要查询的字符串</param>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns>返回DataTable数据表</returns>
        public static DataTable GetApplyDataFromMysql(string tableName, string columnName, string columnName2, string searchStr, string startDateTime, string endDateTime)
        {
            // 从MySQL表中获取数据并返回DataTable  
            DataTable dataTable = new DataTable();
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();//MySqlCommand mySqlCommand = new MySqlCommand(); mySqlCommand.Connection = connection;
                //赋值发送给数据库命令行文字；
                mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = '{searchStr}' AND result = 1 AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}'";
                //mySqlCommand.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = '{searchStr}' AND {columnName2} BETWEEN '{startDateTime}' AND '{endDateTime}'";
                // 创建数据适配器对象,与服务器链接发送命令并取回数据。
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(mySqlCommand))
                {
                    adapter.Fill(dataTable);//填充数据
                }
                connection.Close();//关闭链接；
            }
            return dataTable;//返回数据
        }

        /// <summary>
        /// 从数据库获取真实的文件ID
        /// </summary>
        /// <param name="tempFileIdOrName">临时文件ID或文件名</param>
        /// <returns>真实的数据库文件ID</returns>
        public static string GetRealFileIdFromDatabase(string tempFileIdOrName)
        {
            try
            {
                // 使用现有的GetDataFromMysql方法查询
                DataTable resultTable = SQLiteDataBase.GetDataFromMysql(
                    "qz_project_file",
                    "name",
                    tempFileIdOrName,
                    "type",
                    "5" // 根据您的文件类型调整
                );

                if (resultTable != null && resultTable.Rows.Count > 0)
                {
                    // 获取最新的文件记录
                    return resultTable.Rows[0]["id"].ToString();
                }

                // 如果上面的查询没有结果，尝试其他查询方式
                // 根据您的数据库结构和字段调整
                // 使用现有的GetDataFromMysql方法，查询包含"补充"的记录
                DataTable resultAlternative = SQLiteDataBase.GetDataFromMysql("qz_project_file");

                if (resultAlternative != null)
                {
                    // 在结果中筛选包含"补充"的记录并按创建时间排序
                    var filteredRows = resultAlternative.AsEnumerable()
                        .Where(row => row["name"].ToString().Contains("补充"))
                        .OrderByDescending(row =>
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParse(row["create_time"].ToString(), out parsedDate))
                                return parsedDate;
                            else
                                return DateTime.MinValue;
                        })
                        .FirstOrDefault();

                    if (filteredRows != null)
                    {
                        return filteredRows["id"].ToString();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"查询真实文件ID时发生错误: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 查找SQLite中是不是有存在的项
        /// </summary>
        /// <param name="connection">SQLite中的连接</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static bool TableExists(SQLiteConnection connection, string tableName)
        {
            // 检查SQLite数据库中是否存在指定的表  
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            return (long)command.ExecuteScalar() > 0;
        }

        /// <summary>
        /// 新创建数据库文件
        /// </summary>
        /// <param name="dbPath">数据库文件路径及名称</param>
        /// <returns>新建成功，返回true，否则返回false</returns>
        public static Boolean NewDbFile(string dbPath)
        {
            try
            {
                // 检查路径是否存在  
                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("新建数据库文件" + dbPath + "失败：" + ex.Message);

            }
        }

        /// <summary>
        /// 搜索SQLite表数据
        /// </summary>
        /// <param name="tableName">要搜索的表名</param>
        /// <param name="columnName">要搜索的列名</param>
        /// <param name="searchString">要搜索的字符串</param>
        /// <returns></returns>
        public static DataTable SearchTableFromSQLite(string tableName, string columnName, string searchString)
        {
            // 使用 using 语句确保连接正确释放
            using (SQLiteConnection sqliteConn = new SQLiteConnection(GetConnectionString()))
            {
                sqliteConn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = @searchValue";
                    cmd.Parameters.AddWithValue("@searchValue", searchString);

                    using (SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        try
                        {
                            sQLiteDataAdapter.Fill(dataTable);
                        }
                        catch (SQLiteException ex)
                        {
                            Console.WriteLine($"数据库操作错误: {ex.Message}");
                            // 可以考虑重试机制
                            throw;
                        }
                        return dataTable;
                    }
                }
            }
            //建立数据库连接
            //using (SQLiteConnection sQLiteCon = new SQLiteConnection(DMC_SQLiteDBFilePath))
            //{
            //    SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + DMC_SQLiteDBFilePath);
            //    if (sqliteConn.State != System.Data.ConnectionState.Open)
            //    {
            //        sqliteConn.Open();
            //        SQLiteCommand cmd = new SQLiteCommand();
            //        cmd.Connection = sqliteConn;
            //        cmd.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = '{searchString}'";
            //        SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(cmd);
            //        // 从MySQL表中获取数据并返回DataTable  
            //        DataTable dataTable = new DataTable();
            //        sQLiteDataAdapter.Fill(dataTable);
            //        return dataTable;
            //    }
            //    sqliteConn.Close();
            //    return null;
            //}
        }

        /// <summary>
        /// 搜索SQLite表数据
        /// </summary>
        /// <param name="tableName">要搜索的表名</param>
        public static DataTable SearchTableFromSQLite(string tableName)
        {
            using (SQLiteConnection sqliteConn = new SQLiteConnection(GetConnectionString()))
            {
                sqliteConn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = $"SELECT * FROM {tableName}";

                    using (SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        try
                        {
                            sQLiteDataAdapter.Fill(dataTable);
                        }
                        catch (SQLiteException ex)
                        {
                            Console.WriteLine($"数据库操作错误: {ex.Message}");
                            throw;
                        }
                        return dataTable;
                    }
                }
            }
            //建立数据库连接
            //using (SQLiteConnection sQLiteCon = new SQLiteConnection(DMC_SQLiteDBFilePath))
            //{
            //    SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + DMC_SQLiteDBFilePath);
            //    if (sqliteConn.State != System.Data.ConnectionState.Open)
            //    {
            //        sqliteConn.Open();
            //        SQLiteCommand cmd = new SQLiteCommand();
            //        cmd.Connection = sqliteConn;
            //        cmd.CommandText = $"SELECT * FROM {tableName}";
            //        SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(cmd);
            //        // 从MySQL表中获取数据并返回DataTable  
            //        DataTable dataTable = new DataTable();
            //        sQLiteDataAdapter.Fill(dataTable);
            //        return dataTable;
            //    }
            //    sqliteConn.Close();
            //    return null;
            //}
        }

        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="mysqlData">表数据</param>
        public static void CreateTable(string tableName, DataTable mysqlData)
        {
            using (SQLiteConnection sqliteConn = new SQLiteConnection(GetConnectionString()))
            {
                sqliteConn.Open();

                //创建人员表
                if (!TableExists(sqliteConn, $"{tableName}"))
                {
                    // 在SQLite中创建表  
                    CreateSqliteTable(sqliteConn, $"{tableName}", mysqlData.Columns);
                }
                // 将数据从MySQL插入到SQLite  
                InsertOrUpdateDataToSqlite(sqliteConn, $"{tableName}", mysqlData);
            }
            //建立数据库连接
            //using (SQLiteConnection sQLiteCon = new SQLiteConnection(DMC_SQLiteDBFilePath))
            //{
            //    SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + DMC_SQLiteDBFilePath);
            //    if (sqliteConn.State != System.Data.ConnectionState.Open)
            //    {
            //        //打开链接
            //        sqliteConn.Open();

            //        //创建人员表
            //        if (!TableExists(sqliteConn, $"{tableName}"))
            //        {
            //            // 在SQLite中创建qz_user表  
            //            CreateSqliteTable(sqliteConn, $"{tableName}", mysqlData.Columns);
            //        }
            //        // 将数据从MySQL插入到SQLite  
            //        InsertOrUpdateDataToSqlite(sqliteConn, $"{tableName}", mysqlData);
            //    }
            //    //关闭链接
            //    sqliteConn.Close();
            //}
        }

        /// <summary>
        /// 在SQLite中创建表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        public static void CreateSqliteTable(SQLiteConnection connection, string tableName, DataColumnCollection columns)
        {
            // 根据MySQL表的列结构在SQLite中创建新表  
            var command = connection.CreateCommand();
            var createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (";
            foreach (DataColumn column in columns)
            {
                createTableQuery += $"{column.ColumnName} {GetSqliteDataType(column.DataType)}, ";
            }
            createTableQuery = createTableQuery.TrimEnd(", ".ToCharArray()) + ")";
            command.CommandText = createTableQuery;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入或更新数据
        /// </summary>
        /// <param name="connection">链接地址</param>
        /// <param name="tableName">表头</param>
        /// <param name="dataTable">表名</param>
        public static void InsertOrUpdateDataToSqlite(SQLiteConnection connection, string tableName, DataTable dataTable)
        {
            // 假设第一列是用于识别要更新/替换的行的键  
            string keyColumnName = dataTable.Columns[0].ColumnName;

            // 遍历 DataTable 中的每一行  
            foreach (DataRow row in dataTable.Rows)
            {
                // 使用 using 语句确保 command 对象的正确释放  
                using (var command = connection.CreateCommand())
                {
                    // 检查是否存在具有相同键的行  
                    command.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE {keyColumnName} = @{keyColumnName}";
                    command.Parameters.AddWithValue($"@{keyColumnName}", row[keyColumnName]);
                    long count = (long)command.ExecuteScalar();

                    // 如果存在具有相同键的行  
                    if (count > 0)
                    {
                        // 检查所有值是否相同  
                        if (!IsRowIdentical(connection, tableName, row, dataTable))
                        {
                            // 如果值不同，则删除旧行并插入新行  
                            command.CommandText = $"DELETE FROM {tableName} WHERE {keyColumnName} = @{keyColumnName}";
                            command.ExecuteNonQuery();
                            InsertRow(connection, tableName, row, dataTable);
                        } // 否则，行相同 - 跳过插入  
                    }
                    // 如果没有找到匹配的行  
                    else
                    {
                        // 插入新行  
                        InsertRow(connection, tableName, row, dataTable);
                    }
                }
            }
        }

        /// <summary>
        /// 更新Mysql数据库行
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="searchColName">查询列名</param>
        /// <param name="searchStr">查询字符串</param>
        /// <param name="upDataColName">更新的列表</param>
        /// <param name="upDataStr">更新字符串</param>
        public static void UpdateDataToMysql(string tableName, string searchColName, string searchStr, string upDataColName, string upDataStr)
        {
            using (var connection = new MySqlConnection(dmcBuilder.ConnectionString))
            {
                //打开链接
                connection.Open();
                MySqlCommand mySqlCommand = connection.CreateCommand();
                //赋值发送给数据库命令行文字；
                mySqlCommand.CommandText = $"UPDATE {tableName} SET {upDataColName}='{upDataStr}' WHERE {searchColName}='{searchStr}'";
                mySqlCommand.ExecuteNonQuery();
                connection.Close();//关闭链接；
            }


        }

        /// <summary>
        /// 插入新的行
        /// </summary>
        /// <param name="connection">链接地址</param>
        /// <param name="tableName">表头名</param>
        /// <param name="row">行</param>
        /// <param name="dataTable">表名</param>
        private static void InsertRow(SQLiteConnection connection, string tableName, DataRow row, DataTable dataTable)
        {
            using (var command = connection.CreateCommand())
            {
                // 获取列名和参数名  
                var columnNames = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                var parameterNames = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => "@" + c.ColumnName));
                command.CommandText = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";

                // 添加参数值  
                foreach (DataColumn column in dataTable.Columns)
                {
                    command.Parameters.AddWithValue($"@{column.ColumnName}", row[column] ?? DBNull.Value);
                }
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 判断是不是有重复的数据
        /// </summary>
        /// <param name="connection">链接地址</param>
        /// <param name="tableName">表头名</param>
        /// <param name="row">行</param>
        /// <param name="dataTable">表名</param>
        /// <returns></returns>
        private static bool IsRowIdentical(SQLiteConnection connection, string tableName, DataRow row, DataTable dataTable)
        {
            using (var command = connection.CreateCommand())
            {
                var columns = dataTable.Columns.Cast<DataColumn>();
                // 构建 where 子句  
                var whereClause = string.Join(" AND ", columns.Select(c => $"{c.ColumnName} = @{c.ColumnName}"));
                command.CommandText = $"SELECT * FROM {tableName} WHERE {whereClause}";

                // 添加参数值  
                foreach (DataColumn column in columns)
                {
                    command.Parameters.AddWithValue($"@{column.ColumnName}", row[column] ?? DBNull.Value);
                }

                using (var reader = command.ExecuteReader())
                {
                    // 如果找到匹配的行  
                    if (reader.Read())
                    {
                        // 比较所有列值  
                        foreach (DataColumn column in columns)
                        {
                            // 如果有任何列值不同，则返回 false  
                            if (!Equals(row[column], reader[column.ToString()]))
                                return false;
                        }
                        // 所有值都相同  
                        return true;
                    }
                    // 没有找到匹配的行  
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取SQLite中的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSqliteDataType(Type type)
        {
            // 将.NET数据类型映射到SQLite数据类型  
            switch (type.Name)
            {
                case "Int32":
                    return "INTEGER";
                case "String":
                    return "TEXT";
                case "DateTime":
                    return "DATETIME";
                case "Decimal":
                    return "REAL";
                default:
                    return "TEXT";
            }
        }

    }

}
