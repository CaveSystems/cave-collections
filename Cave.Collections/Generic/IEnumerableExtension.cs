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
using System.Linq;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides extensions to the IEnumerable interface
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>Converts to a set.</summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IItemSet<T> AsSet<T>(this IEnumerable<T> items)
        {
            IItemSet<T> result = items as IItemSet<T>;
            if (result == null) result = new Set<T>(items);
            return result;
        }

        /// <summary>Converts to a set.</summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static IItemSet<T> ToSet<T>(this IEnumerable<T> items)
        {
            return new Set<T>(items);
        }

        /// <summary>Converts to a list.</summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static List<T> AsList<T>(this IEnumerable<T> items)
        {
            List<T> result =  items as List<T>;
            if (result == null) result = items.ToList();
            return result;
        }

        /// <summary>
        /// Allows to create an object array from any IEnumerable object
        /// </summary>
        /// <param name="source">The source IEnumerable type</param>
        /// <returns></returns>
        public static object[] ToObjectArray(this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            List<object> result = new List<object>();
            foreach (object item in source)
            {
                result.Add(item);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Allows to create a typed list from any IEnumerable object
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="source">The source IEnumerable type</param>
        /// <returns></returns>
        public static List<T> ConvertTo<T>(this IEnumerable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            List<T> result = new List<T>();
            foreach (T item in source)
            {
                result.Add(item);
            }
            return result;
        }
    }
}
