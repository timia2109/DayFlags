using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using DayFlags.Core.Models;

namespace DayFlags.Core.Database.Models;

/// <summary>
/// A registred Provider for this Realm
/// </summary>
public class RealmProvider
{
    [Key]
    public Guid RealmProviderId { get; init; }
    
    public Guid RealmId { get; init; }
    
    [StringLength(128)]
    public string ProviderId { get; init; }
    
    [Column(TypeName = "json")]
    public string Configuration { get; init; }
    
    [ForeignKey(nameof(RealmId))]
    public Realm? Realm { get; init; }

    public dynamic AsConfigurationType(Type type)
    {
        return JsonSerializer.Deserialize(Configuration, type) ?? throw new Exception("Unable to deserialize settings");
    }
}