using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a list implementation for <see cref="ItemPair{A, B}"/> objects
    /// </summary>
    /// <typeparam name="TValue1">The type of the first object</typeparam>
    /// <typeparam name="TValue2">The type of the second object</typeparam>
    [DebuggerDisplay("Count={Count}")]
    public class List<TValue1, TValue2> : IList<ItemPair<TValue1, TValue2>>
    {
        List<TValue1> m_ListA;
        List<TValue2> m_ListB;
        bool m_ReadOnly = false;

        #region constructor
        /// <summary>
        /// Creates a new empty list
        /// </summary>
        public List()
        {
            m_ListA = new List<TValue1>();
            m_ListB = new List<TValue2>();
        }

        /// <summary>
        /// Creates a new empty list with the specified capacity
        /// </summary>
        /// <param name="capacity"></param>
        public List(int capacity)
        {
            m_ListA = new List<TValue1>(capacity);
            m_ListB = new List<TValue2>(capacity);
        }

        /// <summary>
        /// Creates a list with the specified content
        /// </summary>
        /// <param name="items"></param>
        public List(IEnumerable<ItemPair<TValue1, TValue2>> items)
        {
            AddRange(items);
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Adds a range of items to the list
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<ItemPair<TValue1, TValue2>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (ItemPair<TValue1, TValue2> item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Sets the list readonly. This operation is not reversible.
        /// </summary>
        public void SetReadOnly()
        {
            m_ReadOnly = true;
        }

        /// <summary>
        /// Adds an ItemPair to the list
        /// </summary>
        /// <param name="value1">A item to add</param>
        /// <param name="value2">B item to add</param>
        public void Add(TValue1 value1, TValue2 value2)
        {
            if (m_ReadOnly)
            {
                throw new ReadOnlyException();
            }

            Add(new ItemPair<TValue1, TValue2>(value1, value2));
        }

        /// <summary>
        /// Inserts an ItemPair into the list
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="value1">A item to add</param>
        /// <param name="value2">B item to add</param>
        public void Insert(int index, TValue1 value1, TValue2 value2)
        {
            if (m_ReadOnly)
            {
                throw new ReadOnlyException();
            }

            Insert(index, new ItemPair<TValue1, TValue2>(value1, value2));
        }

        /// <summary>
        /// Gets/sets a A item at the first found B item
        /// </summary>
        /// <param name="value">The A value to search for</param>
        /// <returns>Returns the B value</returns>
        public TValue2 Find(TValue1 value)
        {
            int index = IndexOfA(value);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return m_ListB[index];
        }

        /// <summary>
        /// Gets/sets the ItemPair at the specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns></returns>
        public ItemPair<TValue1, TValue2> this[int index]
        {
            get => new ItemPair<TValue1, TValue2>(m_ListA[index], m_ListB[index]);
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (m_ReadOnly)
                {
                    throw new ReadOnlyException();
                }

                m_ListA[index] = value.A;
                m_ListB[index] = value.B;
            }
        }

        #endregion

        #region IList Members

        /// <summary>
        /// Reverses the order of the elements in the List
        /// </summary>
        public void Reverse()
        {
            m_ListA.Reverse();
            m_ListB.Reverse();
        }

        /// <summary>
        /// Removes the ItemPair at the specified index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (m_ReadOnly)
            {
                throw new ReadOnlyException();
            }

            m_ListA.RemoveAt(index);
            m_ListB.RemoveAt(index);
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        public void Clear()
        {
            if (m_ReadOnly)
            {
                throw new ReadOnlyException();
            }

            m_ListA.Clear();
            m_ListB.Clear();
        }

        /// <summary>
        /// Obtains the number of items present
        /// </summary>
        public int Count => m_ListA.Count;

        /// <summary>
        /// Obtains whether the list is readonly or not
        /// </summary>
        public bool IsReadOnly => m_ReadOnly;

        #endregion

        #region IList<A> Members
        /// <summary>
        /// Obtains the first index of the specified A value
        /// </summary>
        /// <param name="A">The value to look for</param>
        /// <returns>Returns the first index found or -1</returns>
        public int IndexOfA(TValue1 A)
        {
            return m_ListA.IndexOf(A);
        }

        /// <summary>
        /// Obtains the first index of the specified A value
        /// </summary>
        /// <param name="A">The value to look for</param>
        /// <param name="start">Index to start search</param>
        /// <returns>Returns the first index found or -1</returns>
        public int IndexOfA(TValue1 A, int start)
        {
            return m_ListA.IndexOf(A, start);
        }

        /// <summary>
        /// Obtains the A value of the ItemPair at the specified index
        /// </summary>
        /// <param name="index">The Index to read</param>
        /// <returns>Returns the A value read</returns>
        public TValue1 GetA(int index)
        {
            return m_ListA[index];
        }

        /// <summary>
        /// Sets the A value at the specified index
        /// </summary>
        /// <param name="index">The index to write at</param>
        /// <param name="A">The value to write</param>
        public void SetA(int index, TValue1 A)
        {
            m_ListA[index] = A;
        }

        /// <summary>
        /// Checks whether the specified value is present or not
        /// </summary>
        /// <param name="A">The value to search for</param>
        /// <returns>Returns true if the value is present false otherwise</returns>
        public bool ContainsA(TValue1 A)
        {
            return IndexOfA(A) > -1;
        }

        /// <summary>
        /// Copies all A items to the specified array starting at the specified index
        /// </summary>
        /// <param name="array">The array to write to</param>
        /// <param name="arrayIndex">The array index to start writing at</param>
        public void CopyTo(TValue1[] array, int arrayIndex)
        {
            m_ListA.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurance of the specified A value from the list
        /// </summary>
        /// <param name="A">The A value to remove</param>
        /// <returns>Returns true if an item was removed false otherwise</returns>
        public bool Remove(TValue1 A)
        {
            int index = IndexOfA(A);
            if (index < 0)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Obtains all A items present
        /// </summary>
        public TValue1[] ItemsA => m_ListA.ToArray();

        #endregion

        #region IList<B> Members

        /// <summary>
        /// Obtains the first index of the specified B value
        /// </summary>
        /// <param name="B">The value to look for</param>
        /// <param name="start">Index to start search</param>
        /// <returns>Returns the first index found or -1</returns>
        public int IndexOfB(TValue2 B, int start)
        {
            return m_ListB.IndexOf(B, start);
        }

        /// <summary>
        /// Obtains the first index of the specified B value
        /// </summary>
        /// <param name="B">The value to look for</param>
        /// <returns>Returns the first index found or -1</returns>
        public int IndexOfB(TValue2 B)
        {
            return m_ListB.IndexOf(B);
        }

        /// <summary>
        /// Obtains the B value of the ItemPair at the specified index
        /// </summary>
        /// <param name="index">The Index to read</param>
        /// <returns>Returns the B value read</returns>
        public TValue2 GetB(int index)
        {
            return m_ListB[index];
        }

        /// <summary>
        /// Sets the B value at the specified index
        /// </summary>
        /// <param name="index">The index to write at</param>
        /// <param name="B">The value to write</param>
        public void SetB(int index, TValue2 B)
        {
            m_ListB[index] = B;
        }

        /// <summary>
        /// Checks whether the specified value is present or not
        /// </summary>
        /// <param name="B">The value to search for</param>
        /// <returns>Returns true if the value is present false otherwise</returns>
        public bool ContainsB(TValue2 B)
        {
            return IndexOfB(B) > -1;
        }

        /// <summary>
        /// Copies all A items to the specified array starting at the specified index
        /// </summary>
        /// <param name="array">The array to write to</param>
        /// <param name="arrayIndex">The array index to start writing at</param>
        public void CopyTo(TValue2[] array, int arrayIndex)
        {
            m_ListB.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurance of the specified B value from the list
        /// </summary>
        /// <param name="B">The B value to remove</param>
        /// <returns>Returns true if an item was removed false otherwise</returns>
        public bool Remove(TValue2 B)
        {
            int index = IndexOfB(B);
            if (index < 0)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Obtains all B items present
        /// </summary>
        public TValue2[] ItemsB => m_ListB.ToArray();

        /// <summary>
        /// Gets/sets a B item at the first found A item
        /// </summary>
        /// <param name="value2">The B value to search for</param>
        /// <returns>Returns the A value</returns>
        public TValue1 Find(TValue2 value2)
        {
            int index = IndexOfB(value2);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value2));
            }

            return m_ListA[index];
        }

        #endregion

        #region IList<ItemPair<N, M>> Members
        /// <summary>
        /// Obtains the index of the specified ItemPair
        /// </summary>
        /// <param name="item">The ItemPair to search for</param>
        /// <returns>Returns the index or -1</returns>
        public int IndexOf(ItemPair<TValue1, TValue2> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int index = m_ListA.IndexOf(item.A);
            while (index > -1)
            {
                if (Equals(m_ListB[index], item.B))
                {
                    break;
                }

                index = m_ListA.IndexOf(item.A, index);
            }
            return index;
        }

        /// <summary>
        /// Inserts an ItemPair at the specified index
        /// </summary>
        /// <param name="index">The index to insert the ItemPair at</param>
        /// <param name="item">The ItemPair to insert</param>
        public void Insert(int index, ItemPair<TValue1, TValue2> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            m_ListA.Insert(index, item.A);
            m_ListB.Insert(index, item.B);
        }

        /// <summary>
        /// Adds a new ItemPair at the end of the list
        /// </summary>
        /// <param name="item">The ItemPair to add</param>
        public void Add(ItemPair<TValue1, TValue2> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            m_ListA.Add(item.A);
            m_ListB.Add(item.B);
        }

        /// <summary>
        /// Checks whether the list contains the specified ItemPair or not
        /// </summary>
        /// <param name="item">The ItemPair to search for</param>
        /// <returns>Returns true if the list contains the ItemPair false otherwise</returns>
        public bool Contains(ItemPair<TValue1, TValue2> item)
        {
            return IndexOf(item) > -1;
        }

        /// <summary>
        /// Copies all ItemPairs to the specified array
        /// </summary>
        /// <param name="array">The Array to write to</param>
        /// <param name="arrayIndex">The index to start writing at</param>
        public void CopyTo(ItemPair<TValue1, TValue2>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex++] = this[i];
            }
        }

        /// <summary>
        /// Removes the specified ItemPair from the list if it is present
        /// </summary>
        /// <param name="item">The ItemPair to remove</param>
        /// <returns>Returns true if an ItemPair was removed</returns>
        public bool Remove(ItemPair<TValue1, TValue2> item)
        {
            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Obtains an IEnumerator for all ItemPairs present
        /// </summary>
        /// <returns>Returns an IEnumerator</returns>
        public IEnumerator<ItemPair<TValue1, TValue2>> GetEnumerator()
        {
            ItemPair<TValue1, TValue2>[] array = new ItemPair<TValue1, TValue2>[Count];
            CopyTo(array, 0);
            return new List<ItemPair<TValue1, TValue2>>(array).GetEnumerator();
        }

        /// <summary>
        /// Obtains an IEnumerator for all ItemPairs present
        /// </summary>
        /// <returns>Returns an IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            ItemPair<TValue1, TValue2>[] array = new ItemPair<TValue1, TValue2>[Count];
            CopyTo(array, 0);
            return array.GetEnumerator();
        }
        #endregion
    }
}
