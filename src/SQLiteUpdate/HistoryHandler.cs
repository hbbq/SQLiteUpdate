﻿using System.Data.SQLite;

namespace SQLiteUpdate
{

    internal class HistoryHandler
    {

        private const string Tablename = @"SQLiteUpdateHistory";

        private const string Tabledefinition = @"
            create table SQLiteUpdateHistory (
                Identity text not null primary key,
                Script text not null,
                Time datetime not null default current_timestamp
            )
            ";

        private SQLiteConnection Connection { get; }

        public HistoryHandler(SQLiteConnection connection)
        {
            Connection = connection;
        }

        public void Init()
        {

            var exists =
                Connection.QueryScalar(@"
                    select count(*)
                    from sqlite_master 
                    where type = @type
                        and name = @name
                    ",
                    new {
                        type = "table",
                        name = Tablename
                    }
                ).ToString() != "0";

            if(!exists) Connection.NonQuery(Tabledefinition);

        }

        public bool HasScriptExecuted(string identity)
        {
            return
                Connection.QueryScalar($@"
                    select count(*)
                    from {Tablename}
                    where Identity = @identity
                    ",
                    new { identity = identity }
                ).ToString() != "0";
        }

        public void LogScriptExecution(UpdateScript script)
        {
            LogScriptExecution(script.Identity, script.Script);
        }

        public void LogScriptExecution(string identity, string script)
        {
            Connection.NonQuery($@"
                insert into {Tablename} (
                    Identity,
                    Script
                ) values (
                    @identity,
                    @script
                )
                ",
                new
                {
                    identity = identity,
                    script = script,
                }
            );
        }

    }

}
