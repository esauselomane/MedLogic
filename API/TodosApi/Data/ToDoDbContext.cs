using System;
using Microsoft.EntityFrameworkCore;
using TodosApi.Models;

namespace TodosApi.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> TodoItems { get; set; }
    }
}
