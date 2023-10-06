using Serilog;
using Serilog.Sinks.MSSqlServer;

using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

using (SqlConnection connection = new SqlConnection(config.GetConnectionString("DockerMSSqlServer")))
{
    try
    {
        connection.Open();
        Console.WriteLine("Conexão bem-sucedida!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
    }
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        connectionString : config.GetConnectionString("DockerMSSqlServer"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true,
        })
    .CreateLogger();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
