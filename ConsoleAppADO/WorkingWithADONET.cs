using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ConsoleAppADO
{
    public class WorkingWithADONET
    {
        internal static void Test()
        {
            // define the connection string
            var connStr = @"Server=(local)\MSSQLSERVER123;database=Northwind;integrated security=sspi;trustservercertificate=true";
            var sqlText = "SELECT CustomerID, CompanyName, ContactName, City, Country FROM Customers";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connStr;
            connection.Open();

            var command = new SqlCommand();
            command.CommandText = sqlText;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Id: {0}, Company: {1}\n\tContact: {2}\n\tCity: {3}, Country: {4}",
                    reader.GetString(0), reader["CompanyName"].ToString(),
                    reader[2].ToString(), reader.GetString(3), reader.GetString(4));
            }
            reader.Close();

            connection.Close();
        }
        // Extract ProductId (Int), ProductName (String), UnitPrice (decimal), unitInStock (short), discontinued (bool) from Products and print it
        internal static void Test2()
        {
            var connStr = @"Server=(local)\MSSQLSERVER123;database=Northwind;integrated security=sspi;trustservercertificate=true";
            var sqlText = "SELECT ProductId, ProductName, UnitPrice, unitsInStock, discontinued FROM Products";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connStr;
            connection.Open();

            var command = new SqlCommand();
            command.CommandText = sqlText;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Id: {0}\tProduct: {1}\n\tUnit Price: {2}\tunit In Stock: {3}\tis discontinued: {4}",
                    reader.GetInt32(0), reader["ProductName"].ToString(),
                    reader.GetDecimal(2), reader.GetInt16(3), reader.GetBoolean(4));
            }
            reader.Close();

            connection.Close();



        }
        //read 2 or more queries at the same time.(below)
        internal static void Test3()
        {
            var connStr = @"Server=(local)\MSSQLSERVER123;database=Northwind;integrated security=sspi;trustservercertificate=true;multipleactiveresultsets=true";
            var sqlText = "SELECT CategoryID, CategoryName,Description FROM Categories;" +
                "SELECT ProductId, ProductName, UnitPrice, unitsInStock, discontinued FROM Products;";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connStr;
            connection.Open();

            var command = new SqlCommand();
            command.CommandText = sqlText;
            command.CommandType = CommandType.Text;
            command.Connection = connection;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Id: {0}\tName: {1}\n\tDescription: {2}\t",
                    reader[0].ToString(), reader[1].ToString(),
                    reader[2].ToString());
            }

            reader.NextResult();//for next stmt among the two ,to execute 2nd stmt use this(one for each result set.

            while (reader.Read())
            {
                Console.WriteLine("Id: {0}\tName: {1}\n\tPrice: {2}\tStock: {3}\tIn stock: {4}",
                    reader.GetInt32(0), reader["ProductName"].ToString(),
                    reader.GetDecimal(2), reader.GetInt16(3), reader.GetBoolean(4));
            }
            reader.Close();

            connection.Close();
        }
        // Test 4 which is multiple acitve results sets
        internal static void Test4()
        {
            //define the connection string 
            var connStr =
            @"Server=(local);database=northwind;integrated security=sspi;trustservercertificate=true;multipleactiveresultsets=true";
            //var connStr = @"Server=(local)\MSSQLSERVER2;database=northwind;integrated security=sspi";
            var sqlText1 = "SELECT CategoryId, CategoryName, Description FROM Categories;";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connStr;
            connection.Open();

            var command1 = new SqlCommand();
            command1.CommandText = sqlText1;
            command1.CommandType = CommandType.Text;
            command1.Connection = connection;
            var reader1 = command1.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("".PadLeft(80, '-'))
                .AppendFormat("{0,-5}{1,-40}{2}\n", "Id", "Name", "Description")
                .AppendLine("".PadLeft(80, '-'));
            while (reader1.Read())
            {
                sb.AppendFormat("{0,-5}{1,-40}{2}\n", reader1.GetInt32(0), reader1.GetString(1), reader1.GetString(2));
                var sqlText2 = " SELECT ProductId, ProductName, UnitPrice, UnitsinStock,Discontinued FROM Products " +
                " WHERE CategoryId = " + reader1["CategoryId"].ToString();
                var command2 = new SqlCommand();
                command2.CommandText = sqlText2;
                command2.CommandType = CommandType.Text;
                command2.Connection = connection;
                var reader2 = command2.ExecuteReader();
                sb.AppendLine("".PadLeft(80, '-'))
                .AppendFormat("{0,-5}{1,-40}{2,-10}{3,-10},{4}\n", "Id", "Name", "Price", "Stock", "Discontinued")
                .AppendLine("".PadLeft(80, '-'));
                while (reader2.Read())
                {
                    sb.AppendFormat("{0,-5}", reader2.GetInt32(0))
                        .AppendFormat("{0,-40}", reader2.GetString(1))
                        .AppendFormat("{0,-10}", reader2.GetDecimal(2))
                        .AppendFormat("{0,-10}", reader2.GetInt16(3))
                        .AppendFormat("{0}", reader2.GetBoolean(4))
                        .Append("\n");
                }
                if (!reader2.IsClosed) reader2.Close();

                sb.AppendLine("\n".PadLeft(80, '='));
            }
            Console.WriteLine(sb.ToString());

            if (!reader1.IsClosed) reader1.Close();
            if (connection != null)
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
        }

        internal static void Test5()
        {
            //define the connection string 
            var connStr =
            @"Server=(local);database=northwind;integrated security=sspi;trustservercertificate=true;multipleactiveresultsets=true";
            //var connStr = @"Server=(local)\MSSQLSERVER2;database=northwind;integrated security=sspi";


            Console.Clear();
            Console.WriteLine("Enter the Category id");
            string Id = Console.ReadLine();


            var sqlText1 = " SELECT CategoryId, CategoryName, Description FROM Categories " +
                " WHERE CategoryId = @catId";


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connStr;
            connection.Open();

            var command1 = new SqlCommand();
            command1.CommandText = sqlText1;
            command1.CommandType = CommandType.Text;
            command1.Connection = connection;
            

            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@catId";
            p1.SqlDbType = SqlDbType.Int;
            p1.Size = 4;
            p1.Direction = ParameterDirection.Input;
            p1.Value = Id;
            command1.Parameters.Add(p1);

            var reader1 = command1.ExecuteReader();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("".PadLeft(80, '-'))
                .AppendFormat("{0,-5}{1,-40}{2}\n", "Id", "Name", "Description")
                .AppendLine("".PadLeft(80, '-'));

            while (reader1.Read())
                sb.AppendFormat("{0,-5}{1,-40}{2}\n", reader1.GetInt32(0), reader1.GetString(1), reader1.GetString(2));


            // close connection and reader
            Console.WriteLine(sb.ToString());

            if (!reader1.IsClosed) reader1.Close();
            if (connection != null)
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
        }

        static string connStr =
          @"Server=(local);database=northwind;integrated security=sspi;trustservercertificate=true;multipleactiveresultsets=true";
        internal static void Test6()
        {
            var sqlText1 = "INSERT INTO Customers (CustomerID, Companyname, ContactName, City, Country) " +
                " VALUES (@custId, @company, @contact, @city, @country) ";

            string id, company, contact, city, country;
            Console.Write("Enter ID: ");
            id = Console.ReadLine();
            Console.Write("Enter Company: ");
            company = Console.ReadLine();
            Console.Write("Enter Contact: ");
            contact = Console.ReadLine();
            Console.Write("Enter City: ");
            city = Console.ReadLine();
            Console.Write("Enter Country: ");
            country = Console.ReadLine();

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connStr;
            connection.Open();

            var command1 = new SqlCommand();
            command1.CommandText = sqlText1;
            command1.CommandType = CommandType.Text;
            command1.Connection = connection;

            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@custId";
            p1.Size = 5;
            p1.SqlDbType = SqlDbType.VarChar;
            p1.Value = id;
            command1.Parameters.Add(p1);

            SqlParameter p2 = new SqlParameter("@company", SqlDbType.VarChar, 50);
            p2.Value = company;
            command1.Parameters.Add(p2);

            SqlParameter p3 = new SqlParameter("@contact", contact);
            command1.Parameters.Add(p3);

            command1.Parameters.AddWithValue("@city", city);
            command1.Parameters.AddWithValue("@country", country);
            try
            {
                command1.ExecuteNonQuery();
                Console.WriteLine("Row inserted into the table.");
            }
            catch (SqlException sqle)
            {
                foreach (SqlError error in sqle.Errors)
                { Console.WriteLine(error.Message); }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        internal static void Test7()
        {
            var sqlText1 = "sp_GetCustomer";

            string id, company, contact, city, country;
            Console.Write("Enter ID: ");
            id = Console.ReadLine();
            /*Console.Write("Enter Company: ");
            company = Console.ReadLine();
            Console.Write("Enter Contact: ");
            contact = Console.ReadLine();
            Console.Write("Enter City: ");
            city = Console.ReadLine();
            Console.Write("Enter Country: ");
            country = Console.ReadLine();*/

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connStr;
            connection.Open();

            var command1 = new SqlCommand();
            command1.CommandText = sqlText1;
            command1.CommandType = CommandType.StoredProcedure;
            command1.Connection = connection;

            SqlParameter p1 = new SqlParameter();
            p1.ParameterName = "@customerId";
            p1.Size = 5;
            p1.SqlDbType = SqlDbType.VarChar;
            p1.Value = id;
            command1.Parameters.Add(p1);

            var reader = command1.ExecuteReader();
            if (reader != null) { 
            Console.WriteLine("I got the content ");
                while (reader.Read())
                {
                    Console.WriteLine("Id: {0}, Company: {1}\n\tContact: {2}\n\tCity: {3}, Country: {4}",
                        reader.GetString(0), reader["CompanyName"].ToString(),
                        reader[2].ToString(), reader.GetString(3), reader.GetString(4));
                }
            }
            else
                Console.WriteLine("NO data was fetched");


            /*SqlParameter p2 = new SqlParameter("@company", SqlDbType.VarChar, 50);
            p2.Value = company;
            command1.Parameters.Add(p2);

            SqlParameter p3 = new SqlParameter("@contact", contact);
            command1.Parameters.Add(p3);

            command1.Parameters.AddWithValue("@city", city);
            command1.Parameters.AddWithValue("@country", country);*/
            /*try
            {
                command1.ExecuteNonQuery();
                Console.WriteLine("Row inserted into the table.");
            }
            catch (SqlException sqle)
            {
                foreach (SqlError error in sqle.Errors)
                { Console.WriteLine(error.Message); }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }*/
            reader.Close();
                connection.Close();
            
        }
    }
}
