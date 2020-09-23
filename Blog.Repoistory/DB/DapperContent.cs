
using Core.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blog.Repoistory.DB
{
    public class DapperContent
    {
        public static IDbConnection connection()
        {
            return new MySqlConnection(connStr);
        }
        private static string connStr = null;
        private DapperContent()
        {

        }
        static DapperContent()
        {
            connStr = ConfigureProvider.configuration.GetSection("ConnectionStrings:MySqlConnection").Value;
        }
    }
}
