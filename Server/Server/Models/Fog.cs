using System.Data.SqlClient;

namespace Server.Models;

/**
 * The fog class.
 * It reflects the data found in the database for fog.
 * A fog is a composition of 1 or more functions.
 */
public class Fog
{
    public static Fog ReadFog(SqlDataReader reader)
    {
        return new Fog(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetInt32(3),
            reader.GetInt32(4)
        );
    }

    public Fog(int id, string inputType, string returnType, int memberCount, int timesUsed)
    {
        Id = id;
        InputType = inputType;
        ReturnType = returnType;
        MemberCount = memberCount;
        TimesUsed = timesUsed;
    }

    public int Id { get; set; }
    public string InputType { get; set; }
    public string ReturnType { get; set; }
    public int MemberCount { get; set; }
    public int TimesUsed { get; set; }
}