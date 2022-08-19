namespace Server.Models;

public class SearchPayload
{
    public SearchPayload(string[] inputs, string[] results)
    {
        Inputs = inputs;
        Results = results;
    }

    public string[] Inputs { get; set; }
    public string[] Results { get; set; }
}