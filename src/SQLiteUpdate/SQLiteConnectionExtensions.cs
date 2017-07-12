using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteUpdate
{

    internal static class SQLiteConnectionExtensions
    {

        public static object QueryScalar(this SQLiteConnection @this, string statement)
        {
            return @this.QueryScalar(statement, null);
        }

        public static object QueryScalar(this SQLiteConnection @this, string statement, object parameters)
        {
            using (var cm = @this.CreateCommand(statement, parameters))
            {
                return cm.ExecuteScalar();
            }
        }

        public static int NonQuery(this SQLiteConnection @this, string statement)
        {
            return @this.NonQuery(statement, null);
        }

        public static int NonQuery(this SQLiteConnection @this, string statement, object parameters)
        {
            using (var cm = @this.CreateCommand(statement, parameters))
            {
                return cm.ExecuteNonQuery();
            }
        }

        public static SQLiteCommand CreateCommand(this SQLiteConnection @this, string statement, object parameters)
        {

            if (@this.State != System.Data.ConnectionState.Open) @this.Open();

            var cm = @this.CreateCommand();
            cm.CommandText = statement;

            if(parameters != null)
            {
                foreach(var prop in parameters.GetType().GetProperties().Where(p => p.CanRead))
                {
                    cm.Parameters.AddWithValue(prop.Name, prop.GetValue(parameters));
                }
            }

            return cm;

        }

    }

}
