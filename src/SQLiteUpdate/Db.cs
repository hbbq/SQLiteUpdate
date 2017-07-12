using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUpdate
{

    public class Db
    {

        public static UpdateResult UpdateFromScripts(SQLiteConnection connection, IEnumerable<UpdateScript> scripts)
        {

            var historyHandler = new HistoryHandler(connection);
            historyHandler.Init();

            return null;

        }

    }

}
