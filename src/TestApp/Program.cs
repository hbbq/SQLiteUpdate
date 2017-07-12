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

            using(var cn = new SQLiteConnection($@"Data source={filename}"))
            {

                SQLiteUpdate.Db.UpdateFromResources(cn, System.Reflection.Assembly.GetExecutingAssembly(), "TestApp.DbScripts.Test");

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
