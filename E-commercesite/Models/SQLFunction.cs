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
                sql.Close();
            }
        }

        public bool CheckUser(String username, String password)
        {
            using(SqlConnection sql = new SqlConnection(conn))
            {
                sql.Open();
                String sqlCommand = "select * from Users where Username=@us and Password=@ps";
                SqlCommand command = new SqlCommand(sqlCommand, sql);
                command.Parameters.AddWithValue("@us", username);
                command.Parameters.AddWithValue("@ps", password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Close();
                        sql.Close();
                        return true;
                    }
                }
                
                sql.Close();
            }

            return false;
        }

        public void InsertFood(String name, double price, byte[] image)
        {
            using (SqlConnection sql = new SqlConnection(conn))
            {
                sql.Open();
                SqlCommand command = new SqlCommand("insert into Food (Name, Price, Image) values (@nm, @pr, @im)", sql);
                command.Parameters.AddWithValue("@nm", name);
                command.Parameters.AddWithValue("@pr", price);
                command.Parameters.AddWithValue("@im", image);

                command.ExecuteNonQuery();
                sql.Close();
            }
        }

        public List<Food> RetriveFood(bool random)
        {
            List<Food> allFood = new List<Food>();

            using (SqlConnection sql = new SqlConnection(conn))
            {
                sql.Open();
                String query = (random) ? "select top 6 * from Food order by NEWID()" : "select * from Food";

                SqlCommand command = new SqlCommand(query, sql);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allFood.Add(new Food
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2),
                            ImageString = Convert.ToBase64String((byte[])reader.GetValue(3))
                        }); ;
                    }
                    reader.Close();
                }
                sql.Close();
            }


            return allFood;
        }

        public Food RetriveCart(String id)
        {
            using (SqlConnection sql = new SqlConnection(conn))
            {
                sql.Open();
                String query = "select * from Food where ID=@id";

                SqlCommand command = new SqlCommand(query, sql);
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Food food = new Food
                        {
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2),
                            ImageString = Convert.ToBase64String((byte[])reader.GetValue(3))
                        };
                        
                        return food;
        
                    }
                    reader.Close();
                }
                sql.Close();

            }

            return null;
        }

    
        public String GetUserCart(String username)
        {
            using (SqlConnection sql = new SqlConnection(conn))
            {
                sql.Open();
                String query = "select * from Users where Username=@us";
                SqlCommand command = new SqlCommand(query, sql);
                command.Parameters.AddWithValue("@us", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String item = reader.GetString(3);
                        reader.Close();
                        return item;
                        
                    }
                }
                sql.Close();
            }
                


            return "";
        }

        public void UpdateCart(String username, String list)
        {
            using(SqlConnection sql = new SqlConnection(conn))
            {
                sql.Open();
                String query = "update Users set Cart=@ls where Username=@us";
                SqlCommand command = new SqlCommand(query, sql);
                command.Parameters.AddWithValue("@ls", list);
                command.Parameters.AddWithValue("@us", username);
                command.ExecuteNonQuery();
                sql.Close();
            }
        }


    }
}
