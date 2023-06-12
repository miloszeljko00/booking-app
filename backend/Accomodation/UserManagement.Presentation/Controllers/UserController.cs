using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Prometheus;
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
    private readonly ITracer _tracer;

    Counter UserCounter = Metrics.CreateCounter("user_counter", "Number of requests for User microservice" +
        "for given endpoint and with given status code", new CounterConfiguration
        {
            LabelNames = new[] { "microservice", "endpoint", "status" }
        });

    public UserController(IMediator mediator, ITracer tracer)
    {
        _tracer = tracer;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserCommand createUserCommand)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("Create user");
        var result = await _mediator.Send(createUserCommand);
        if (result is null)
        {
            UserCounter.WithLabels("user", "create_user", "400").Inc();
            return BadRequest();
        }
        UserCounter.WithLabels("user", "create_user", "200").Inc();
        return Ok(result);
    }

    [HttpPut]
    [Route("{userId}")]
    public async Task<ActionResult> UpdateUser(string userId, UpdateUserCommand updateUserCommand)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("Update user");
        if (userId != updateUserCommand.UserId)
        {
            UserCounter.WithLabels("user", "update_user", "400").Inc();
            return BadRequest();
        }
        var result = await _mediator.Send(updateUserCommand);
        if (result is null)
        {
            UserCounter.WithLabels("user", "update_user", "400").Inc();
            return BadRequest();
        }
        UserCounter.WithLabels("user", "update_user", "200").Inc();
        return Ok(result);
    }
    [HttpPut]
    [Route("updateFlightsApiKey/{userId}")]
    public async Task<ActionResult> UpdateUserFlightsApiKey(string userId, UpdateUserFlightsApiKeyCommand updateUserFlightsApiKeyCommand)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("Update user flight's API key");
        if (userId != updateUserFlightsApiKeyCommand.UserId)
        {
            UserCounter.WithLabels("user", "update_user_flights_api_key", "400").Inc();
            return BadRequest();
        }
        var result = await _mediator.Send(updateUserFlightsApiKeyCommand);
        if (result is null)
        {
            UserCounter.WithLabels("user", "update_user_flights_api_key", "400").Inc();
            return BadRequest();
        }
        UserCounter.WithLabels("user", "update_user_flights_api_key", "200").Inc();
        return Ok(result);
    }
    [HttpGet]
    [Route("getFlightsApiKey/{userId}")]
    public async Task<ActionResult> GetUserFlightsApiKey(string userId)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("Get user flight's API key");
        var result = await _mediator.Send(new GetUserFlightsApiKeyByUserIdQuery(userId));
        if (result is null)
        {
            UserCounter.WithLabels("user", "get_user_flights_api_key", "400").Inc();
            return BadRequest();
        }
        UserCounter.WithLabels("user", "get_user_flights_api_key", "200").Inc();
        return Ok(result);
    }
    [HttpGet]
    [Route("{userId}")]
    public async Task<ActionResult> GetUser(string userId)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("Get user");
        var result = await _mediator.Send(new GetUserByIdQuery(userId));
        if (result is null)
        {
            UserCounter.WithLabels("user", "get_user", "400").Inc();
            return BadRequest();
        }
        UserCounter.WithLabels("user", "get_user", "200").Inc();
        return Ok(result);
    }

    [HttpDelete]
    [Route("{userId}")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        using var scope = _tracer.BuildSpan(actionName).StartActive(true);
        scope.Span.Log("Delete user");
        var command = new DeleteUserCommand(userId);
        var result = await _mediator.Send(command);
        UserCounter.WithLabels("user", "delete_user", "200").Inc();
        return Ok(result);
    }

}
