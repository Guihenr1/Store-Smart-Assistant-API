namespace StoreSmart.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    
    private User() { }
    
    public static User Create(string email, string name, string hashPassword)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(), 
            Email = email, 
            Name = name,
            PasswordHash = hashPassword,
            IsActive = true
        };
        
        return user;
    }
}