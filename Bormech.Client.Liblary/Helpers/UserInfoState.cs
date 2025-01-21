namespace Bormech.Client.Liblary.Helpers;

public class UserInfo
{
    public string? Id { get; set; } // {"Id":"1","Name":"string","Email":"lukasz.l@bormech.pl","Role":"Admin"}
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }

} 

public class UserInfoState
{
    private string Id { get; set; } // {"Id":"1","Name":"string","Email":"lukasz.l@bormech.pl","Role":"Admin"}
    private string? Name { get; set; }
    private string? Email { get; set; }
    private string? Role { get; set; }
    
    public void SetUserInfo(string id, string name, string email, string role)
    {
        Id = id;
        Name = name;
        Email = email;
        Role = role;
    }

    public UserInfo GetAll() => new UserInfo() { Id = Id, Name = Name, Email = Email, Role = Role };
}