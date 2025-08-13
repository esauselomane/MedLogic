using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TodosApi.Data;
using TodosApi.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly TodoDbContext _context;

    public TodoController(TodoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_context.TodoItems.ToList());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var item = _context.TodoItems.Find(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public IActionResult Create(Todo item)
    {
        _context.TodoItems.Add(item);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Todo updated)
    {
        var item = _context.TodoItems.Find(id);
        if (item == null) return NotFound();

        item.Title = updated.Title;
        item.Description = updated.Description;
        item.IsCompleted = updated.IsCompleted;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] bool isCompleted)
    {
        var item = _context.TodoItems.Find(id);
        if (item == null) return NotFound();

        item.IsCompleted = isCompleted;
        _context.SaveChanges();

        return NoContent();
    }
}
