using ConsoleAppADO.DataAccess;

namespace ConsoleAppADO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // WorkingWithADO1.Test();
            //WorkingWithADO1.Test2();
            //WorkingWithADONET.Test3();
            //WorkingWithADONET.Test4();
            //WorkingWithADONET.Test5();
            //WorkingWithADONET.Test6();
            //WorkingWithADONET.Test7();
            //TestGetCustomersById();
            InsertIntoCustomers();
            //TestGetAllCustomers();

        }

        static void TestGetAllCustomers()
        {
            CustomerDataAccess cda = new CustomerDataAccess();
            var list = cda.GetAllCustomers();
            list.ForEach(c =>
            {
                Console.WriteLine("ID: {0}, Company: {1}", c.CustomerId, c.CompanyName);
                Console.WriteLine("\tContact: {0}, Location: {1}-{2}", c.ContactName, c.City, c.Country);
            });
            Console.WriteLine();
        }

        static void TestGetCustomersById()
        {
            try
            {

                CustomerDataAccess cda = new CustomerDataAccess();
                string Id = Console.ReadLine();
                var list = cda.GetCustomersById(Id);
                if (list is null)
                {
                    Console.WriteLine("Nothing Found");
                }
                else
                {
                    Console.WriteLine("ID: {0}, Company: {1}", list.CustomerId, list.CompanyName);
                    Console.WriteLine("\tContact: {0}, Location: {1}-{2}", list.ContactName, list.City, list.Country);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

                
          
            Console.WriteLine();
        }

        static void InsertIntoCustomers()
        {
            CustomerDataAccess cda = new CustomerDataAccess();
        
            Func<string, string> GetInput = (text) =>
            {
                Console.WriteLine($"Enter {text}:");
                var str = Console.ReadLine();
                return str;
            };
            /*Customer cust = new Customer
            {
                CustomerId="ABCDE",
                CompanyName="ADBCDE",
                ContactName="ABDCDE",
                City="ADBCCC",
                Country="asrfll"
            };*/ // --one way
            Customer cust = new Customer
            {
                CustomerId = GetInput("Customer Id"),
                CompanyName = GetInput("Company Name"),
                ContactName = GetInput("Contact Name"),
                City = GetInput("City"),
                Country = GetInput("Country")
            };
            cda.InsertIntoCustomers(cust);//second-wy
        }

        static void UpdateCustomers()
        {
            CustomerDataAccess cda = new CustomerDataAccess();

            Func<string, string> GetInput = (text) =>
            {
                Console.WriteLine($"Enter {text}:");
                var str = Console.ReadLine();
                return str;
            };
            /*Customer cust = new Customer
            {
                CustomerId="ABCDE",
                CompanyName="ADBCDE",
                ContactName="ABDCDE",
                City="ADBCCC",
                Country="asrfll"
            };*/ // --one way
            Customer cust = new Customer
            {
                CustomerId = GetInput("Customer Id"),
                CompanyName = GetInput("Company Name"),
                ContactName = GetInput("Contact Name"),
                City = GetInput("City"),
                Country = GetInput("Country")
            };
            cda.UpadteNewCustomer(cust);//second-wy
        }
    }
}

