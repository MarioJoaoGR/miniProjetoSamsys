
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using DDDNetCore.Domain.Authors;
using DDDNetCore.Domain.Books;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DDDSample1DbContext>();

        // Criar autores adicionais
        var authors = new List<Author>
    {
        new Author("John Doe", "501964843"),
        new Author("Jane Smith", "123456789"),
        new Author("Alice Johnson", "245083600"),
        new Author("Robert Brown", "808321234"),
        new Author("Emily White", "543210987"),
        new Author("Michael Green", "210310944")
    };

        // Adicionar autores ao contexto
        context.Authors.AddRange(authors);

        // Criar livros adicionais
        var books = new List<Book>
    {
        new Book("978-3-16-148410-0", "Book One", authors[0].Id.AsString(), "20,00"),
        new Book("978-1-4028-9462-6", "Book Two", authors[1].Id.AsString(), "25,00"),
        new Book("978-0-596-52068-7", "Book Three", authors[2].Id.AsString(), "30,00"),
        new Book("978-0-321-48681-3", "Advanced Programming", authors[3].Id.AsString(), "40,00"),
        new Book("978-1-86197-876-9", "Data Science Basics", authors[4].Id.AsString(), "35,00"),
        new Book("978-0-262-03384-8", "Machine Learning Essentials", authors[5].Id.AsString(), "50,00"),
        new Book("978-0-12-374856-0", "Artificial Intelligence Principles", authors[3].Id.AsString(), "55,00"),
        new Book("978-1-59327-599-0", "Cybersecurity Fundamentals", authors[4].Id.AsString(), "45,00")
    };

        // Adicionar livros ao contexto
        context.Books.AddRange(books);

        // Salvar as altera��es no banco de dados
        await context.SaveChangesAsync();
    }


    public static async Task UnseedAsync(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DDDSample1DbContext>();


    // Remove books
    var books = context.Books.ToList();
    context.Books.RemoveRange(books);

  

    // Remove authors
    var authors = context.Authors.ToList();
    context.Authors.RemoveRange(authors);

 
    // Save the changes to the database
    await context.SaveChangesAsync();
  }


}
