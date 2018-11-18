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

using Cave.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cave.Collections
{
    /// <summary>
    /// Provides a simple moving average calculation
    /// </summary>
    /// <seealso cref="IAverage{T}" />
    public class ExpiringMovingAverageLong : IAverage<long>
    {
        LinkedList<C<DateTime, long>> items = new LinkedList<C<DateTime, long>>();
        long total;

        /// <summary>Gets the average for the current items.</summary>
        /// <value>The average.</value>
        public long Average { get { return total / items.Count; } }

        /// <summary>Gets or sets the maximum item count.</summary>
        /// <value>The maximum count.</value>
        /// <remarks>Setting this to zero or negative values disables the maximum item count. An update is done after next call to <see cref="Add(long)"/>.</remarks>
        public int MaximumCount { get; set; }

        /// <summary>Gets or sets the maximum age of the items.</summary>
        /// <value>The maximum age.</value>
        /// <remarks>Setting this to zero or negative values disables the maximum age. An update is done after next call to <see cref="Add(long)"/>.</remarks>
        public TimeSpan MaximumAge { get; set; }

        /// <summary>Gets the current item count.</summary>
        /// <value>The item count.</value>
        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>Adds the specified item.</summary>
        /// <param name="item">The item.</param>
        public void Add(long item)
        {
            items.AddLast(new C<DateTime, long>(DateTime.UtcNow, item));
            total += item;
            if (MaximumCount > 0)
            {
                while (items.Count > MaximumCount)
                {
                    total -= items.First.Value.V2;
                    items.RemoveFirst();
                }
            }
            if (MaximumAge > TimeSpan.Zero)
            {
                DateTime keepAfter = DateTime.UtcNow - MaximumAge;
                while (items.First.Value.V1 < keepAfter)
                {
                    total -= items.First.Value.V2;
                    items.RemoveFirst();
                }
            }
        }

        /// <summary>Clears this instance.</summary>
        public void Clear()
        {
            items.Clear();
            total = 0;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<long> GetEnumerator()
        {
            return items.Select(i => i.V2).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Select(i => i.V2).GetEnumerator();
        }
    }
}
