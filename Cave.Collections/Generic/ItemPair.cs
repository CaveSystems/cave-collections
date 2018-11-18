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

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a struct with two typed objects
    /// </summary>
    /// <typeparam name="T1">The type of the first object</typeparam>
    /// <typeparam name="T2">The type of the second object</typeparam>
    public class ItemPair<T1, T2>
    {
        /// <summary>
        /// Creates a new instance with the specified values
        /// </summary>
        /// <param name="value1">First value</param>
        /// <param name="value2">Second value</param>
        public ItemPair(T1 value1, T2 value2)
        {
            A = value1;
            B = value2;
        }

        /// <summary>
        /// Obtains the first value
        /// </summary>
        public T1 A { get; private set; }

        /// <summary>
        /// Obtains the second value
        /// </summary>
        public T2 B { get; private set; }

        /// <summary>
        /// Obtains a string "A B"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", A, B);
        }

        /// <summary>
        /// Obtains the hash code for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }

        /// <summary>
        /// Checks another ItemPair{T1, T2} for equality
        /// </summary>
        /// <param name="obj">The other instance to check</param>
        /// <returns>Returns true if the other instance equals this one, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ItemPair<T1, T2>)) return false;
            ItemPair<T1, T2> other = (ItemPair<T1, T2>)obj;
            return Equals(other.A, A) && Equals(other.B, B);
        }
    }
}
