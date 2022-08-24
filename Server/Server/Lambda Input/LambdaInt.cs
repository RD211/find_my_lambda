namespace Server.Lambda_Input;

/**
 * Lambda int.
 * The class that models the int.
 * It represents a intermediary state between
 * string representation and value representation.
 */
public class LambdaInt : LambdaInput 
{
    /**
     * The value of the int.
     * If null then the element is generic.
     */
    private readonly int? _value;
    
    /**
     * The non-generic array constructor.
     */
    public LambdaInt(int input)
    {
        _value = input;
    }

    /**
     * Generic int constructor.
     */
    public LambdaInt()
    {
    }

    /**
     * Gets the string representation of the int.
     * Ex(non-generic): 1, 2, 3, 4, -1, -2, -3, -4
     * Ex(generic): int
     */
    public override string GetStringRepresentation()
    {
        return (_value is null? "int": _value.ToString())!;
    }

    /**
     * Gets the value representation of an int.
     * If int is generic it throws an exception.
     * Ex: 1, 2, 3, 4, -1, -2, -3, -4
     */
    public override object GetValueRepresentation()
    {
        if (IsGeneric()) throw new Exception("Tried to get value of generic int.");
        return _value!;
    }
    
    /**
     * Decomposes the int.
     * Just returns a list of itself.
     */
    public override List<LambdaInput> Decompose()
    {
        return new List<LambdaInput> { this };
    }

    /**
     * Checks if the int is generic or not.
     * Checks this by seeing if _value is set or not.
     */
    public override bool IsGeneric()
    {
        return _value is null;
    }
    
    /**
     * Converts int to generic version.
     * If the int is already generic it just returns
     * the current object without changes.
     */
    public override LambdaInput ToGeneric()
    {
        return new LambdaInt();
    }

    #region Generated overrides

    protected bool Equals(LambdaInt other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LambdaInt)obj);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    #endregion
}