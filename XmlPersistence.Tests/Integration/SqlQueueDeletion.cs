using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

static class SqlQueueDeletion
{
    public static async Task DeleteQueue(SqlConnection connection, string schema, string queueName)
    {
        var sql = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U'))
                    DROP TABLE [{0}].[{1}]";
        var deleteScript = string.Format(sql, schema, queueName);
        using (var command = new SqlCommand(deleteScript, connection))
        {
            await command.ExecuteNonQueryAsync();
        }
    }

    public static async Task DeleteQueuesForEndpoint(SqlConnection connection, string schema, string endpointName)
    {
        //main queue
        await DeleteQueue(connection, schema, endpointName);

        //callback queue
        await DeleteQueue(connection, schema, endpointName + "." + Environment.MachineName);

        //retries queue
        await DeleteQueue(connection, schema, endpointName + ".Retries");

        //timeout queue
        await DeleteQueue(connection, schema, endpointName + ".Timeouts");

        //timeout dispatcher queue
        await DeleteQueue(connection, schema, endpointName + ".TimeoutsDispatcher");
    }

}