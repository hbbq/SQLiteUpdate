using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{

    class Program
    {

        static void Main(string[] args)
        {
            
            var filename = "test.sqlite";

            SQLiteConnection.CreateFile(filename);

            //var scripts = new List<SQLiteUpdate.UpdateScript>
            //{
            //    new SQLiteUpdate.UpdateScript
            //    {
            //        Identity = "00001",
            //        Script = "create table a (a int, b text)",
            //    },
            //    new SQLiteUpdate.UpdateScript
            //    {
            //        Identity = "00002",
            //        Script = "insert into a (a, b) values (1, 'a')",
            //    }
            //};

            using(var cn = new SQLiteConnection($@"Data source={filename}"))
            {
                SQLiteUpdate.Db.UpdateFromResources(cn, System.Reflection.Assembly.GetExecutingAssembly(), "TestApp.DbScripts");
            //    SQLiteUpdate.Db.UpdateFromScripts(cn, scripts);
                using (var da = new SQLiteDataAdapter("select * from test", cn))                
                using (var ds = new DataSet())
                {
                    da.Fill(ds);
                    Console.WriteLine(ds.GetXml());
                }
            }

            Console.ReadLine();

        }

    }

}
