using System.Text.Json.Serialization;

namespace Bormech.Data.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [JsonIgnore] public List<Employe>? Employes { get; set; }
}