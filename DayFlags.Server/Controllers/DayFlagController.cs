using DayFlags.Core.Repositories;

namespace DayFlags.Server.Controllers;

public class DayFlagController(IRealmRepository realmRepository,
    IDayFlagRepository dayFlagRepository) : BaseRealmRelatedController(realmRepository)
{

}