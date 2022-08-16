using System.Data.SqlClient;

namespace Server.Models;

public class Lambda
{
    public Lambda(int id, string? programmingLanguage, string? code, string? inputType, string? returnType, DateTime uploadDate, int timesUsed)
    {
        Id = id;
        ProgrammingLanguage = programmingLanguage;
        Code = code;
        InputType = inputType;
        ReturnType = returnType;
        UploadDate = uploadDate;
        TimesUsed = timesUsed;
    }

    public static Lambda ReadLambda(SqlDataReader reader)
    {
        return new Lambda(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetDateTime(5),
            reader.GetInt32(6)
        );
    }

    public int Id { get; set; }
    public string? ProgrammingLanguage { get; set; }
    public string? Code { get; set; }
    public string? InputType { get; set; }
    public string? ReturnType { get; set; }
    public DateTime UploadDate { get; set; }
    public int TimesUsed { get; set; }
    
    
}