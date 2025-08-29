using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController(ApplicationDbContext db) : ControllerBase
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> Get([FromQuery] Models.TaskStatus? status)
    {
        var query = db.Tasks.AsNoTracking().Where(t => t.OwnerId == UserId);
        if (status.HasValue) query = query.Where(t => t.Status == status);

        var list = await query
        .OrderBy(t => t.Status)
        .ThenByDescending(t => t.Priority)
        .ThenBy(t => t.DueDate)
        .Select(t => new TaskDto(t.Id, t.Title, t.Description, t.Status, t.Priority, t.DueDate, t.CreatedAt, t.UpdatedAt))
        .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskDto>> GetOne(int id)
    {
        var t = await db.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == UserId);
        if (t is null) return NotFound();
        return Ok(new TaskDto(t.Id, t.Title, t.Description, t.Status, t.Priority, t.DueDate, t.CreatedAt, t.UpdatedAt));
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create([FromBody] TaskCreateUpdateDto dto)
    {
        var entity = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            OwnerId = UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        db.Tasks.Add(entity);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOne), new { entity = entity.Id }, entity);
    }
}