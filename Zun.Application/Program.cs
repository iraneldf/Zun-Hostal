using Microsoft.EntityFrameworkCore;
using Zun.Aplicacion.IoC;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using log4net.Config;

//Se configuran los log del sistema con la librería Log4Net
XmlConfigurator.Configure(new FileInfo("log4net.config"));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddRegistration(configuration);
IoCRegister.AddLogsRegistration(builder);

var app = builder.Build();

IoCRegister.AddRegistration(app, app.Environment);




