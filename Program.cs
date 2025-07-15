
using Service_Record.BAL.Interfaces;
using Service_Record.BAL.Services;
using Service_Record.DAL.Interfaces;
using Service_Record.DAL.Repositories;
using Service_Record.Extensions;




//configuration

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPersistence(builder.Configuration);


// Add services to the container.

// === AutoMapper Configuration ===
// This registers AutoMapper and scans your project for any mapping profiles you've created.
builder.Services.AddAutoMapper(typeof(Program));


// === Repository and Service Layer Dependencies ===
// The following lines would have been added automatically by your ark.py script.

//Repositories
builder.Services.AddScoped<IBranchRepo, BranchRepo>();
builder.Services.AddScoped<IPartRepo, PartRepo>();
builder.Services.AddScoped<IQuotationRepo, QuotationRepo>();
builder.Services.AddScoped<IServicerecordRepo, ServicerecordRepo>();
builder.Services.AddScoped<IServicerecordpartRepo, ServicerecordpartRepo>();
builder.Services.AddScoped<IUserlogRepo, UserlogRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

//Services
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();
builder.Services.AddScoped<IServicerecordService, ServicerecordService>();
builder.Services.AddScoped<IServicerecordpartService, ServicerecordpartService>();
builder.Services.AddScoped<IUserlogService, UserlogService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllers();
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
