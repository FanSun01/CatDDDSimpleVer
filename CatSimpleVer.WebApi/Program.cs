using CatSimpleVer.Extensions;
using CatSimpleVer.Extensions.ServiceSetup;
using Autofac;


var builder = WebApplication.CreateBuilder(args);

//1.配置host与容器




//2.配置服务注入
builder.Services.AddSqlsugarSetup();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//3.配置中间件
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();

//4.运行
app.Run();
