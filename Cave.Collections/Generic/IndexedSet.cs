#region CopyRight 2018
/*
    Copyright (c) 2003-2018 Andreas Rohleder (andreas@rohleder.cc)
    All rights reserved
*/
#endregion
#region License LGPL-3
/*
    This program/library/sourcecode is free software; you can redistribute it
    and/or modify it under the terms of the GNU Lesser General Public License
    version 3 as published by the Free Software Foundation subsequent called
    the License.

    You may not use this program/library/sourcecode except in compliance
    with the License. The License is included in the LICENSE file
    found at the installation directory or the distribution package.

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be included
    in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion
#region Authors & Contributors
/*
   Author:
     Andreas Rohleder <andreas@rohleder.cc>

   Contributors:

 */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a generic typed set of objects
    /// </summary>

    [DebuggerDisplay("Count={Count}")]
    public sealed class IndexedSet<T> : IList<T>, IEquatable<IndexedSet<T>>
    {
        #region operators

        /// <summary>
        /// Obtains a <see cref="Set{T}"/> containing all objects part of one of the specified sets
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> operator |(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            return BitwiseOr(set1, set2);
        }

        /// <summary>
        /// Obtains a <see cref="Set{T}"/> containing only objects part of both of the specified sets
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> operator &(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            return BitwiseAnd(set1, set2);
        }

        /// <summary>
        /// Obtains a <see cref="Set{T}"/> containing all objects part of the first set after removing all objects present at the second set.
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> operator -(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            return Subtract(set1, set2);
        }

        /// <summary>
        /// Builds a new <see cref="Set{T}"/> containing only the items found exclusivly in one of both specified sets.
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> operator ^(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            return Xor(set1, set2);
        }

        /// <summary>
        /// Checks two sets for equality
        /// </summary>
        /// <param name="set1"></param>
        /// <param name="set2"></param>
        /// <returns></returns>
        public static bool operator ==(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            if (ReferenceEquals(set1, null)) return (ReferenceEquals(set2, null));
            if (ReferenceEquals(set2, null)) return false;
            return set1.Equals(set2);
        }

        /// <summary>
        /// Checks two sets for inequality
        /// </summary>
        /// <param name="set1"></param>
        /// <param name="set2"></param>
        /// <returns></returns>
        public static bool operator !=(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            return !(set1 == set2);
        }
        #endregion

        #region static Member

        /// <summary>
        /// Builds the union of two specified <see cref="Set{T}"/>s
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> BitwiseOr(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            if (set1 == null) throw new ArgumentNullException("set1");
            if (set2 == null) throw new ArgumentNullException("set2");
            if (set1.Count < set2.Count) return BitwiseOr(set2, set1);

            IndexedSet<T> result = new IndexedSet<T>();
            result.AddRange(set2);
            foreach (T item in set1)
            {
                if (set2.Contains(item)) continue;
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Builds the intersection of two specified <see cref="Set{T}"/>s
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> BitwiseAnd(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            if (set1 == null) throw new ArgumentNullException("set1");
            if (set2 == null) throw new ArgumentNullException("set2");
            if (set1.Count < set2.Count) return BitwiseAnd(set2, set1);

            IndexedSet<T> result = new IndexedSet<T>();
            foreach (T itemsItem in set1)
            {
                if (set2.Contains(itemsItem)) result.Add(itemsItem);
            }
            return result;
        }

        /// <summary>
        /// Subtracts the specified <see cref="Set{T}"/> from this one and returns a new <see cref="Set{T}"/> containing the result
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> Subtract(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            if (set1 == null) throw new ArgumentNullException("set1");
            if (set2 == null) throw new ArgumentNullException("set2");
            IndexedSet<T> result = new IndexedSet<T>();
            foreach (T setItem in set1)
            {
                if (!set2.Contains(setItem))
                {
                    result.Add(setItem);
                }
            }
            return result;
        }

        /// <summary>
        /// Builds a new <see cref="Set{T}"/> containing only items found exclusivly in one of both specified sets.
        /// </summary>
        /// <param name="set1">The first set used to calculate the result</param>
        /// <param name="set2">The second set used to calculate the result</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public static IndexedSet<T> Xor(IndexedSet<T> set1, IndexedSet<T> set2)
        {
            if (set1 == null) throw new ArgumentNullException("set1");
            if (set2 == null) throw new ArgumentNullException("set2");
            if (set1.Count < set2.Count) return Xor(set2, set1);

            LinkedList<T> newSet2 = new LinkedList<T>(set2);
            IndexedSet<T> result = new IndexedSet<T>();
            foreach (T setItem in set1)
            {
                if (!set2.Contains(setItem))
                {
                    result.Add(setItem);
                }
                else
                {
                    newSet2.Remove(setItem);
                }
            }
            result.AddRange(newSet2);
            return result;
        }

        #endregion

        #region private Member

        List<T> m_Items;
        Dictionary<T, int> m_Lookup;

        #endregion

        #region constructors

        /// <summary>
        /// Creates a new empty set
        /// </summary>
        public IndexedSet()
        {
            m_Items = new List<T>();
            m_Lookup = new Dictionary<T, int>();
        }

        /// <summary>
        /// Creates a new empty set
        /// </summary>
        public IndexedSet(int capacity)
        {
            m_Items = new List<T>(capacity);
            m_Lookup = new Dictionary<T, int>(capacity);
        }

        /// <summary>
        /// Creates a new set with the specified items
        /// </summary>
        public IndexedSet(T item)
            : this(256)
        {
            Add(item);
        }

        /// <summary>
        /// Creates a new set with the specified items
        /// </summary>
        public IndexedSet(IEnumerable<T> items)
            : this()
        {
            AddRange(items);
        }

        #endregion

        #region public Member

        /// <summary>
        /// Builds the union of the specified and this <see cref="Set{T}"/> and returns a new set with the result.
        /// </summary>
        /// <param name="items">Provides the other <see cref="Set{T}"/> used.</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public IndexedSet<T> Union(IndexedSet<T> items)
        {
            return BitwiseOr(this, items);
        }

        /// <summary>
        /// Builds the intersection of the specified and this <see cref="Set{T}"/> and returns a new set with the result.
        /// </summary>
        /// <param name="items">Provides the other <see cref="Set{T}"/> used.</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public IndexedSet<T> Intersect(IndexedSet<T> items)
        {
            return BitwiseAnd(this, items);
        }

        /// <summary>
        /// Subtracts a specified <see cref="Set{T}"/> from this one and returns a new set with the result.
        /// </summary>
        /// <param name="items">Provides the other <see cref="Set{T}"/> used.</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public IndexedSet<T> Subtract(IndexedSet<T> items)
        {
            return Subtract(this, items);
        }

        /// <summary>
        /// Builds a new <see cref="Set{T}"/> containing only items found exclusivly in one of both specified sets.
        /// </summary>
        /// <param name="items">Provides the other <see cref="Set{T}"/> used.</param>
        /// <returns>Returns a new <see cref="Set{T}"/> containing the result.</returns>
        public IndexedSet<T> ExclusiveOr(IndexedSet<T> items)
        {
            return Xor(this, items);
        }

        /// <summary>
        /// Checks whether a specified object is part of the set
        /// </summary>
        public bool Contains(T item)
        {
            return m_Lookup.ContainsKey(item);
        }

        /// <summary>
        /// Checks whether all specified objects are part of the set
        /// </summary>
        public bool ContainsRange(ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            bool allFound = true;
            foreach (T obj in collection)
            {
                allFound &= Contains(obj);
            }
            return allFound;
        }

        /// <summary>
        /// Returns true if the set was empty
        /// </summary>
        public bool IsEmpty
        {
            get { return m_Items.Count == 0; }
        }

        /// <summary>
        /// Adds a specified object to the set
        /// </summary>
        /// <param name="item">The item to be added to the set</param>
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            int index = m_Items.Count;
            m_Items.Add(item);
            m_Lookup.Add(item, index);
        }

        /// <summary>
        /// Adds a range of objects to the set
        /// </summary>
        /// <param name="items">The objects to be added to the list</param>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (T obj in items) { Add(obj); }
        }

        /// <summary>
        /// Includes an object that is not already present in the set (others are ignored).
        /// </summary>
        /// <param name="obj">The object to be included</param>
        public void Include(T obj)
        {
            if (!Contains(obj)) Add(obj);
        }

        /// <summary>
        /// Includes objects that are not already present in the set (others are ignored).
        /// </summary>
        /// <param name="items">The objects to be included</param>
        public void IncludeRange(ICollection<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            foreach (T obj in items) { Include(obj); }
        }

        void RebuildIndex()
        {
            m_Lookup.Clear();
            for(int i = 0; i < m_Items.Count; i++)
            {
                m_Lookup.Add(m_Items[i], i);
            }
        }

        /// <summary>
        /// Removes an object from the set
        /// </summary>
        /// <param name="item">The object to be removed</param>
        public bool Remove(T item)
        {
            RemoveAt(m_Lookup[item]);
            return true;
        }

        /// <summary>
        /// Removes objects from the set
        /// </summary>
        public void RemoveRange(ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            foreach (T obj in collection) { Remove(obj); }
        }

        /// <summary>
        /// Clears the set
        /// </summary>
        public void Clear()
        {
            m_Lookup.Clear();
            m_Items.Clear();
        }
        #endregion

        #region IList<T> member
        
        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in the set.
        /// </summary>
        /// <param name="item">The object to locate in the set.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire set, if found; otherwise, –1.</returns>
        public int IndexOf(T item)
        {
            return m_Lookup[item];
        }

        /// <summary>
        /// Inserts an element into the set at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > m_Items.Count) throw new IndexOutOfRangeException();
            m_Lookup.Add(item, index);
            m_Items.Insert(index, item);
            for (int i = index + 1; i < m_Items.Count; i++)
            {
                m_Lookup[m_Items[i]] = i - 1;
            }
        }

        /// <summary>
        /// Removes the element at the specified index of the set.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index > m_Items.Count) throw new IndexOutOfRangeException();
            for (int i = index + 1; i < m_Items.Count; i++)
            {
                m_Lookup[m_Items[i]] = i - 1;
            }
            if (!m_Lookup.Remove(m_Items[index])) throw new IndexOutOfRangeException();
            m_Items.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return m_Items[index];
            }
            set
            {
                T oldKey = m_Items[index];
                if (!m_Lookup.Remove(oldKey)) throw new KeyNotFoundException();
                m_Lookup.Add(value, index);
                m_Items[index] = value;
            }
        }
        #endregion

        #region ICollection Member

        /// <summary>
        /// Copies all objects present at the set to the specified array, starting at a specified index
        /// </summary>
        /// <param name="array">one-dimensional array to copy to</param>
        /// <param name="arrayIndex">the zero-based index in array at which copying begins</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Obtains the number of objects present at the set
        /// </summary>
        public int Count
        {
            get { return m_Items.Count; }
        }

        #endregion

        #region IEnumerable Member

        /// <summary>
        /// Obtains an <see cref="IEnumerator"/> for this set
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        #endregion

        #region ICloneable Member

        /// <summary>
        /// Creates a copy of this set
        /// </summary>
        public object Clone()
        {
            return new IndexedSet<T>(m_Items);
        }

        #endregion

        #region ICollection<T> Member

        /// <summary>
        /// Returns false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable<T> Member

        /// <summary>
        /// Obtains an <see cref="IEnumerator"/> for this set
        /// </summary>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Obtains an array of all elements present
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return m_Items.ToArray();
        }

        /// <summary>
        /// Checks another Set{T} instance for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            IndexedSet<T> other = obj as IndexedSet<T>;
            if (null == other) return false;
            return Equals(other);
        }

        /// <summary>
        /// Checks another Set{T} instance for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IndexedSet<T> other)
        {
            if (other == null) return false;
            if (other.Count != Count) return false;
            return ContainsRange(other);
        }

        /// <summary>
        /// Obtains the hash code of the base list.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_Items.GetHashCode();
        }
    }
}

