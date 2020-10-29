using Core.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blog.Repository.DB
{
   public class DapperContext
    {
        public static IDbConnection connection()
        {
            return new MySqlConnection(connStr);
        }
        private static string connStr = null;
        private DapperContext()
        {

        }
        static DapperContext()
        {
            connStr = ConfigureProvider.configuration.GetSection("ConnectionStrings:MySqlConnection").Value;
        }
    }
}
