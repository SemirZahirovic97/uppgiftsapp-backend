using Microsoft.AspNetCore.Mvc;
using uppgiftsapp_backend.Models;

namespace uppgiftsapp_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
  private static readonly List<TaskItem> _tasks = new()
    {
        new TaskItem { Id = 1, Title = "Handla mat", Description = "Mjölk, bröd och ägg", IsDone = false },
        new TaskItem { Id = 2, Title = "Plugga React", Description = "Läs om komponenter och useState", IsDone = false },
        new TaskItem { Id = 3, Title = "Skicka in uppgiften", Description = "Lämna in repo-länkarna", IsDone = true }
    };

  private static int _nextId = 4;

  private readonly IWebHostEnvironment _env;

  public TasksController(IWebHostEnvironment env)
  {
    _env = env;
  }

  [HttpGet]
  public ActionResult<List<TaskItem>> GetAll()
  {
    return Ok(_tasks);
  }

  [HttpPost]
  public ActionResult<TaskItem> Create(TaskItem newTask)
  {
    newTask.Id = _nextId++;
    _tasks.Add(newTask);
    return Ok(newTask);
  }

  [HttpPut("{id}")]
  public ActionResult<TaskItem> Update(int id, TaskItem updated)
  {
    var task = _tasks.FirstOrDefault(t => t.Id == id);
    if (task == null)
    {
      return NotFound();
    }

    task.Title = updated.Title;
    task.Description = updated.Description;
    task.IsDone = updated.IsDone;
    return Ok(task);
  }

  [HttpPost("{id}/image")]
  public async Task<ActionResult<TaskItem>> UploadImage(int id, IFormFile file)
  {
    var task = _tasks.FirstOrDefault(t => t.Id == id);
    if (task == null)
    {
      return NotFound();
    }

    if (file == null || file.Length == 0)
    {
      return BadRequest("Ingen fil skickades");
    }

    var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    if (!allowed.Contains(extension))
    {
      return BadRequest("Endast bilder är tillåtna");
    }

    var folder = Path.Combine(_env.WebRootPath, "uploads");
    Directory.CreateDirectory(folder);

    var fileName = Guid.NewGuid() + extension;
    var path = Path.Combine(folder, fileName);

    using (var stream = new FileStream(path, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    task.ImageUrl = "/uploads/" + fileName;
    return Ok(task);
  }
}