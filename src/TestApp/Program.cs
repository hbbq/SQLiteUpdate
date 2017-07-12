using System;
using System.Collections.Generic;
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
                SQLiteUpdate.Db.UpdateFromScripts(cn, null);
            }

        }

    }

}
