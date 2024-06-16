using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppADO.DataAccess
{
    internal class CustomerDataAccess : BaseDataAccess
    {
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            string sql = "sp_GetAllCustomers";

            try
            {
                var reader = ExecuteReader(sql,System.Data.CommandType.StoredProcedure,
                    parameters: new System.Data.SqlClient.SqlParameter("@filter",""));
                while (reader.Read())
                {
                    var obj = new Customer
                    {
                        CustomerId = reader.IsDBNull(0) ? "" : reader.GetString(0),
                        CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        ContactName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        City = reader.IsDBNull(3)?"":reader.GetString(3),
                        Country = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    };
                    customers.Add(obj);
                }
                if(!reader.IsClosed) reader.Close();
            }
            catch (SqlException Sqle)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return customers;
            
        }

        public Customer GetCustomersById(string id)
        {
            Customer customers = null;
            string sql = "sp_GetCustomer";
            try
            {
                var reader = ExecuteReader(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                    parameters: new SqlParameter("@customerId", id));
                while (reader.Read())
                {
                    customers = new Customer
                    {
                        CustomerId = reader.IsDBNull(0) ? "" : reader.GetString(0),
                        CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        ContactName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        City = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Country = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    };
                }
                if (!reader.IsClosed) reader.Close();
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return customers;
        }

        public void InsertIntoCustomers(Customer customer)
        {
           
            string sql = "sp_InsertCustomer";
            try
            {
                ExecuteNonQuery(
                   sqltext: sql,
                   commandType: CommandType.StoredProcedure,
                       new SqlParameter("@customerId", customer.CustomerId),
                       new SqlParameter("@customerId", customer.CompanyName),
                       new SqlParameter("@customerId", customer.ContactName),
                       new SqlParameter("@customerId", customer.City),
                       new SqlParameter("@customerId", customer.Country)
                  );
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public void UpadteNewCustomer(Customer model)
        {
            string sql = "sp_UpdateCustomer";
            try
            {
                ExecuteNonQuery(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                        new SqlParameter("@customerId", model.CustomerId),
                        new SqlParameter("@company", model.CompanyName),
                        new SqlParameter("@contact", model.ContactName),
                        new SqlParameter("@city", model.City),
                        new SqlParameter("@country", model.Country));
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
