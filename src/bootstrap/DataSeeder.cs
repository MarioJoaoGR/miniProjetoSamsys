
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.StaffMembers;
using DDDSample1.Domain.Utils;
using DDDSample1.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

public static class DataSeeder
{
  public static async Task SeedAsync(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DDDSample1DbContext>();

    

    Staff staff = new Staff(new StaffId("D202512345"), "staff", "12345", specialization1.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff1 = new Staff(new StaffId("D202512346"), "staff", "12345", specialization1.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff2 = new Staff(new StaffId("D202512344"), "staff", "12345", specialization1.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff3 = new Staff(new StaffId("D202512340"), "staff", "12345", specialization1.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff4 = new Staff(new StaffId("D202512341"), "staff", "12345", specialization1.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff5 = new Staff(new StaffId("D202512310"), "staff", "12345", specialization2.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff6 = new Staff(new StaffId("D202512311"), "staff", "12345", specialization2.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff7 = new Staff(new StaffId("D202512312"), "staff", "12345", specialization2.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff8 = new Staff(new StaffId("D202512313"), "staff", "12345", specialization2.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff9 = new Staff(new StaffId("D202512314"), "staff", "12345", specialization4.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff10 = new Staff(new StaffId("D202512315"), "staff", "12345", specialization4.Id, "email@gmail.com", "+951999999999", "Doctor");
    Staff staff11 = new Staff(new StaffId("D202512316"), "staff", "12345", specialization4.Id, "email@gmail.com", "+951999999999", "Doctor");





    SeedStaff(context, staff);
    SeedStaff(context, staff1);
    SeedStaff(context, staff2);
    SeedStaff(context, staff3);
    SeedStaff(context, staff4);
    SeedStaff(context, staff5);
    SeedStaff(context, staff6);
    SeedStaff(context, staff7);
    SeedStaff(context, staff8);
    SeedStaff(context, staff9);
    SeedStaff(context, staff10);
    SeedStaff(context, staff11);

    context.SaveChanges();
  }


  private static void SeedPatients(DDDSample1DbContext context, Patient patient)
  {
    if (!context.Patients.Any())
    {
      context.Patients.Add(patient);
    }
  }

 

  public static void SeedStaff(DDDSample1DbContext context, Staff staff)
  {
    if (!context.Specializations.Any())
    {
      context.StaffMembers.Add(staff);
    }
  }

 



  public static async Task UnseedAsync(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DDDSample1DbContext>();


    // Remove staff
    var staffMembers = context.StaffMembers.ToList();
    context.StaffMembers.RemoveRange(staffMembers);

  

    // Remove patients
    var patients = context.Patients.ToList();
    context.Patients.RemoveRange(patients);

 
    // Save the changes to the database
    await context.SaveChangesAsync();
  }


}
