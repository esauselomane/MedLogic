using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using TodosApi.Data;
using TodosApi.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly TodoDbContext _context;
    private readonly IMapper _mapper;

    public TodoController(TodoDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public IActionResult Todo()
    {
        //test data. Remove before submit
        //var tods = new Faker<Todo>()
        //.RuleFor(u => u.Id, f => Math.Abs(f.Random.Int()))
        //.RuleFor(u => u.Title, f => f.Name.FirstName())
        //.RuleFor(u => u.IsCompleted, f => f.Random.Bool())
        //.RuleFor(u => u.Description, f => f.Name.LastName());

        var todos = _context.TodoItems.ToList();
        var todoDtos = _mapper.Map<List<TodoViewModel>>(todos);

        return  Ok(todoDtos);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var todo = _context.TodoItems.Find(id);
        var todoDTO = _mapper.Map<TodoViewModel>(todo);
        return todoDTO == null ? NotFound() : Ok(todoDTO);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoViewModel item)
    {
        var dbTodo = _mapper.Map<Todo>(item);
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        dbTodo.UserId = int.Parse(userId);
        dbTodo.CreatedDate = DateTime.Now;
        _context.TodoItems.Add(dbTodo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, dbTodo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TodoViewModel updated)
    {
        var todo = _context.TodoItems.Find(id);
        if (todo == null) return NotFound();

        todo.Title = updated.Title;
        todo.Description = updated.Description;
        todo.IsCompleted = updated.IsCompleted;
        await _context.SaveChangesAsync();

        return Ok(todo);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] bool isCompleted)
    {
        var todo = _context.TodoItems.Find(id);
        if (todo == null) return NotFound();

        todo.IsCompleted = isCompleted;
        await _context.SaveChangesAsync();

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = _context.TodoItems.Find(id);
        if (todo == null) return NotFound();

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
