using System.Text.Json.Serialization;
using Bormech.Data.Entities;

namespace BorAdmin.Data.Entities;

public class Relationship
{
    [JsonIgnore] public List<Employe>? Employes { get; set; }
}