using System;
using System.Collections.Generic;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a set of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.ICollection{T}" />
    public interface IItemSet<T> : ICollection<T>
    {
        /// <summary>
        /// Checks whether all specified items are part of the set
        /// </summary>
        /// <exception cref="ArgumentNullException">items</exception>
        /// <returns>Returns true if all items are present.</returns>
        bool ContainsRange(IEnumerable<T> items);

        /// <summary>
        /// Returns true if the set is empty
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Adds a range of items to the set
        /// </summary>
        /// <param name="items">The items to be added to the list</param>
        /// <exception cref="ArgumentNullException">items</exception>
        /// <exception cref="ArgumentException">Item is already present!</exception>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Adds a range of items to the set
        /// </summary>
        /// <param name="items">The items to be added to the list</param>
        /// <exception cref="ArgumentNullException">items</exception>
        /// <exception cref="ArgumentException">Item is already present!</exception>
        void AddRange(params T[] items);

        /// <summary>
        /// Includes an item that is not already present in the set (others are ignored).
        /// </summary>
        /// <param name="item">The item to be included</param>
        /// <returns>Returns true if the item was added, false if it was present already.</returns>
        bool Include(T item);

        /// <summary>
        /// Includes items that are not already present in the set (others are ignored).
        /// </summary>
        /// <param name="items">The items to be included</param>
        /// <returns>Returns the number of items added.</returns>
        int IncludeRange(IEnumerable<T> items);

        /// <summary>
        /// Adds a range of items to the set
        /// </summary>
        /// <param name="items">The items to be added to the list</param>
        /// <returns>Returns the number of items added.</returns>
        int IncludeRange(params T[] items);

        /// <summary>Tries the remove the specified value.</summary>
        /// <param name="value">The value to remove.</param>
        /// <returns>Returns true if the item was found and removed, false otherwise.</returns>
        bool TryRemove(T value);

        /// <summary>
        /// Removes items from the set
        /// </summary>
        /// <returns>Returns the number of items removed.</returns>
        int TryRemoveRange(IEnumerable<T> items);

        /// <summary>Removes an item from the set</summary>
        /// <param name="item">The item to be removed</param>
        /// <returns>Returns always true.</returns>
        /// <exception cref="KeyNotFoundException">Key not found!</exception>
        new void Remove(T item);

        /// <summary>
        /// Removes items from the set
        /// </summary>
        void RemoveRange(IEnumerable<T> items);
    }

    /// <summary>
    /// Provides an interface for 2D set implementations (Each set A and B may only contain each value once. If typeof(A) == typeof(B)
    /// a value may be present once at each set. Each value in set a is linked to a value in set b via its index)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IItemSet<TKey, TValue> : IList<ItemPair<TKey, TValue>>
    {
        /// <summary>
        /// Obtains the index of the specified A item
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if the set does not support indexing</exception>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a A index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <param name="A">The A key</param>
        /// <returns>Returns the index of the item</returns>
        int IndexOfA(TKey A);

        /// <summary>
        /// Obtains the index of the specified B item
        /// </summary>
        /// <param name="B">The B index.</param>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a B index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <returns>Returns the index of the item</returns>
        int IndexOfB(TValue B);

        /// <summary>
        /// Checks whether a specified A key is present
        /// </summary>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a A index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <param name="A">The A key</param>
        /// <returns>Returns true if the key is present</returns>
        bool ContainsA(TKey A);

        /// <summary>
        /// Checks whether a specified B key is present
        /// </summary>
        /// <param name="B">The B index.</param>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a B index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <returns>Returns true if the key is present</returns>
        bool ContainsB(TValue B);

        /// <summary>
        /// Obtains a list of all A items
        /// </summary>
        IList<TKey> ItemsA { get; }

        /// <summary>
        /// Obtains a list of all B items
        /// </summary>
        IList<TValue> ItemsB { get; }

        /// <summary>Removes the item with the specified A key</summary>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a A index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <param name="A">The A key</param>
        void RemoveA(TKey A);

        /// <summary>Removes the item with the specified B key</summary>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a B index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <param name="B">The A key</param>
        void RemoveB(TValue B);

        /// <summary>Gets the item with the specified A index.</summary>
        /// <param name="A">The A index.</param>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a A index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <returns>Returns the <see cref="ItemPair{A, B}"/></returns>
        ItemPair<TKey, TValue> GetA(TKey A);

        /// <summary>Gets the item with the specified B index.</summary>
        /// <param name="B">The B index.</param>
        /// <exception cref="NotSupportedException">The exception that is thrown when the set does not support a B index.</exception>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        /// <returns>Returns the <see cref="ItemPair{A, B}"/></returns>
        ItemPair<TKey, TValue> GetB(TValue B);
    }
}
