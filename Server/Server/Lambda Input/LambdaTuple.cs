namespace Server.Lambda_Input;

/**
 * Lambda tuple.
 * The class that models the tuple.
 * It represents a intermediary state between
 * string representation and value representation.
 */
public class LambdaTuple : LambdaInput
{
    /**
     * The list of elements of the tuple.
     */
    private readonly List<LambdaInput> _values;

    /**
     * Unique constructor.
     * Different from the rest of the types.
     * It generates a generic tuple if every element in
     * the list is generic and non-generic if every element is non-generic.
     *
     * Throws exception if the list length is not between 1 and 8
     * Throws exception if some inputs are generic and others are not.
     */
    public LambdaTuple(List<LambdaInput> values)
    {
        if (values.Count is > 8 or < 1)
        {
            throw new ArgumentException("Invalid number of arguments for LambdaTuple.");
        }

        if (values.Any(input => input.IsGeneric()) && values.Any(input => !input.IsGeneric()))
        {
            throw new ArgumentException("Some generic and some not generic parameters provided to Lambda Tuple.");
        }
        
        _values = values;
    }

    /**
     * Gets the string representation of the string.
     * Ex(non-generic): (1,2.0), (1,2), ("hi", true), (true), (1,2,3)
     * Ex(generic): (int,double), (int,int), (string,bool), (bool), (int,int,int)
     */
    public override string GetStringRepresentation()
    {
        return 
            $"({string.Join(',', _values.Select((t) => t.GetStringRepresentation()))})";
    }

    /**
     * Gets the value representation of the tuple.
     * If tuple is generic it throws an exception.
     * Ex: (1,2), (""), (1,3,4,5), (2.0,4.0)
     */
    public override object? GetValueRepresentation()
    {
        if (IsGeneric()) throw new Exception("Tried to get value of generic string.");
        var typ = _values.Select(v => v.GetValueRepresentation())
            .ToList();
        
        return _values.Count switch
        {
            1 => ValueTuple.Create(typ[0]),
            2 => ValueTuple.Create(typ[0],typ[1]),
            3 => ValueTuple.Create(typ[0],typ[1],typ[2]),
            4 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3]),
            5 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4]),
            6 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5]),
            7 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5],typ[6]),
            8 => ValueTuple.Create(typ[0],typ[1],typ[2],typ[3],typ[4],typ[5],typ[6],typ[7]),
            _ => null
        };
    }
    
    /**
     * Decomposes the tuple.
     * Just returns the elements of the tuple.
     */
    public override List<LambdaInput> Decompose()
    {
        return _values;
    }

    /**
     * Checks if the tuple is generic or not.
     * Checks this by seeing if the first value in _values is set or not.
     */
    public override bool IsGeneric()
    {
        return _values[0].IsGeneric();
    }
    
    /**
     * Converts tuple to generic version.
     * If the tuple is already generic it just returns
     * the current object without changes.
     */
    public override LambdaInput ToGeneric()
    {
        return IsGeneric() ? this : new LambdaTuple(_values.Select(input => input.ToGeneric()).ToList());
    }

    #region Generated overrides

    protected bool Equals(LambdaTuple other)
    {
        return _values.SequenceEqual(other._values);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LambdaTuple)obj);
    }

    public override int GetHashCode()
    {
        return _values.GetHashCode();
    }

    #endregion
    
}