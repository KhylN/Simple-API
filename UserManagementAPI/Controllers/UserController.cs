using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;
using UserManagementAPI.Repositories;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository repo, ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll() => Ok(repo.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<User> GetOne(Guid id)
    {
        var user = repo.Get(id);
        return user is null ? NotFound(new { error = "User not found." }) : Ok(user);
    }

    [HttpPost]
    public ActionResult<User> Create([FromBody] CreateUserDto dto)
    {
        var user = new User { Name = dto.Name.Trim(), Email = dto.Email.Trim(), Age = dto.Age };
        var created = repo.Add(user);
        return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateUserDto dto)
    {
        var existing = repo.Get(id);
        if (existing is null) return NotFound(new { error = "User not found." });

        existing.Name = dto.Name.Trim();
        existing.Email = dto.Email.Trim();
        existing.Age = dto.Age;

        repo.Update(existing);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        return repo.Delete(id) ? NoContent() : NotFound(new { error = "User not found." });
    }
}
