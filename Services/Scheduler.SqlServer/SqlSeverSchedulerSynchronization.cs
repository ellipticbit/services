using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
#if NETSTANDARD
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

namespace EllipticBit.Services.Scheduler
{
	internal class SqlSeverSchedulerSynchronization : ISchedulerSynchronizationContext
	{
		private readonly SqlServerNetworkSynchronizationOptions options;
		private readonly ILogger logger;

		public SqlSeverSchedulerSynchronization(SqlServerNetworkSynchronizationOptions options, ILogger<SqlSeverSchedulerSynchronization> logger) {
			this.options = options;
			this.logger = logger;
		}

		async Task<bool> ISchedulerSynchronizationContext.Acquire(ISchedulerAction action) {
			if (action.IntervalMode == SchedulerActionIntervalMode.Second) throw new NotSupportedException("SQL Server Synchronization does not support SchedulerActionIntervalMode.Second");

			var datestr = action.IntervalMode == SchedulerActionIntervalMode.Day ? $"DATEADD(mi,{Math.Round(action.IntervalOffset?.TotalMinutes ?? 0)},DATEADD(dd,{action.Interval},DATETIMEOFFSETFROMPARTS(YEAR(GETUTCDATE()),MONTH(GETUTCDATE()),DAY(GETUTCDATE()),0,0,0,0,0,0,7)))" :
				action.IntervalMode == SchedulerActionIntervalMode.Hour ? $"DATEADD(mi,{Math.Round(action.IntervalOffset?.TotalMinutes ?? 0)},DATEADD(hh,{action.Interval},DATETIMEOFFSETFROMPARTS(YEAR(GETUTCDATE()),MONTH(GETUTCDATE()),DAY(GETUTCDATE()),DATEPART(hh, GETUTCDATE()),0,0,0,0,0,7)))" :
				$"DATEADD(mi,{action.Interval},DATETIMEOFFSETFROMPARTS(YEAR(GETUTCDATE()),MONTH(GETUTCDATE()),DAY(GETUTCDATE()),DATEPART(hh, GETUTCDATE()),DATEPART(mi, GETUTCDATE()),0,0,0,0,7))";
			var cmdstr = $"MERGE [{options.Mapping.Schema}].[{options.Mapping.ActionTable}] WITH(UPDLOCK, SERIALIZABLE) tgt" + Environment.NewLine +
			             $"USING (VALUES(@ActionId, @ActionName, {datestr})) AS src ([ActionId], [Name], [Next])" + Environment.NewLine +
			             $"ON tgt.[{options.Mapping.ActionIdColumn}] = @ActionId AND tgt.[{options.Mapping.ActionEnabledColumn}] <> 0 AND tgt.[{options.Mapping.ActionNextColumn}] < GETUTCDATE()" + Environment.NewLine +
			             $"WHEN MATCHED THEN UPDATE SET [{options.Mapping.ActionNameColumn}] = src.[Name], [{options.Mapping.ActionNextColumn}] = src.[Next]" + Environment.NewLine +
			             $"WHEN NOT MATCHED BY TARGET THEN INSERT ([{options.Mapping.ActionIdColumn}], [{options.Mapping.ActionNameColumn}], [{options.Mapping.ActionEnabledColumn}], [{options.Mapping.ActionNextColumn}]) VALUES (src.[ActionId], src.[Name], 1, src.[Next]);";
			using var sql = new SqlConnection(options.Connection.ToString());
			using var cmd = new SqlCommand(cmdstr, sql);
			cmd.Parameters.Add(new SqlParameter("ActionId", action.Id));
			cmd.Parameters.Add(new SqlParameter("ActionName", action.Name));
			logger.LogDebug("SqlSeverSchedulerSynchronization Acquire Query for Job '{Name}'" + Environment.NewLine + "{CommandString}", action.Name, cmdstr);

			bool locked = false;
			try {
				await sql.OpenAsync();
				locked = (await cmd.ExecuteNonQueryAsync()) != 0;
				if (locked)
				{
					logger.LogInformation("Scheduled job lock for '{Name}' acquired at: {CompletedAt}.", action.Name, DateTime.UtcNow.ToString("G"));
				}
				return locked;
			}
			catch (SqlException ex) {
				logger.LogDebug(ex, "Unable to acquire lock for Scheduled Job '{Name}' at: {CompletedAt}.", action.Name, DateTime.UtcNow.ToString("G"));
				return false;
			}
			finally {
				sql.Close();
			}
		}

		Task ISchedulerSynchronizationContext.Completed(ISchedulerAction action, DateTimeOffset startedAt, Exception ex = null) {
			if (ex == null) {
				logger.LogInformation("SQL Scheduled job '{Name}' started at {StartedAt} and completed at {CompletedAt}.", action.Name, startedAt.ToString("G"), DateTime.UtcNow.ToString("G"));
			}
			else {
				logger.LogError(ex, "SQL Scheduled job '{Name}' started at {StartedAt} and failed at {CompletedAt}.", action.Name, startedAt.ToString("G"), DateTime.UtcNow.ToString("G"));
			}

			return Task.CompletedTask;
		}
	}
}
