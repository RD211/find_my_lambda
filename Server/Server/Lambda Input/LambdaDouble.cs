namespace Server.Lambda_Input;

/**
 * Lambda double.
 * The class that models the double.
 * It represents a intermediary state between
 * string representation and value representation.
 */
public class LambdaDouble : LambdaInput
{
    /**
     * The value of the double.
     * If null then the element is generic.
     */
    private readonly double? _value;
    
    /**
     * The non-generic array constructor.
     */
    public LambdaDouble(double input)
    {
        _value = input;
    }

    /**
     * Generic double constructor.
     */
    public LambdaDouble()
    {
    }

    /**
     * Gets the string representation of the double.
     * Ex(non-generic): 1.0, 2.2, 3.4
     * Ex(generic): double
     */
    public override string GetStringRepresentation()
    {
        return (_value is null? "double": _value.ToString())!;
    }

    /**
     * Gets the value representation of the double.
     * If double is generic it throws an exception.
     * Ex: 1.0d, 2.0d, 3.4d, 5.9d
     */
    public override object GetValueRepresentation()
    {
        if (IsGeneric()) throw new Exception("Tried to get value of generic double.");
        return _value!;
    }
    
    /**
     * Decomposes the double.
     * Just returns a list of itself.
     */
    public override List<LambdaInput> Decompose()
    {
        return new List<LambdaInput> { this };
    }

    /**
     * Checks if the double is generic or not.
     * Checks this by seeing if _value is set or not.
     */
    public override bool IsGeneric()
    {
        return _value is null;
    }
    
    /**
     * Converts double to generic version.
     * If the double is already generic it just returns
     * the current object without changes.
     */
    public override LambdaInput ToGeneric()
    {
        return new LambdaDouble();
    }

    #region Generated overrides

    protected bool Equals(LambdaDouble other)
    {
        return Nullable.Equals(_value, other._value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LambdaDouble)obj);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    #endregion
}