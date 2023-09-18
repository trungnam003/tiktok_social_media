using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Common.DTOs;
using Tiktok.API.Application.Features.TestBase.Queries.GetTestData;

namespace Tiktok.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TestController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("get-data-test")]
    public async Task<ActionResult> GetDataTest([FromQuery] TestDataDto data)
    {
        var query = _mapper.Map<GetDataQuery>(data);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}