using Tax_Radar_Domain.Common;
using Tax_Radar_Domain.ValueObjects;

namespace Tax_Radar_Domain.Entities;

public class User : BaseEntity
{
    public EmailAddress Email { get; private set; }
    public string FullName { get; private set; }
    public Ico? Ico { get; private set; }
    public Dic? Dic { get; private set; }
    public string PasswordHash { get; private set; }

    protected User()
    {
        Email = null!;
        FullName = string.Empty;
        PasswordHash = string.Empty;
    }

    public User(string email, string fullName, string passwordHash, string? ico = null, string? dic = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        Email = new EmailAddress(email);
        FullName = fullName;
        Ico = string.IsNullOrWhiteSpace(ico) ? null : new Ico(ico);
        Dic = string.IsNullOrWhiteSpace(dic) ? null : new Dic(dic);
        PasswordHash = passwordHash;
    }

    public void UpdateProfile(string fullName, string? ico, string? dic)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        FullName = fullName;
        Ico = string.IsNullOrWhiteSpace(ico) ? null : new Ico(ico);
        Dic = string.IsNullOrWhiteSpace(dic) ? null : new Dic(dic);
        MarkAsUpdated();
    }

    public void ChangeEmail(string email)
    {
        Email = new EmailAddress(email);
        MarkAsUpdated();
    }
}
