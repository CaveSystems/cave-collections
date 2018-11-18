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

namespace Cave.Collections
{
    /// <summary>
    /// Provides a default comparer
    /// </summary>
    public static class DefaultComparer
    {
        /// <summary>
        /// Compares two byte arrays as fast as possible using the native bit size.
        /// </summary>
        /// <param name="array1">The first byte array to check for equality</param>
        /// <param name="array2">The second byte array to check for equality</param>
        /// <returns>Returns true if the object equal each other</returns>
        public static unsafe bool Equals(byte[] array1, byte[] array2)
        {
            if (array1 == array2) return true;
            if (array1 == null || array2 == null || array1.Length != array2.Length) return false;
            int len = array1.Length;
            if (IntPtr.Size == 8)
            {
                fixed (byte* p1 = array1, p2 = array2)
                {
                    byte* x1 = p1, x2 = p2;
                    if (len > 7)
                    {
                        byte* e1 = x1 + len - 7;
                        for (; x1 < e1; x1 += 8, x2 += 8)
                        {
                            if (*((long*)x1) != *((long*)x2)) return false;
                        }
                    }
                    if ((len & 4) != 0)
                    {
                        if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4;
                    }
                    if ((len & 2) != 0)
                    {
                        if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2;
                    }
                    if ((len & 1) != 0)
                    {
                        if (*x1 != *x2) return false;
                    }
                    return true;
                }
            }
            else
            {
                fixed (byte* p1 = array1, p2 = array2)
                {
                    byte* x1 = p1, x2 = p2;
                    if (len > 3)
                    {
                        byte* e1 = x1 + len - 3;
                        for (; x1 < e1; x1 += 4, x2 += 4)
                        {
                            if (*((int*)x1) != *((int*)x2)) return false;
                        }
                    }
                    if ((len & 2) != 0)
                    {
                        if (*((short*)x1) != *((short*)x2)) return false;
                        x1 += 2; x2 += 2;
                    }
                    if ((len & 1) != 0)
                    {
                        if (*x1 != *x2) return false;
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// Compares two objects by type and if they are arrays item by item. Otherwise the objs1
        /// equals function is used to compare it to obj2.
        /// </summary>
        /// <param name="value1">The first <see cref="object"/> to check for equality</param>
        /// <param name="value2">The second <see cref="object"/> to check for equality</param>
        /// <returns>Returns true if the object equal each other</returns>
        public static new bool Equals(object value1, object value2)
        {
            if (ReferenceEquals(value1, value2)) return true;
            //check references
            if (ReferenceEquals(value1, null)) return ReferenceEquals(value2, null);
            if (ReferenceEquals(value2, null)) return false;
            //check type
            Type type1 = value1.GetType();
            if (type1 != value2.GetType()) return false;
            //is array ?
            if (value1 is Array)
            {
                return ItemsEqual(value1 as Array, value2 as Array);
            }
            //is IEnumerable
            if (value1 is IEnumerable)
            {
                return ItemsEqual(value1 as IEnumerable, value2 as IEnumerable);
            }
            //check equals
            return object.Equals(value1, value2);
        }

        /// <summary>
        /// Compares items of two arrays item by item without checking the type of the array.
        /// </summary>
        /// <param name="array1">The first array its items are compared</param>
        /// <param name="array2">The second array its items are compared</param>
        /// <returns>Returns true if all items in both array equal each other and the number of items equals, too</returns>
        public static bool Equals(IEnumerable array1, IEnumerable array2)
        {
            if (ReferenceEquals(array1, array2)) return true;

            if (ReferenceEquals(null, array1)) return ReferenceEquals(null, array2);
            if (ReferenceEquals(null, array2)) return false;

            return ItemsEqual(array1, array2);
        }

        /// <summary>
        /// Compares items of two arrays item by item without checking the type of the array.
        /// </summary>
        /// <param name="array1">The first array its items are compared</param>
        /// <param name="array2">The second array its items are compared</param>
        /// <returns>Returns true if all items in both array equal each other and the number of items equals, too</returns>
        public static bool ItemsEqual(IEnumerable array1, IEnumerable array2)
        {
            if (ReferenceEquals(array1, null)) throw new ArgumentNullException("array1");
            if (ReferenceEquals(array2, null)) throw new ArgumentNullException("array2");

            IEnumerator enumerator1 = array1.GetEnumerator();
            IEnumerator enumerator2 = array2.GetEnumerator();
            enumerator1.Reset();
            enumerator2.Reset();
            while (true)
            {
                bool moved1 = enumerator1.MoveNext();
                bool moved2 = enumerator2.MoveNext();
                if (moved1 != moved2) return false;
                if (!moved1) return true;
                if (!object.Equals(enumerator1.Current, enumerator2.Current)) return false;
            }
        }
    }
}
