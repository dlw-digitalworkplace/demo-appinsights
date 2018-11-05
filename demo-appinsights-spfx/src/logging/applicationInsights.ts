import { WebPartContext } from "@microsoft/sp-webpart-base";
import { initAppInsightsFetchMonitor } from "application-insights-fetch-monitor";
import { AppInsights } from "applicationinsights-js";

export function initializeApplicationInsights(instrumentationKey: string, context: WebPartContext) {
  // initialize application insights
  AppInsights.downloadAndSetup({
    instrumentationKey: instrumentationKey,
    enableCorsCorrelation: true,
    disableCorrelationHeaders: false,
    // exclude domains which are used by SharePoint but do not allow custom headers in their cors policy
    correlationHeaderExcludedDomains: [
      "*.azureedge.net",
      "*.msedge.net",
      "*.office.com",
      "*.office365.com",
      "*.microsoft.com",
      "*.bingapis.com"
    ]
  });

  // add fetch monitor
  initAppInsightsFetchMonitor();

  // add authenticated user information
  AppInsights.setAuthenticatedUserContext(context.pageContext.user.loginName);

  // add additional telementry
  AppInsights.queue.push(() => {
    AppInsights.context.addTelemetryInitializer((envelope: Microsoft.ApplicationInsights.IEnvelope) => {
      const excludedTargets = ["browser.pipe.aria.microsoft.com"];

      try {
        if (envelope.name === Microsoft.ApplicationInsights.Telemetry.RemoteDependencyData.envelopeType) {
          // ignore excluded domain as dependency to prevent irrelevant data in AppInsights
          if (excludedTargets.indexOf(envelope.data.baseData.target) > -1) return false;
        }
      } catch (err) {
        // swallow error
      }

      envelope.tags["ai.cloud.role"] = "Demo.AppInsights.SPFx";
    });
  });
}
