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
#endregion Authors & Contributors

namespace Cave.Collections.Generic
{
    /// <summary>Provides a binder class for 2 other objects</summary>
    /// <typeparam name="TValue1">The type of the value1.</typeparam>
    /// <typeparam name="TValue2">The type of the value2.</typeparam>
    public class C<TValue1, TValue2>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public C(TValue1 v1, TValue2 v2)
        {
            V1 = v1;
            V2 = v2;
        }

        /// <summary>
        /// Value 1
        /// </summary>
        public TValue1 V1 { get; private set; }

        /// <summary>
        /// Value 2
        /// </summary>
        public TValue2 V2 { get; private set; }

        /// <summary>
        /// Prints V1 + " " + V2
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return V1 + " " + V2;
        }

        /// <summary>
        /// Obtains the hash code of V1 ^ V2
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return V1.GetHashCode() ^ V2.GetHashCode();
        }

        /// <summary>
        /// Checks two instances for equality using the equality comparers for V1 and V2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            C<TValue1, TValue2> o = obj as C<TValue1, TValue2>;
            if (o == null) return false;
            return Equals(o.V1, V1) && Equals(o.V2, V2);
        }
    }

    /// <summary>Provides a binder class for 3 other objects</summary>
    /// <typeparam name="TValue1">The type of the value1.</typeparam>
    /// <typeparam name="TValue2">The type of the value2.</typeparam>
    /// <typeparam name="TValue3">The type of the value3.</typeparam>
    public class C<TValue1, TValue2, TValue3>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public C(TValue1 v1, TValue2 v2, TValue3 v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        /// <summary>
        /// Value 1
        /// </summary>
        public TValue1 V1 { get; private set; }

        /// <summary>
        /// Value 2
        /// </summary>
        public TValue2 V2 { get; private set; }

        /// <summary>
        /// Value 3
        /// </summary>
        public TValue3 V3 { get; private set; }

        /// <summary>
        /// Prints V1 + " " + V2 + " " + V3
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return V1 + " " + V2 + " " + V3;
        }

        /// <summary>
        /// Obtains the hash code of V1 ^ V2 ^ V3
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return V1.GetHashCode() ^ V2.GetHashCode() ^ V3.GetHashCode();
        }

        /// <summary>
        /// Checks two instances for equality using the equality comparers for V1 and V2 and V3
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            C<TValue1, TValue2, TValue3> o = obj as C<TValue1, TValue2, TValue3>;
            if (o == null) return false;
            return Equals(o.V1, V1) && Equals(o.V2, V2) && Equals(o.V3, V3);
        }
    }

    /// <summary>Provides a binder class for 4 other objects</summary>
    /// <typeparam name="TValue1">The type of the value1.</typeparam>
    /// <typeparam name="TValue2">The type of the value2.</typeparam>
    /// <typeparam name="TValue3">The type of the value3.</typeparam>
    /// <typeparam name="TValue4">The type of the value4.</typeparam>
    public class C<TValue1, TValue2, TValue3, TValue4>
    {
        /// <summary>Creates a new instance</summary>
        /// <param name="v1">The value 1.</param>
        /// <param name="v2">The value 2.</param>
        /// <param name="v3">The value 3.</param>
        /// <param name="v4">The value 4.</param>
        public C(TValue1 v1, TValue2 v2, TValue3 v3, TValue4 v4)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }

        /// <summary>
        /// Value 1
        /// </summary>
        public TValue1 V1 { get; private set; }

        /// <summary>
        /// Value 2
        /// </summary>
        public TValue2 V2 { get; private set; }

        /// <summary>
        /// Value 3
        /// </summary>
        public TValue3 V3 { get; private set; }

        /// <summary>
        /// Value 3
        /// </summary>
        public TValue4 V4 { get; private set; }

        /// <summary>
        /// Prints V1 + " " + V2 + " " + V3
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return V1 + " " + V2 + " " + V3 + " " + V4;
        }

        /// <summary>
        /// Obtains the hash code of V1 ^ V2 ^ V3
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return V1.GetHashCode() ^ V2.GetHashCode() ^ V3.GetHashCode() ^ V4.GetHashCode();
        }

        /// <summary>
        /// Checks two instances for equality using the equality comparers for V1 and V2 and V3
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            C<TValue1, TValue2, TValue3, TValue4> o = obj as C<TValue1, TValue2, TValue3, TValue4>;
            if (o == null) return false;
            return Equals(o.V1, V1) && Equals(o.V2, V2) && Equals(o.V3, V3) && Equals(o.V4, V4);
        }
    }
}
