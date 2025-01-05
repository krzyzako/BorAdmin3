using System.Text.Json.Serialization;
using BorAdmin.Data.Entities;
using Bormech.Data.Entities;

namespace BorAdmin3.Data.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [JsonIgnore] public List<Employe>? Employes { get; set; }
}