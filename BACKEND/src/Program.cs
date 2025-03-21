﻿using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DDDSample1.Infrastructure.Shared;
using Microsoft.Extensions.Hosting;
using DDDNetCore.Domain.Books;
using DDDNetCore.Domain.Authors;
using DDDNetCore.Infraestructure.Books;
using DDDNetCore.Infraestructure.Authors;


namespace DDDSample1.Startup
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração Swagger para API (sem segurança, pois não vamos usar autenticação/autorizações)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Adicionar CORS para permitir chamadas do frontend React
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000") // 🔥 Permite chamadas do frontend
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            // Adicionar o contexto de banco de dados
            var useInMemoryDatabase = builder.Configuration.GetValue<bool>("DatabaseSettings:UseInMemoryDatabase");
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<DDDSample1DbContext>(opt =>
            {
                if (useInMemoryDatabase)
                {
                    opt.UseInMemoryDatabase("DDDSample1DB");
                }
                else
                {
                    opt.UseSqlServer(connectionString);
                }

                opt.ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>();
            });

            // Adicionar controladores e suporte a JSON
            builder.Services.AddControllers().AddNewtonsoftJson();

            // Adicionar repositórios e serviços
            ConfigureMyServices(builder.Services);

            var app = builder.Build();

            // Seed de dados (se necessário)
            await DataSeeder.SeedAsync(app.Services);

            // Configuração do pipeline de requisições
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowReactApp");
            app.UseMiddleware<CustomExceptionMiddleware>();

            app.MapControllers();
            app.Run();
        }

        // Configuração de serviços
        static void ConfigureMyServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IBookService, BookService>();

            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IAuthorService, AuthorService>();
        }
    }
}