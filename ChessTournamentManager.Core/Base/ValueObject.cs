namespace ChessTournamentManager.Core.Base;

public abstract class ValueObject : IComparable, IComparable<ValueObject>, IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        left switch
        {
            null when right is null => true,
            null => false,
            not null when right is null => false,
            _ => left.Equals(right)
        };
    
    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);


    public override bool Equals(object? obj) => Equals(obj as ValueObject);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var component in GetComparableComponents())
        {
            hashCode.Add(component); 
        }
        
        return hashCode.ToHashCode();
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }
        
        var thisType = GetType();
        var otherType = obj.GetType();
        
        return thisType != otherType
            ? string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal)
            : CompareTo((ValueObject) obj);
    }

    public int CompareTo(ValueObject? other)
    {
        if (other is null)
        {
            return 1;
        }

        foreach (var (thisComponent, otherComponent) in GetComparableComponents().Zip(other.GetComparableComponents()))
        {
            int comparisonResult = CompareComponents(thisComponent, otherComponent);
            
            if (comparisonResult != 0)
            {
                return comparisonResult;
            }
        }
        
        return 0;
    }

    public bool Equals(ValueObject? other) =>
        other is not null
        && GetType() == other.GetType()
        && GetComparableComponents().SequenceEqual(other.GetComparableComponents());

    protected abstract IEnumerable<object> GetComparableComponents();
    
    private static int CompareComponents(object? left, object? right) =>
        left switch
        {
            null when right is null => 0,
            null => -1,
            not null when right is null => 1,
            IComparable leftComparable when right is IComparable rightComparable => leftComparable.CompareTo(
                rightComparable),
            _ => left.Equals(right) ? 0 : -1
        };
}