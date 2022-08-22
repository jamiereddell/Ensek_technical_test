using ReddellJ.UsageImporter.Data;
using ReddellJ.UsageImporter.Domain;
using ReddellJ.UsageImporter.Domain.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IFileSystem, FileSystemAdapter>();
builder.Services.AddTransient<ICsvFileReader, CsvFileReader>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IUsageRepository, UsageRepository>();
builder.Services.AddTransient<IUsageFileRepository, UsageFileRepository>();
builder.Services.AddTransient<IMeterReadingContext, MeterReadingContext>((serviceProvider) =>
{
    var config = serviceProvider.GetService<IConfiguration>();
    return new MeterReadingContext(config.GetConnectionString("MeterReading"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
