using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using asp_interpreter_lib.Visitors;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using asp_interpreter_exe;
using System;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<ProgramVisitor>();
builder.Services.AddSingleton(new PrefixOptions(
    "rwh_", 
    "fa_",
    "eh",
    "chk_",
    "not_",
    "V"));

builder.Services.AddTransient<Application>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<Logger<Application>>();


var conf = GetConfig(args);

if (conf.Help)
{
    DisplayHelp();
    return; 
}

builder.Services.AddSingleton(conf);
var host = builder.Build();
host.Services.GetRequiredService<Application>().Run();

ProgramConfig GetConfig(string[] args)
{
    var switchMappings = new Dictionary<string, string>()
           {
               { "-l", "log-level" },
               { "-p", "path" },
               { "-h", "help" },
               { "--log-level", "log-level" },
               { "--path", "path" },
               { "--help", "help" }
           };

    builder.Configuration.AddCommandLine(args, switchMappings);

    bool help = builder.Configuration["help"] != null;

    string path = builder.Configuration["path"];

    int logLevel;

    if (!int.TryParse(builder.Configuration["log-level"] ?? "3", out logLevel)
        || logLevel < 0 || logLevel > 4)
    {
        Console.WriteLine($"The specified log level {builder.Configuration["log-level"]} " +
            $"was not valid therefore 3 has been set as a defaul value!");
    }

    if(string.IsNullOrEmpty(path))
    {
        Console.WriteLine("Please provide a valid Path!");
        return new ProgramConfig(" ", true);
    }

    return new ProgramConfig(path, help, logLevel);
}

void DisplayHelp()
{
    Console.WriteLine("How to use: ");
}