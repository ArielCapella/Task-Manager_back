using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs;

public record TaskCreateUpdateDto(
string Title,
string? Description,
Models.TaskStatus Status,
TaskPriority Priority,
DateTime? DueDate
);

public record TaskDto(
int Id,
string Title,
string? Description,
Models.TaskStatus Status,
TaskPriority Priority,
DateTime? DueDate,
DateTime CreatedAt,
DateTime UpdatedAt
);