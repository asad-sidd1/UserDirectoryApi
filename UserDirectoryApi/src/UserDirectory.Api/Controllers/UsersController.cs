#nullable enable
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserDirectory.Application.DTOs;
using UserDirectory.Application.Interfaces;
using UserDirectory.Domain.Entities;

namespace UserDirectory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _repo.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is null) return NotFound(new { message = $"User with ID {id} was not found." });
        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var user = _mapper.Map<User>(request);
        await _repo.AddAsync(user);
        var dto = _mapper.Map<UserDto>(user);

        return CreatedAtAction(nameof(GetUser), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto request)
    {
        if (id != request.Id) return BadRequest(new { message = "The ID in the URL does not match the ID in the request body." });
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return NotFound(new { message = $"User with ID {id} was not found." });

        var updated = _mapper.Map(request, existing);
        await _repo.UpdateAsync(updated);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return NotFound(new { message = $"User with ID {id} was not found." });

        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
