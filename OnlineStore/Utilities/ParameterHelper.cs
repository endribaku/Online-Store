using System.Data;
using System.Data.Common;

namespace OnlineStore.Utilities
{
    public static class ParameterHelper
    {
        public static void AddParameter(this DbCommand command, string name, object value)
        {
            DbParameter param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }
    }
}

