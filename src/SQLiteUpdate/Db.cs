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

            if (scripts == null) return new UpdateResult { Successful = false };

            var result = new UpdateResult
            {
                Successful = false,
                ExecutedScripts = new List<UpdateScript>()
            };

            var historyHandler = new HistoryHandler(connection);
            historyHandler.Init();

            var scriptsToRun = scripts.Where(s => !historyHandler.HasScriptExecuted(s.Identity));

            foreach(var script in scriptsToRun)
            {

                connection.NonQuery(script.Script);

                historyHandler.LogScriptExecution(script);

                result.ExecutedScripts.Add(script);

            }

            result.Successful = true;

            return result;

        }

    }

}
