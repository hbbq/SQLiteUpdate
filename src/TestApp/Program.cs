using System;
using System.Data;
using System.Data.SQLite;

namespace TestApp
{

    class Program
    {

        static void Main()
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
