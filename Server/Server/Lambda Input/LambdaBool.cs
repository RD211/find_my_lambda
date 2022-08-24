namespace Server.Lambda_Input;

/**
 * Lambda bool.
 * The class that models the bool.
 * It represents a intermediary state between
 * string representation and value representation.
 */
public class LambdaBool : LambdaInput
{
    /**
     * The value of the bool.
     * If null then the element is generic.
     */
    private readonly bool? _value;
    
    /**
     * Non-generic bool constructor.
     */
    public LambdaBool(bool input)
    {
        _value = input;
    }

    /**
     * Generic bool constructor.
     */
    public LambdaBool()
    {
    }

    /**
     * Gets the string representation of the bool.
     * Ex(non-generic): true, false
     * Ex(generic): bool
     */
    public override string GetStringRepresentation()
    {
        return (_value is null? "bool": _value.ToString()!.ToLower())!;
    }

    /**
     * Gets the value representation of the bool.
     * If bool is generic it throws an exception.
     * Ex: true, false
     */
    public override object? GetValueRepresentation()
    {
        if (IsGeneric()) throw new Exception("Tried to get value of generic bool.");
        return _value;
    }
    
    /**
     * Decomposes the bool.
     * Just returns a list of itself.
     */
    public override List<LambdaInput> Decompose()
    {
        return new List<LambdaInput> { this };
    }

    /**
     * Checks if the bool is generic or not.
     * Checks this by seeing if _value is set or not.
     */
    public override bool IsGeneric()
    {
        return _value is null;
    }
    
    /**
     * Converts bool to generic version.
     * If the bool is already generic it just returns
     * the current object without changes.
     */
    public override LambdaInput ToGeneric()
    {
        return new LambdaBool();
    }

    #region Generated overrides

    protected bool Equals(LambdaBool other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LambdaBool)obj);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    #endregion
}