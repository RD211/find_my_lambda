namespace Server.Lambda_Input;

/**
 * Lambda array.
 * The class that models the array.
 * It represents a intermediary state between
 * string representation and value representation.
 */
public class LambdaArray : LambdaInput
{
    /**
     * The list of elements of the array.
     * Null if array is generic.
     * Must be set otherwise.
     */
    private readonly List<LambdaInput>? _values;
    
    /**
     * The type of the elements of the array.
     * Null if array is non-generic.
     * Must be set otherwise.
     */
    private readonly LambdaInput? _type;

    /**
     * The non-generic array constructor.
     */
    public LambdaArray(List<LambdaInput> values)
    {
        // The number of values must be greater than 0.
        // Type can't be figured out with no elements.
        if (values.Count == 0)
        {
            throw new ArgumentException("Wrong number of elements provided for LambdaArray. " +
                                        "There must be at least 1 to figure out the type.");
        }

        // All values must all be non-generic.
        if (values.Any(input => input.IsGeneric()))
        {
            throw new ArgumentException("Bad arguments for lambda array. " +
                                        "Generic type provided in non generic constructor.");
        }

        if (values.Select(input => input.ToGeneric().ToString()).ToHashSet().Count != 1)
        {
            throw new ArgumentException("Bad arguments for lambda array. " +
                                        "Elements of multiple types provided.");
        }
        
        _values = values;
    }
    
    /**
     * The generic array constructor.
     */
    public LambdaArray(LambdaInput type)
    {
        // If the type is not actually generic throw an error.
        if (!type.IsGeneric())
        {
            throw new ArgumentException("Bad arguments for lambda array. " +
                                        "Not generic type provided in generic constructor.");
        }
        
        _type = type;
    }

    /**
     * Gets the string representation of the array.
     * Ex(non-generic): [1,2,3], [true,false], [1.0,2.3], ["hi", "you"]
     * Ex(generic): [int], [bool], [double], [string], [[int]]
     */
    public override string GetStringRepresentation()
    {
        return _values is null ? $"[{_type!.GetStringRepresentation()}]" :
            $"[{string.Join(',', _values!.Select((t) => t.GetStringRepresentation()))}]";
    }

    /**
     * Gets the value representation of an array.
     * If array is generic it throws an exception.
     * Ex: new int[]{1,2,3}, new double[]{1d,2d,3d}, new string[]{"hi", "you"}
     */
    public override object GetValueRepresentation()
    {
        if (IsGeneric()) throw new Exception("Tried to get value of generic lambda array.");
        return _values!.Select(input => input.GetValueRepresentation()).ToArray();
    }
    
    /**
     * Decomposes the array.
     * Returns a list:
     *  If generic -> return a list of the inner type.
     *  if non-generic -> return the list of elements.
     */
    public override List<LambdaInput> Decompose()
    {
        return _values is null? new List<LambdaInput>{_type!} : _values!;
    }

    /**
     * Checks if the array is generic or not.
     * Checks this by seeing if _value is set or not.
     */
    public override bool IsGeneric()
    {
        return _values is null;
    }
    
    /**
     * Converts array to generic version.
     * If the array is already generic it just returns
     * the current object without changes.
     */
    public override LambdaInput ToGeneric()
    {
        return IsGeneric() ? this : new LambdaArray(_values!.First().ToGeneric());
    }

    #region Generated overrides

    protected bool Equals(LambdaArray other)
    {
        if (_values is null || other._values is null)
        {
            return Equals(_values, other._values) && Equals(_type, other._type);
        }
        
        return _values.SequenceEqual(other._values);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LambdaArray)obj);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(_values, _type);
    }
    #endregion
}