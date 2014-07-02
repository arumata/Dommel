using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;

namespace Dommel.Linq.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var con = new SqlConnection("Data Source=sql2012; Initial Catalog=DapperTest;Integrated Security=True"))
            {
                for (int i = 0; i < 10; i++)
                {
                    var sw = Stopwatch.StartNew();

                    var q = con.Table<Product>();
                    q = q.Where(p => p.Name != "bla");
                    
                    var y = q.ToList();

                    //var y = con.Query<Product>("select * from Products where Name != 'bla'").ToList();

                    sw.Stop();
                    Console.WriteLine("Retrieved {0} objects in {1}ms", y.Count, sw.Elapsed.TotalMilliseconds);
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
