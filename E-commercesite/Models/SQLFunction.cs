using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace E_commercesite.Models
{
    public class SQLFunction
    {
        public String conn;

        public SQLFunction(String conn)
        {
            this.conn = conn;
        }

        public void InsertUser(String username, String password)
        {
            using(SqlConnection sql = new SqlConnection(conn))
            {
                sql.Open();
                String sqlCommand = "insert into Users(username, password) values (@us, @ps)";
                SqlCommand command = new SqlCommand(sqlCommand, sql);
                command.Parameters.AddWithValue("@us", username);
                command.Parameters.AddWithValue("@ps", password);
                command.ExecuteNonQuery();
            }
        }
    }
}
