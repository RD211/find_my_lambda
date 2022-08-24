namespace Server.Lambda_Input;

/**
 * Lambda string.
 * The class that models the string.
 * It represents a intermediary state between
 * string representation and value representation.
 */
public class LambdaString : LambdaInput 
{
    /**
     * The value of the string.
     * If null then the element is generic.
     */
    private readonly string? _value;
    
    /**
     * The non-generic array constructor.
     */
    public LambdaString(string input)
    {
        _value = input;
    }

    /**
     * Generic string constructor.
     */
    public LambdaString()
    {
    }

    /**
     * Gets the string representation of the string.
     * Ex(non-generic): "hi", "cool", "there"
     * Ex(generic): string
     */
    public override string GetStringRepresentation()
    {
        return _value is null? "string": $"\"{_value}\"";
    }

    /**
     * Gets the value representation of a string.
     * If string is generic it throws an exception.
     * Ex: "a", "", "andy", "tester"
     */
    public override object GetValueRepresentation()
    {
        if (IsGeneric()) throw new Exception("Tried to get value of generic string.");
        return _value!;
    }
    
    /**
     * Decomposes the string.
     * Just returns a list of itself.
     */
    public override List<LambdaInput> Decompose()
    {
        return new List<LambdaInput> { this };
    }

    /**
     * Checks if the string is generic or not.
     * Checks this by seeing if _value is set or not.
     */
    public override bool IsGeneric()
    {
        return _value is null;
    }
    
    /**
     * Converts string to generic version.
     * If the string is already generic it just returns
     * the current object without changes.
     */
    public override LambdaInput ToGeneric()
    {
        return new LambdaString();
    }

    #region Generated overrides

    protected bool Equals(LambdaString other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LambdaString)obj);
    }

    public override int GetHashCode()
    {
        return (_value != null ? _value.GetHashCode() : 0);
    }


    #endregion
}