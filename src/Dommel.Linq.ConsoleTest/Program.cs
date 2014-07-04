using System;
using System.Data.SqlClient;
using System.Linq;
using Dommel.Linq.Utils;

namespace Dommel.Linq.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var con = new SqlConnection("Data Source=sql2012; Initial Catalog=DapperTest;Integrated Security=True"))
            {
                Profiler.Profile("Open connection", () => con.Open());

                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("Iteration: {0}", i);

                    using (new Profiler("Query execute total"))
                    {
                        IQueryable<Product> q;
                        using (new Profiler("Create IQueryable<Product>"))
                        {
                            q = con.Table<Product>().Where(p => p.Name != "bla").Take(10);
                        }

                        using (new Profiler("Materialize Query"))
                        {
                            var y = q.ToList();
                        }

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
        public int Id { get; set; }

        public string Name { get; set; }

        public string NameUrlOptimized { get; set; }

        public string Description { get; set; }
    }
}
