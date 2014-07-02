using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Dommel.Linq.Utils;

namespace Dommel.Linq.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var con = new SqlConnection("Data Source=sql2012; Initial Catalog=DapperTest;Integrated Security=True"))
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("Iteration: {0}", i);

                    using (new Profiler("Query execute"))
                    {
                        var q = from p in con.Table<Product>()
                                where p.Name != "bla"
                                select p;
                        var y = q.ToList();
                        //var y = con.Query<Product>("select * from Products where Name != 'bla'").ToList();
                    }

                    Console.WriteLine("");
                }

                Console.ReadKey();
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
    }
}
