using System.Text.Json.Serialization;

namespace Bormech.Data.Entities;

public class Relationship
{
    [JsonIgnore] public List<Employe>? Employes { get; set; }
}