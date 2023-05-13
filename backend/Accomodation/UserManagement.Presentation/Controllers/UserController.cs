﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Users.Commands;

namespace UserManagement.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserCommand createUserCommand)
    {
        var result = await _mediator.Send(createUserCommand);
        if (result is null) return BadRequest();
        return Ok(result);
    }

    [HttpDelete]
    [Route("/{userId}")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        var command = new DeleteUserCommand(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}
