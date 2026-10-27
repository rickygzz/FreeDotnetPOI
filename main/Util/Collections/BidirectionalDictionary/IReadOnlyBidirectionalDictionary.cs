using System.Collections.Generic;

namespace NPOI.Util.Collections.BidirectionalDictionary
{
    public interface IReadOnlyBidirectionalDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        public IReadOnlyBidirectionalDictionary<TValue, TKey> Inverse { get; }
        public bool ContainsValue(TValue value);
    }
}
