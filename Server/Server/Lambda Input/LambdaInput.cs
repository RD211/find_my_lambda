namespace Server.Lambda_Input;

/**
 * The abstract base class of
 * the Lambda Input.
 * The purpose of the lambda input class
 * is for the input to be able to stay in an
 * intermediary state between string and value representations
 * and to make handling inputs easier.
 */
public abstract class LambdaInput { 
     /**
      * Gets the string representation of the lambda input.
      * Can be called both on generic and non-generic inputs.
      * Ex(non-generic): 5, 2.0, [1,2,3], (4,5)
      * Ex(generic): int, double, [int], (int,int)
      */
     public abstract string GetStringRepresentation(); 
     
     /**
     * Gets the value representation of the lambda input.
     * Must be called only on non-generic lambda inputs.
     * Exception or null if something went very wrong.
     */
    public abstract object? GetValueRepresentation();
 
    /**
     * Decomposes the lambda input.
     * Returns the children.
     */
    public abstract List<LambdaInput> Decompose();
    
    /**
     * Checks if lambda input is generic(has values) or not.
     */
    public abstract bool IsGeneric();
    
    /**
     * Converts a lambda input to a generic version.
     */
    public abstract LambdaInput ToGeneric();

    /**
     * Override for string to return string representation.
     */
    public override string ToString()
    {
        return GetStringRepresentation();
    }
}