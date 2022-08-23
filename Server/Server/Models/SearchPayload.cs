namespace Server.Models;

public class SearchPayload
{
    public string[] Inputs { get; set; }
    public string[] Results { get; set; }
    
    public SearchPayload(string[] inputs, string[] results)
    {
        Inputs = inputs;
        Results = results;
    }
}