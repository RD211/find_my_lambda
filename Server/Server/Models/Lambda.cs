using System.Data.SqlClient;

namespace Server.Models;

public class Lambda
{
    public Lambda(int id, string name, string description, string email, string programmingLanguage, string code, string inputType, string returnType, DateTime uploadDate, int timesUsed, bool confirmed, bool verified, int likes)
    {
        Id = id;
        Name = name;
        Description = description;
        Email = email;
        ProgrammingLanguage = programmingLanguage;
        Code = code;
        InputType = inputType;
        ReturnType = returnType;
        UploadDate = uploadDate;
        TimesUsed = timesUsed;
        Confirmed = confirmed;
        Verified = verified;
        Likes = likes;
    }

    public static Lambda ReadLambda(SqlDataReader reader)
    {
        return new Lambda(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetString(5),
            reader.GetString(6),
            reader.GetString(7),
            reader.GetDateTime(8),
            reader.GetInt32(9),
            reader.GetBoolean(10),
            reader.GetBoolean(11),
            reader.GetInt32(12)
        );
    }
    

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string ProgrammingLanguage { get; set; }
    public string Code { get; set; }
    public string InputType { get; set; }
    public string ReturnType { get; set; }
    public DateTime UploadDate { get; set; }
    public int TimesUsed { get; set; }
    public bool Confirmed { get; set; }
    public bool Verified { get; set; }
    public int Likes { get; set; }
}