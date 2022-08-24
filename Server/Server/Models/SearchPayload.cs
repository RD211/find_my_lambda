namespace Server.Models;

/**
 * Search payload.
 * This is what is sent to the server by the client
 * to request a Lambda search.
 * Inputs and results list must have the same size.
 */
public class SearchPayload
{
    public string[] Inputs { get; set; }
    public string[] Results { get; set; }
    
    public SearchPayload(string[] inputs, string[] results)
    {
        if (inputs.Length != results.Length)
            throw new ArgumentException("Inputs has different size from results in SearchPayload.");
        
        Inputs = inputs;
        Results = results;
    }
}