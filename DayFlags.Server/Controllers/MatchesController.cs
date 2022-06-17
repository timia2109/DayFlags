using DayFlags.Core.Models.Rest;
using DayFlags.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Core.Controller;

[ApiController]
[Route("/api/[controller]")]
public class MatchesController : ControllerBase
{

    private readonly EntryTypeService _entryTypeService;
    private readonly MatchingService _matchingService;

    public MatchesController(MatchingService matchingService,
        EntryTypeService entryTypeService)
    {
        _matchingService = matchingService;
        _entryTypeService = entryTypeService;
    }

    [HttpGet("{entryTypeId}")]
    public async Task<IActionResult> FindAllMatchings(string entryTypeId)
    {
        var entryType = await _entryTypeService.GetEntryTypeById(entryTypeId);
        var results = await _matchingService.FindAllMatchesAsync(entryType);
        return Ok(results);
    }
}