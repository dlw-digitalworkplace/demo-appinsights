using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Demo.AppInsights.Core.Telemetry
{
    /// <summary>
    /// Initializes the CloudRoleName property.
    /// </summary>
    public class CloudRoleNameInitializer : ITelemetryInitializer
    {
        private readonly string _roleName;

        public CloudRoleNameInitializer(string roleName)
        {
            this._roleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = this._roleName;
        }
    }
}
