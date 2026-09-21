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
}