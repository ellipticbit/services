using System;
#if NETSTANDARD
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

namespace EllipticBit.Services.Scheduler
{
	public class SqlServerNetworkSynchronizationOptions
	{
		public SqlConnectionStringBuilder Connection { get; }
		public SqlServerNetworkSynchronizationMapping Mapping { get; }

		public SqlServerNetworkSynchronizationOptions(SqlConnectionStringBuilder connection, SqlServerNetworkSynchronizationMapping mapping) {
			Connection = connection ?? throw new ArgumentNullException(nameof(connection));
			Mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
		}
	}

	public class SqlServerNetworkSynchronizationMapping
	{
		public string Schema { get; }

		public string ActionTable { get; }
		public string ActionIdColumn { get; }
		public string ActionNameColumn { get; }
		public string ActionEnabledColumn { get; }
		public string ActionNextColumn { get; }

		public SqlServerNetworkSynchronizationMapping(string schema, string actionTable, string actionIdColumn, string actionNameColumn, string actionEnabledColumn, string actionNextColumn)
		{
			if (string.IsNullOrWhiteSpace(schema) || string.IsNullOrWhiteSpace(actionTable) || string.IsNullOrWhiteSpace(actionIdColumn) || string.IsNullOrWhiteSpace(actionNameColumn) || string.IsNullOrWhiteSpace(actionEnabledColumn) || string.IsNullOrWhiteSpace(actionNextColumn))
				throw new ArgumentNullException(nameof(schema), "Please provides non-null values for all parameters.");

			Schema = schema;
			ActionTable = actionTable;
			ActionIdColumn = actionIdColumn;
			ActionNameColumn = actionNameColumn;
			ActionEnabledColumn = actionEnabledColumn;
			ActionNextColumn = actionNextColumn;
		}
	}
}
