using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
IConfiguration configuration = builder.Configuration;

builder.Services.AddDatabase(configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureMappings();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCorsPolicy();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();