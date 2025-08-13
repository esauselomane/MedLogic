using System;
using Microsoft.EntityFrameworkCore;
using TodosApi.Models;

public class ToDoDbContext:DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Todo> TodoItems { get; set; }
}
