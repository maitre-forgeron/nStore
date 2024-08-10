using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace NStore.Logging;

public static class SeriLogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
       (context, configuration) =>
       {
           var elasticUri = context.Configuration["ElasticConfiguration:Uri"];
           
           configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch([new Uri(elasticUri)], options =>
                {
                    options.DataStream = new Elastic.Ingest.Elasticsearch.DataStreams.DataStreamName("logs", $"{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}");
                })
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .ReadFrom.Configuration(context.Configuration);
       };
}
