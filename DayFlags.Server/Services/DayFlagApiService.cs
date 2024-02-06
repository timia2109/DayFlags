using DayFlags.Core.Repositories;

namespace DayFlags.Server.Services;

/// <summary>
/// Utility helper to use DayFlags threw the API
/// </summary>
/// <param name="dayFlagRepository"></param>
/// <param name="flagGroupRepository"></param>
/// <param name="flagTypeRepository"></param>
public class DayFlagApiService(
    IDayFlagRepository dayFlagRepository,
    IFlagGroupRepository flagGroupRepository,
    IFlagTypeRepository flagTypeRepository
)
{

}