using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommel.Linq.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var con = new SqlConnection("Data Source=.\\sql2012; Initial Catalog=DapperTest;Integrated Security=True"))
            {

                var q = con.Table<Product>();

                var x = q.Where(p => p.Name == "test");
                var y = x.ToList();
            }
            
        }
    }

    public class Product
    {
        public string Name { get; set; }
    }
}
