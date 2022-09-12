using CatSimpleVer.Extensions;
using CatSimpleVer.Extensions.ServiceSetup;
using Autofac;


var builder = WebApplication.CreateBuilder(args);

//1.����host������




//2.���÷���ע��
builder.Services.AddSqlsugarSetup();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//3.�����м��
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();

//4.����
app.Run();
