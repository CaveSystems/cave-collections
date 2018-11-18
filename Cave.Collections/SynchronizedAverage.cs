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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cave.Collections
{
    /// <summary>
    /// Provides a synchronzation wrapper for <see cref="IAverage{T}"/> implementations
    /// </summary>
    /// <seealso cref="IAverage{T}" />
    public class SynchronizedAverage<T> : IAverage<T>
    {
        /// <summary>Initializes a new instance of the <see cref="SynchronizedAverage{T}"/> class.</summary>
        /// <param name="base">The base.</param>
        public SynchronizedAverage(IAverage<T> @base)
        {
            Base = @base;
        }

        /// <summary>Gets the base.</summary>
        /// <value>The base.</value>
        public IAverage<T> Base { get; }

        /// <summary>Gets the average for the current items.</summary>
        /// <value>The average.</value>
        public T Average { get { lock (Base) return Base.Average; } }

        /// <summary>Gets or sets the maximum item count.</summary>
        /// <value>The maximum count.</value>
        /// <remarks>
        /// Setting this to zero or negative values disables the maximum item count. An update is done after next call to <see cref="Add(T)" />.
        /// </remarks>
        public int MaximumCount { get { lock (Base) return Base.MaximumCount; } set { lock (Base) Base.MaximumCount = value; } }

        /// <summary>Gets the current item count.</summary>
        /// <value>The item count.</value>
        public int Count { get { lock (Base) return Base.Count; } }

        /// <summary>Adds the specified item.</summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            lock (Base) Base.Add(item);
        }

        /// <summary>Clears this instance.</summary>
        public void Clear()
        {
            lock (Base) Base.Clear();
        }

        /// <summary>Copies the collection to a static list and returns an enumerator that iterates through the list.</summary>
        /// <returns>An enumerator that can be used to iterate through the copy of the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (Base) return Base.ToList().GetEnumerator();
        }

        /// <summary>Copies the collection to a static list and returns an enumerator that iterates through the list.</summary>
        /// <returns>An enumerator that can be used to iterate through the copy of the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (Base) return Base.ToList().GetEnumerator();
        }
    }
}
