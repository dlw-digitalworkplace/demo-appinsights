# Application Insights Demo Project

This project serves as a demo project for Application Insights in a .Net Core API and WebJob project.

The purpose of this demo is to set an example on how to use and integrate (Application Insights) logging in a project and preserving a **deep level of correlation** management spanning multiple parts of a solution.

Without a custom handler for `operationId` there would be no way to trace correlation between various components of the application - for example a Page View, Rest API calls and WebJob actions.

# Developer-specific project configuration

## .Net Back-End

- In `demo-appinsights-dotnet/Demo.AppInsights.API` create a new file `appsettings.Personal.json` with content:

```
{
  "ApplicationInsights": {
    "InstrumentationKey": "<YOUR_APPINSIGHTS_INSTRUMENTATION_KEY>"
  },

  "AzureStorage": {
    "QueueConnectionString": "<YOUR_AZURE_QUEUE_CONNECTION_STRING>",
  }
}
```

- In `demo-appinsights-dotnet/Demo.AppInsights.WebJob` create a new file `appsettings.Personal.json` with content:

```
{
  "ApplicationInsights": {
    "InstrumentationKey": "<YOUR_APPINSIGHTS_INSTRUMENTATION_KEY>"
  },

  "ConnectionStrings": {
    "AzureWebJobsStorage": "<YOUR_AZURE_QUEUE_CONNECTION_STRING>"
  }
}
```

## SPFx Front-End

- In `demo-appinsights-spfx/src/webparts/appInsightsDemo` edit the file `AppInsightsDemoWebPart.manifest.json` and update the settings under:

```
"preconfiguredEntries": [
  {
    "properties": {
      "instrumentationKey": "<INSTRUMENTATION_KEY>",
      "webApiUrl": "<WEB_API_URL>"
    }
  }
]
```

# Running the .Net Back-End

## Running from Visual Studio

Run the WebApi and WebJob projects (you can specify multiple startup projects by right-clicking the solution and clicking `Set StartUp Projects...`)

## Running from command line

- Execute `dotnet run -p .\demo-appinsights-dotnet\Demo.AppInsights.API` to run the .Net API
- Open a browser and go to https://localhost:5001/api/values

##

- Execute `dotnet run -p .\demo-appinsights-dotnet\Demo.AppInsights.WebJob` to run the WebJob
- Create a `POST` request to https://localhost:5001/api/values to create a message to be picked up by the Web Job. (include a string value as `POST body`)

# Running the SPFx Front-End

- Navigate to the SPFx project directory: `cd ./demo-appinsights-spfx`
- Execute `gulp serve` to run the project and open the browser
- Add the `Application Insights Demo` webpart to the page

# Logging guidelines

## Using LogLevels correctly

### `TRACE`

Used for showing logic-level program flow. Messages with `Trace` severity indicate _where_ in the code the application is at that current moment. For example: entering a function, leaving a function, `if`/`else` statement matched, ...

This log level is not expected to be enabled in production. If the logs _are_ enabled, this would be for a very short period only.

### `DEBUG`

Used for dumping variable state.

When, for example, a bug keeps occuring which is hard to reproduce, these dumped variables might help you debug the issue.

### `INFORMATION`

This is useful information that should be logged such as completing an action, creating data, ...

### `WARNING`

A warning indicates _something_ that is unexpected but can still be managed by your application and will not block execution of the current action.

For example: a service might be throttling incoming requests. This can be handled by your application but could still be worth logging.

### `ERROR`

An error indicates an unexpected issue which causes the application to stop execution.
These issues should be addressed as quickly as possible to ensure the functioning of the application.

### `CUSTOM EVENT`

A custom event is used to log actions or events in the application. To these events, a `metric` can be added and explored in Application Insights.

An example of such an event can be: `trackEvent("createNewItem", { title: "My new item", { elapsed: 217 })` which would log the title of a newly created item, along with the elapsed time for the API call.
