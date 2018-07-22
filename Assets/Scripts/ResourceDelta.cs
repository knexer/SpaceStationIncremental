using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ResourceDelta
{
    public ResourceType Type { get; }
    public int Amount { get; }

    public ResourceDelta(ResourceType type, int amount)
    {
        Type = type;
        Amount = amount;
    }

    public override bool Equals(object obj)
    {
        var delta = obj as ResourceDelta;
        return delta != null &&
               Type == delta.Type &&
               Amount == delta.Amount;
    }

    public override int GetHashCode()
    {
        int hashCode = -1636817442;
        hashCode = hashCode * -1521134295 + Type.GetHashCode();
        hashCode = hashCode * -1521134295 + Amount.GetHashCode();
        return hashCode;
    }
}
