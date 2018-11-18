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
using System.Data;
using System.Diagnostics;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a readonly collection implementation for A items of <see cref="Set{A, B}"/>
    /// </summary>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    [DebuggerDisplay("Count={Count}")]
    public sealed class ReadOnlyListA<TValue1, TValue2> : IList<TValue1> 
    {
        IItemSet<TValue1, TValue2> m_Set;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="items"></param>
        public ReadOnlyListA(IItemSet<TValue1, TValue2> items)
        {
            m_Set = items;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(TValue1 item)
        {
            return m_Set.IndexOfA(item);
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, TValue1 item)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Obtains the item at the specified index. 
        /// Setter throws a ReadOnlyException
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue1 this[int index]
        {
            get
            {
                return m_Set[index].A;
            }
            set
            {
                throw new ReadOnlyException();
            }
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="item"></param>
        public void Add(TValue1 item)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        public void Clear()
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Determines whether an element is part of the collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TValue1 item)
        {
            return IndexOf(item) > -1;
        }

        /// <summary>
        /// Copies the entire collection to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The index to start writing items at</param>
        public void CopyTo(TValue1[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            for (int i = 0; i < m_Set.Count; i++ )
            {
                array[arrayIndex++] = m_Set[i].A;
            }
        }

        /// <summary>
        /// Returns the number of elements present.
        /// </summary>
        public int Count
        {
            get { return m_Set.Count; }
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(TValue1 item)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TValue1> GetEnumerator()
        {
            return new EnumeratorA(m_Set);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EnumeratorA(m_Set);
        }

        class EnumeratorA : IEnumerator<TValue1>
        {
            IItemSet<TValue1, TValue2> m_Set;
            int m_Index = -1;

            public EnumeratorA(IItemSet<TValue1, TValue2> items)
            {
                m_Set = items;
            }

            public TValue1 Current
            {
                get { return m_Set[m_Index].A; }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return m_Set[m_Index].A; }
            }

            public bool MoveNext()
            {
                return (++m_Index < m_Set.Count);
            }

            public void Reset()
            {
                m_Index = -1;
            }
        }
    }

    /// <summary>
    /// Provides a readonly collection implementation for A items of <see cref="Set{A, B}"/>
    /// </summary>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    [DebuggerDisplay("Count={Count}")]
    public sealed class ReadOnlyListB<TValue1, TValue2> : IList<TValue2>
    {
        IItemSet<TValue1, TValue2> m_Set;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="items"></param>
        public ReadOnlyListB(IItemSet<TValue1, TValue2> items)
        {
            m_Set = items;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(TValue2 item)
        {
            return m_Set.IndexOfB(item);
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, TValue2 item)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Obtains the item at the specified index. 
        /// Setter throws a ReadOnlyException
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue2 this[int index]
        {
            get
            {
                return m_Set[index].B;
            }
            set
            {
                throw new ReadOnlyException();
            }
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="item"></param>
        public void Add(TValue2 item)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        public void Clear()
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Determines whether an element is part of the collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TValue2 item)
        {
            return m_Set.IndexOfB(item) > -1;
        }

        /// <summary>
        /// Copies the entire collection to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The index to start writing items at</param>
        public void CopyTo(TValue2[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            for (int i = 0; i < m_Set.Count; i++)
            {
                array[arrayIndex++] = m_Set[i].B;
            }
        }

        /// <summary>
        /// Returns the number of elements present.
        /// </summary>
        public int Count
        {
            get { return m_Set.Count; }
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Throws a ReadOnlyException
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(TValue2 item)
        {
            throw new ReadOnlyException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TValue2> GetEnumerator()
        {
            return new EnumeratorB(m_Set);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EnumeratorB(m_Set);
        }

        class EnumeratorB : IEnumerator<TValue2>
        {
            IItemSet<TValue1, TValue2> m_Set;
            int m_Index = -1;

            public EnumeratorB(IItemSet<TValue1, TValue2> items)
            {
                m_Set = items;
            }

            public TValue2 Current
            {
                get { return m_Set[m_Index].B; }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return m_Set[m_Index].B; }
            }

            public bool MoveNext()
            {
                return (++m_Index < m_Set.Count);
            }

            public void Reset()
            {
                m_Index = -1;
            }
        }
    }
}
