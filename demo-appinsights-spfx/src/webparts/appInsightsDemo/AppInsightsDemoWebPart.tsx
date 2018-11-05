import { Version } from "@microsoft/sp-core-library";
import { BaseClientSideWebPart, IPropertyPaneConfiguration } from "@microsoft/sp-webpart-base";
import { AppInsights } from "applicationinsights-js";
import * as React from "react";
import * as ReactDom from "react-dom";
import { initializeApplicationInsights } from "../../logging/applicationInsights";
import AppInsightsDemo from "./components/AppInsightsDemo";

export interface IAppInsightsDemoWebPartProps {
  instrumentationKey: string;
  webApiUrl: string;
}

export default class AppInsightsDemoWebPart extends BaseClientSideWebPart<IAppInsightsDemoWebPartProps> {
  public async onInit(): Promise<void> {
    initializeApplicationInsights(this.properties.instrumentationKey, this.context);

    AppInsights.trackPageView();
  }

  public render(): void {
    ReactDom.render(<AppInsightsDemo webApiUrl={this.properties.webApiUrl} />, this.domElement);
  }

  protected onDispose(): void {
    ReactDom.unmountComponentAtNode(this.domElement);
  }

  protected get dataVersion(): Version {
    return Version.parse("1.0");
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return {
      pages: []
    };
  }
}
