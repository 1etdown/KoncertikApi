using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KoncApi;
public class User
{
     [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public int PhoneNumber { get; set; }
    public required string Email { get; set; }
}
