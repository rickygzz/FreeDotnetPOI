using System.Collections.Generic;

namespace NPOI.Util.Collections.BidirectionalDictionary;

public interface IBidirectionalDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    public IBidirectionalDictionary<TValue, TKey> Inverse { get; }
    public bool ContainsValue(TValue value);
}
