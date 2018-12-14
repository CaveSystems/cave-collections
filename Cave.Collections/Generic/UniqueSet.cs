using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a typed 2D set. This class uses a Set for each dimension allowing only unique values in each dimension.
    /// Additionally to the fast A (Key1) and B (Key2) lookup it provides indexing like a list.
    /// </summary>
    /// <typeparam name="TKey1">The type of the key1.</typeparam>
    /// <typeparam name="TKey2">The type of the key2.</typeparam>
    /// <seealso cref="Cave.Collections.Generic.IItemSet{TKey1, TKey2}" />
    [DebuggerDisplay("Count={Count}")]
    public sealed class UniqueSet<TKey1, TKey2> : IItemSet<TKey1, TKey2>
    {
        Dictionary<TKey1, ItemPair<TKey1, TKey2>> m_LookupA = new Dictionary<TKey1, ItemPair<TKey1, TKey2>>();
        Dictionary<TKey2, ItemPair<TKey1, TKey2>> m_LookupB = new Dictionary<TKey2, ItemPair<TKey1, TKey2>>();
        List<ItemPair<TKey1, TKey2>> m_List = new List<ItemPair<TKey1, TKey2>>();

        /// <summary>
        /// Rebuilds the index after an operation that destroys (one of) them.
        /// This is an O(n) operation, where n is Count.
        /// </summary>
        void RebuildIndex()
        {
            m_LookupA.Clear();
            m_LookupB.Clear();
            foreach (ItemPair<TKey1, TKey2> node in m_List)
            {
                m_LookupA.Add(node.A, node);
                m_LookupB.Add(node.B, node);
            }
        }

        /// <summary>
        /// Adds an item pair to the end of the List.
        /// This is an O(1) operation.
        /// </summary>
        /// <param name="A">The A object to be added</param>
        /// <param name="B">The B object to be added</param>
        public void Add(TKey1 A, TKey2 B)
        {
            ItemPair<TKey1, TKey2> node = new ItemPair<TKey1, TKey2>(A, B);
            m_LookupA.Add(A, node);
            try { m_LookupB.Add(B, node); }
            catch { m_LookupA.Remove(A); throw; }
            m_List.Add(node);
        }

        /// <summary>
        /// Clears the set.
        /// </summary>
        public void Clear()
        {
            m_List.Clear();
            m_LookupA.Clear();
            m_LookupB.Clear();
        }

        /// <summary>Determines whether the specified A is part of the set.</summary>
        /// <param name="A">The a value to check for</param>
        /// <returns><c>true</c> if present; otherwise, <c>false</c>.</returns>
        public bool ContainsA(TKey1 A)
        {
            return m_LookupA.ContainsKey(A);
        }

        /// <summary>Determines whether the specified B is part of the set.</summary>
        /// <param name="B">The b value to check for.</param>
        /// <returns><c>true</c> if present; otherwise, <c>false</c>.</returns>
        public bool ContainsB(TKey2 B)
        {
            return m_LookupB.ContainsKey(B);
        }

        /// <summary>Tries to the get the a key.</summary>
        /// <param name="a">The a key.</param>
        /// <param name="b">The b key.</param>
        /// <returns></returns>
        public bool TryGetA(TKey1 a, out TKey2 b)
        {
            if (m_LookupA.TryGetValue(a, out ItemPair<TKey1, TKey2> item))
            {
                b = item.B;
                return true;
            }
            else
            {
                b = default(TKey2);
                return false;
            }
        }

        /// <summary>Tries to the get the key b.</summary>
        /// <param name="b">The b key.</param>
        /// <param name="a">The a key.</param>
        /// <returns></returns>
        public bool TryGetB(TKey2 b, out TKey1 a)
        {
            if (m_LookupB.TryGetValue(b, out ItemPair<TKey1, TKey2> item))
            {
                a = item.A;
                return true;
            }
            else
            {
                a = default(TKey1);
                return false;
            }
        }

        /// <summary>
        /// Obtains the index of the specified A object.
        /// This is an O(1) operation.
        /// </summary>
        /// <param name="A">'A' object to be found.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOfA(TKey1 A)
        {
            if (!m_LookupA.ContainsKey(A))
            {
                return -1;
            }

            return m_List.IndexOf(m_LookupA[A]);
        }

        /// <summary>
        /// Obtains the index of the specified B object.
        /// This is an O(1) operation.
        /// </summary>
        /// <param name="B">'B' object to be found.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOfB(TKey2 B)
        {
            if (!m_LookupB.ContainsKey(B))
            {
                return -1;
            }

            return m_List.IndexOf(m_LookupB[B]);
        }

        /// <summary>Obtains a enumeration of the A elements of the Set.</summary>
        /// <value>The keys a.</value>
        public IEnumerable<TKey1> KeysA => m_LookupA.Keys;

        /// <summary>Obtains a enumeration of the B elements of the Set.</summary>
        /// <value>The keys b.</value>
        public IEnumerable<TKey2> KeysB => m_LookupB.Keys;

        /// <summary>
        /// Obtains a read only indexed list for the A elements of the Set.
        /// This method is an O(1) operation;
        /// </summary>
        public IList<TKey1> ItemsA => new ReadOnlyListA<TKey1, TKey2>(this);

        /// <summary>
        /// Obtains a read only indexed list for the B elements of the Set.
        /// This method is an O(1) operation;
        /// </summary>
        public IList<TKey2> ItemsB => new ReadOnlyListB<TKey1, TKey2>(this);

        /// <summary>
        /// Obtains the A element that is assigned to the specified B element.
        /// This method is an O(1) operation;
        /// </summary>
        /// <param name="B">The B index.</param>
        /// <returns></returns>
        public ItemPair<TKey1, TKey2> GetB(TKey2 B)
        {
            return m_LookupB[B];
        }

        /// <summary>
        /// Obtains the A element that is assigned to the specified B element.
        /// This method is an O(1) operation;
        /// </summary>
        /// <param name="A">The A index.</param>
        /// <returns></returns>
        public ItemPair<TKey1, TKey2> GetA(TKey1 A)
        {
            return m_LookupA[A];
        }

        /// <summary>
        /// Gets the number of elements actually present at the Set.
        /// </summary>
        public int Count => m_List.Count;

        /// <summary>
        /// Returns false
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Returns an enumerator that iterates through the set.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the set.</returns>
        public IEnumerator<ItemPair<TKey1, TKey2>> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the set.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the set.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        /// <summary>
        /// Obtains the index of the specified ItemPair.
        /// This is an O(1) operation.
        /// </summary>
        /// <param name="item">The ItemPair to search for.</param>
        /// <returns>The index of the ItemPair if found in the list; otherwise, -1.</returns>
        public int IndexOf(ItemPair<TKey1, TKey2> item)
        {
            return m_List.IndexOf(item);
        }

        /// <summary>
        /// Obtains the index of the specified ItemPair.
        /// This is an O(1) operation.
        /// </summary>
        /// <param name="A">The A value of the ItemPair to search for.</param>
        /// <param name="B">The B value of the ItemPair to search for.</param>
        /// <returns>The index of the ItemPair if found in the list; otherwise, -1.</returns>
        public int IndexOf(TKey1 A, TKey2 B)
        {
            return IndexOf(new ItemPair<TKey1, TKey2>(A, B));
        }

        /// <summary>
        /// Inserts a new ItemPair at the specified index.
        /// This method needs a full index rebuild and is an O(n) operation, where n is Count.
        /// </summary>
        /// <param name="index">The index to insert the item at.</param>
        /// <param name="A">The A value of the ItemPair to insert.</param>
        /// <param name="B">The B value of the ItemPair to insert.</param>
        public void Insert(int index, TKey1 A, TKey2 B)
        {
            Insert(index, new ItemPair<TKey1, TKey2>(A, B));
        }

        /// <summary>
        /// Inserts a new ItemPair at the specified index.
        /// This method needs a full index rebuild and is an O(n) operation, where n is Count.
        /// </summary>
        /// <param name="index">The index to insert the item at.</param>
        /// <param name="item">The ItemPair to insert.</param>
        public void Insert(int index, ItemPair<TKey1, TKey2> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            m_LookupA.Add(item.A, item);
            try { m_LookupB.Add(item.B, item); }
            catch { m_LookupA.Remove(item.A); throw; }
            m_List.Insert(index, item);
        }

        /// <summary>
        /// Removes the ItemPair at the specified index.
        /// This method needs a full index rebuild and is an O(n) operation, where n is Count.
        /// </summary>
        /// <param name="index">The index to remove the item.</param>
        public void RemoveAt(int index)
        {
            try
            {
                ItemPair<TKey1, TKey2> node = m_List[index];
                if (!m_LookupA.Remove(node.A))
                {
                    throw new KeyNotFoundException();
                }

                if (!m_LookupB.Remove(node.B))
                {
                    throw new KeyNotFoundException();
                }
            }
            catch
            {
                Clear();
                throw;
            }
        }

        /// <summary>
        /// Accesses the ItemPair at the specified index. The getter is a O(1) operation.
        /// The setter needs a full index rebuild and is an O(n) operation, where n is Count.
        /// </summary>
        /// <param name="index">The index of the ItemPair to be accessed.</param>
        /// <returns></returns>
        public ItemPair<TKey1, TKey2> this[int index]
        {
            get => m_List[index];
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                ItemPair<TKey1, TKey2> old = m_List[index];
                if (!m_LookupA.Remove(old.A))
                {
                    throw new KeyNotFoundException();
                }

                if (!m_LookupB.Remove(old.B))
                {
                    throw new KeyNotFoundException();
                }

                m_LookupA.Add(value.A, value);
                try { m_LookupB.Add(value.B, value); }
                catch { Clear(); throw; }
                m_List[index] = value;
            }
        }

        /// <summary>
        /// Adds an itempair to the set
        /// </summary>
        /// <param name="item"></param>
        public void Add(ItemPair<TKey1, TKey2> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Add(item.A, item.B);
        }

        /// <summary>
        /// Checks whether the list contains an itempair or not
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ItemPair<TKey1, TKey2> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return Contains(item.A, item.B);
        }

        /// <summary>
        /// Checks whether the list contains an itempair or not
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool Contains(TKey1 A, TKey2 B)
        {
            return m_LookupA.ContainsKey(A) && Equals(m_LookupA[A].B, B);
        }

        /// <summary>
        /// Copies the elements of the set to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(ItemPair<TKey1, TKey2>[] array, int arrayIndex)
        {
            m_List.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes an itempair from the set
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(ItemPair<TKey1, TKey2> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            try
            {
                m_LookupA.Remove(item.A);
                m_LookupB.Remove(item.B);
                return m_List.Remove(item);
            }
            catch
            {
                Clear();
                throw;
            }
        }

        /// <summary>Removes the item with the specified A key</summary>
        /// <param name="A">The A key</param>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        public void RemoveA(TKey1 A)
        {
            Remove(GetA(A));
        }

        /// <summary>Removes the item with the specified B key</summary>
        /// <param name="B">The A key</param>
        /// <exception cref="KeyNotFoundException">The exception that is thrown when the key specified for accessing an element in a collection does not match any key in the collection.</exception>
        public void RemoveB(TKey2 B)
        {
            Remove(GetB(B));
        }

        /// <summary>
        /// Removes an itempair from the set
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool Remove(TKey1 A, TKey2 B)
        {
            return Remove(new ItemPair<TKey1, TKey2>(A, B));
        }

        /// <summary>
        /// Reverses the index of the set
        /// </summary>
        public void Reverse()
        {
            m_List.Reverse();
            RebuildIndex();
        }
    }
}
