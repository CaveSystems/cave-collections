using System.Collections.Generic;
using System.Diagnostics;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a debug view for collections
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CollectionDebuggerView<T>
    {
        ICollection<T> Collection;

        /// <summary>
        /// Creates a new instance for the specified collection
        /// </summary>
        /// <param name="collection"></param>
        public CollectionDebuggerView(ICollection<T> collection)
        {
            Collection = collection;
        }

        /// <summary>
        /// Obtains all items
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] result = new T[Collection.Count];
                Collection.CopyTo(result, 0);
                return result;
            }
        }
    }

    internal sealed class CollectionDebuggerView<TKey, TValue>
    {
        ICollection<KeyValuePair<TKey, TValue>> Collection;

        public CollectionDebuggerView(ICollection<KeyValuePair<TKey, TValue>> collection)
        {
            Collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                KeyValuePair<TKey, TValue>[] result = new KeyValuePair<TKey, TValue>[Collection.Count];
                Collection.CopyTo(result, 0);
                return result;
            }
        }
    }
}
