using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IActionResult> Get(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var todo = await _context.TodoItems.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
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
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var todo = await _context.TodoItems.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();

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
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var todo = await _context.TodoItems.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();

        if (todo == null) return NotFound();

        todo.IsCompleted = isCompleted;
        await _context.SaveChangesAsync();

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var todo = await _context.TodoItems.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();

        //Would ideal use this, but need to ensure a person can only edit/add what they added
        //var todo = _context.TodoItems.Find(id);
        if (todo == null) return NotFound();

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
