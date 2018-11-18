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

namespace Cave.Collections
{
    /// <summary>
    /// Provides an <see cref="IEnumerator"/> implementation for simple integer counting
    /// </summary>
    public class CountEnumerator : IEnumerator<int>, IEnumerator
    {
        Counter m_Counter;

        /// <summary>
        /// Creates a new <see cref="CountEnumerator"/>.
        /// </summary>
        /// <param name="counter">The <see cref="Counter"/> used to create values to be enumerated</param>
        public CountEnumerator(Counter counter)
        {
            m_Counter = counter;
        }

        /// <summary>
        /// Creates a new <see cref="CountEnumerator"/> with an initial start value.
        /// </summary>
        /// <param name="start">The first value to be enumerated</param>
        public CountEnumerator(int start)
            : this(new Counter(start))
        { }

        /// <summary>
        /// Creates an new <see cref="CountEnumerator"/> with an initial start value and a maximum count.
        /// </summary>
        /// <param name="start">The first value to be enumerated</param>
        /// <param name="count">The value count to be enumerated</param>
        public CountEnumerator(int start, int count)
            : this(new Counter(start, count))
        { }

        #region IEnumerator<T> Member

        /// <summary>
        /// Obtains the the curent value
        /// </summary>
        public int Current { get { return m_Counter.Current; } }

        #endregion

        #region IEnumerator Member

        /// <summary>
        /// Obtains the the curent value
        /// </summary>
        object IEnumerator.Current { get { return m_Counter.Current; } }

        /// <summary>
        /// Moves to the next value
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() { return m_Counter.MoveNext(); }

        /// <summary>
        /// Resets the <see cref="CountEnumerator"/>
        /// </summary>
        public void Reset() { m_Counter.Reset(); }

        #endregion

        #region IDisposable Member

        /// <summary>Releases the unmanaged resources used by this instance and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) { }

        /// <summary>
        /// Frees all used resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
