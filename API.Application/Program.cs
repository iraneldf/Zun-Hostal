using API.Application.IoC;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
ConfigurationManager configuration = builder.Configuration;


builder.Services.AddRegistration(configuration); ;
IoCRegister.AddLogsRegistration(builder);

var app = builder.Build();


IoCRegister.AddRegistration(app, app.Environment);




