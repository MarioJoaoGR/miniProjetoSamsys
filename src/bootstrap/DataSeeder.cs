
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



        // Create authors
        var authors = new List<Author>
    {
        new Author("John Doe", "501964843"),
        new Author("Jane Smith", "123456789"),
        new Author("Alice Johnson", "245083600")
    };

        // Add authors to the context
        context.Authors.AddRange(authors);

        // Create books
        var books = new List<Book>
    {
        new Book("978-3-16-148410-0", "Book One", authors[0].Id.AsString(), "20,00"),
        new Book("978-1-4028-9462-6", "Book Two", authors[1].Id.AsString(), "25,00"),
        new Book("978-0-596-52068-7", "Book Three", authors[2].Id.AsString(), "30,00")
    };

        // Add books to the context
        context.Books.AddRange(books);

        // Save the changes to the database
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
