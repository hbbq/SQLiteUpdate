using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

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

            var history = new HistoryHandler(connection);
            history.Init();

            var scriptsToRun = scripts.Where(s => !history.HasScriptExecuted(s.Identity));

            foreach(var script in scriptsToRun)
            {
                connection.NonQuery(script.Script);
                history.LogScriptExecution(script);
                result.ExecutedScripts.Add(script);
            }

            result.Successful = true;

            return result;

        }

        public static UpdateResult UpdateFromResources(SQLiteConnection connection, Assembly resourceAssembly, string containingNamespace)
        {

            var scripts = resourceAssembly.GetManifestResourceNames()
                .Where(n => n.StartsWith(containingNamespace + "."))
                .Select(name =>
                    {
                        using (var stream = resourceAssembly.GetManifestResourceStream(name))
                        using (var reader = new System.IO.StreamReader(stream))
                        return new UpdateScript
                        {
                            Identity = name.Substring(containingNamespace.Length + 1),
                            Script = reader.ReadToEnd()
                        };                        
                    }
                ).ToList();

            return UpdateFromScripts(connection, scripts);

        }

    }

}
