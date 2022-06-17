using DayFlags.Core.Models.Rest;
using DayFlags.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DayFlags.Core.Controller;

[ApiController]
[Route("/api/[controller]")]
public class DayEntriesController : ControllerBase
{

    private readonly DayEntriesService _dayEntryService;

    public DayEntriesController(DayEntriesService dayEntryService)
    {
        _dayEntryService = dayEntryService;
    }

    [HttpPost]
    public async Task<IActionResult> PostDayEntry(DayEntryRequest request)
    {
        var entity = await _dayEntryService.AddDayEntryAtAsync(
            request.EntryTypeId, request.Timestamp ?? DateTime.Now,
            request.Create
        );

        return Ok(entity);
    }
}