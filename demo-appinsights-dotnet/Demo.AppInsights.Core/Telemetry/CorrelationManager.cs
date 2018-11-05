using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Demo.AppInsights.Core.Telemetry
{
    /// <summary>
    /// Provides a thread-safe way of storing and retrieving the current OperationId.
    /// </summary>
    public static class CorrelationManager
    {
        private const string OperationIdKey = "OperationId";

        public static void SetOperationId(string operationId)
        {
            CallContext.SetData(OperationIdKey, operationId);
        }

        public static string GetOperationId()
        {
            var id = CallContext.GetData(OperationIdKey) as string;
            if (null == id)
            {
                id = Guid.NewGuid().ToString("N");
                CallContext.SetData(OperationIdKey, id);
            }

            return id;
        }
    }

    /// <summary>
    /// Provides a way to set contextual data that flows with the call and 
    /// async context of a test or invocation.
    /// </summary>
    internal static class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state =
            new ConcurrentDictionary<string, AsyncLocal<object>>();

        /// <summary>
        /// Stores a given object and associates it with the specified name.
        /// </summary>
        /// <param name="name">The name with which to associate the new item in the call context.</param>
        /// <param name="data">The object to store in the call context.</param>
        public static void SetData(string name, object data) =>
            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;

        /// <summary>
        /// Retrieves an object with the specified name from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="name">The name of the item in the call context.</param>
        /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
        public static object GetData(string name) =>
            state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
    }
}