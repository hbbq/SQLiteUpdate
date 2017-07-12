using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
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

        public static UpdateResult UpdateFromResources(SQLiteConnection connection, Assembly resourceAssembly, string containingNamespace)
        {

            var names =
                resourceAssembly.GetManifestResourceNames()
                .Where(r => r.StartsWith(containingNamespace + "."));

            var scripts = new List<UpdateScript>();

            foreach(var name in names)
            {
                using (var stream = resourceAssembly.GetManifestResourceStream(name))
                using (var reader = new System.IO.StreamReader(stream))
                {
                    var identity = name.Substring(containingNamespace.Length + 1);
                    var script = reader.ReadToEnd();
                    scripts.Add(new UpdateScript { Identity = identity, Script = script });
                }
            }

            return UpdateFromScripts(connection, scripts);

        }

    }

}
