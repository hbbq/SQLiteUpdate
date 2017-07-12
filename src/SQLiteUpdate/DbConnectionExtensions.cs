using System.Data.Common;
using System.Linq;

namespace SQLiteUpdate
{

    internal static class DbConnectionExtensions
    {

        public static object QueryScalar(this DbConnection @this, string statement)
        {
            return @this.QueryScalar(statement, null);
        }

        public static object QueryScalar(this DbConnection @this, string statement, object parameters)
        {
            using (var cm = @this.CreateCommand(statement, parameters))
            {
                return cm.ExecuteScalar();
            }
        }

        public static int NonQuery(this DbConnection @this, string statement)
        {
            return @this.NonQuery(statement, null);
        }

        public static int NonQuery(this DbConnection @this, string statement, object parameters)
        {
            using (var cm = @this.CreateCommand(statement, parameters))
            {
                return cm.ExecuteNonQuery();
            }
        }

        public static DbCommand CreateCommand(this DbConnection @this, string statement, object parameters)
        {

            if (@this.State != System.Data.ConnectionState.Open) @this.Open();

            var cm = @this.CreateCommand();
            cm.CommandText = statement;

            if(parameters != null)
            {
                foreach(var prop in parameters.GetType().GetProperties().Where(p => p.CanRead))
                {
                    var parameter = cm.CreateParameter();
                    parameter.ParameterName = prop.Name;
                    parameter.Value = prop.GetValue(parameters);
                    cm.Parameters.Add(parameter);
                }
            }

            return cm;

        }

    }

}
