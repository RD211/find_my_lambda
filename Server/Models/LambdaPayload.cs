namespace Server.Models;

public class LambdaPayload
{
    public LambdaPayload(string programmingLanguage, string code, string name, string description, string email)
    {
        ProgrammingLanguage = programmingLanguage;
        Code = code;
        Name = name;
        Description = description;
        Email = email;
    }

    public string ProgrammingLanguage { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
}