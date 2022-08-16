namespace Server.Models;

public class LambdaPayload
{
    public LambdaPayload(string programmingLanguage, string code)
    {
        ProgrammingLanguage = programmingLanguage;
        Code = code;
    }

    public string ProgrammingLanguage { get; set; }
    public string Code { get; set; }
}