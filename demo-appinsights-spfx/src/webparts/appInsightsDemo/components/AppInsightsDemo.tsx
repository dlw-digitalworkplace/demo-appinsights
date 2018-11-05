import { trimEnd } from "@microsoft/sp-lodash-subset";
import { AppInsights } from "applicationinsights-js";
import { DefaultButton } from "office-ui-fabric-react/lib/Button";
import * as React from "react";
import styles from "./AppInsightsDemo.module.scss";
import { IAppInsightsDemoProps } from "./IAppInsightsDemoProps";

export default class AppInsightsDemo extends React.Component<IAppInsightsDemoProps> {
  constructor(props: IAppInsightsDemoProps) {
    super(props);

    this._getWebApi = this._getWebApi.bind(this);
    this._postWebApi = this._postWebApi.bind(this);
    this._generateCaughtException = this._generateCaughtException.bind(this);
    this._generateUncaughtException = this._generateUncaughtException.bind(this);
    this._generateServerException = this._generateServerException.bind(this);
  }

  public render(): React.ReactElement<IAppInsightsDemoProps> {
    return (
      <div className={styles.appInsightsDemo}>
        <DefaultButton text="GET Web Api" onClick={this._getWebApi} />{" "}
        <DefaultButton text="POST Web Api" onClick={this._postWebApi} />{" "}
        <DefaultButton text="Generate Caught Exception" onClick={this._generateCaughtException} />{" "}
        <DefaultButton text="Generate Uncaught Exception" onClick={this._generateUncaughtException} />
        <DefaultButton text="Generate Server Exception" onClick={this._generateServerException} />
      </div>
    );
  }

  private async _getWebApi(): Promise<void> {
    var t0 = performance.now();

    await fetch(`${trimEnd(this.props.webApiUrl, "/")}/api/values`, {
      method: "GET",
      headers: {
        Accept: "application/json",
        "Content-Type": "text/json"
      }
    });

    var t1 = performance.now();
    AppInsights.trackEvent("AppInsightsDemo._getWebApi", {}, { ElapsedMilliseconds: t1 - t0 });
  }

  private async _postWebApi(): Promise<void> {
    var t0 = performance.now();

    await fetch(`${trimEnd(this.props.webApiUrl, "/")}/api/values`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "text/json"
      },
      body: JSON.stringify("Hello SPFx")
    });

    var t1 = performance.now();
    AppInsights.trackEvent("AppInsightsDemo._postWebApi", {}, { ElapsedMilliseconds: t1 - t0 });
  }

  private _generateCaughtException(): void {
    try {
      document.getElementById("NotReal").appendChild(null);
    } catch (err) {
      AppInsights.trackException(err);
    }
  }

  private _generateUncaughtException(): void {
    document.getElementById("NotReal").appendChild(null);
  }

  private async _generateServerException(): Promise<void> {
    var t0 = performance.now();

    await fetch(`${trimEnd(this.props.webApiUrl, "/")}/api/values?value=demo`, {
      method: "DELETE",
      headers: {
        Accept: "application/json",
        "Content-Type": "text/json"
      }
    });

    var t1 = performance.now();
    AppInsights.trackEvent("AppInsightsDemo._generateServerException", {}, { ElapsedMilliseconds: t1 - t0 });
  }
}
