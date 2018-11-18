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

using Cave.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a option collection implementation
    /// </summary>
    [DebuggerDisplay("Count={Count}")]
    public class OptionCollection : IEnumerable<Option>, IEquatable<OptionCollection>
    {
        /// <summary>Parses the specified string.</summary>
        /// <param name="text">The string to parse.</param>
        /// <returns></returns>
        public static OptionCollection Parse(string text)
        {
            return FromStrings(StringExtensions.SplitNewLine(text));
        }

        /// <summary>
        /// Obtains an Array of <see cref="Option"/>s from a <see cref="IDictionary"/>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static OptionCollection FromDictionary(IDictionary dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            List<Option> opts = new List<Option>(); 
            foreach (DictionaryEntry entry in dictionary)
            {
                opts.Add(Option.FromDictionaryEntry(entry));
            }
            OptionCollection result = new OptionCollection(opts);
            return result;
        }

        /// <summary>Obtains an Array of <see cref="Option" />s from a specified string Array.</summary>
        /// <param name="lines">The strings to obtain Options from</param>
        /// <param name="ignoreInvalid">if set to <c>true</c> [ignore invalid options].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">texts</exception>
        public static OptionCollection FromStrings(string[] lines, bool ignoreInvalid = false)
        {
            if (lines == null) throw new ArgumentNullException("texts");
            List<Option> opts = new List<Option>();
            foreach (string line in lines)
            {
                if (ignoreInvalid && !Option.IsOption(line)) continue;
                opts.Add(Option.Parse(line));
            }
            OptionCollection result = new OptionCollection(opts);
            return result;
        }

        List<string, Option> m_Items = new List<string, Option>();

        /// <summary>Initializes a new empty instance of the <see cref="OptionCollection"/> class.</summary>
        public OptionCollection() { }

        /// <summary>
        /// Creates a new <see cref="OptionCollection"/>
        /// </summary>
        /// <param name="enumeration">The <see cref="IEnumerable"/> list of <see cref="Option"/>s</param>
        public OptionCollection(IEnumerable<Option> enumeration)
        {
            if (enumeration == null) throw new ArgumentNullException("enumeration");
            foreach (Option option in enumeration)
            {
                m_Items.Add(option.Name, option);
            }
        }

        /// <summary>
        /// Checks whether a specified option name is part of the collection
        /// </summary>
        /// <param name="optionName">Name of the option</param>
        /// <returns></returns>
        public bool Contains(string optionName)
        {
            if (optionName == null) throw new ArgumentNullException("optionName");
            return m_Items.ContainsA(optionName);
        }

        /// <summary>
        /// Obtains the index of the first option with the specified name.
        /// </summary>
        /// <param name="optionName">Name of the option</param>
        /// <returns>Returns the index of the first option or -1 if no option with the specified name can be found</returns>
        int IndexOf(string optionName)
        {
            if (optionName == null) throw new ArgumentNullException("optionName");
            if (Option.GetPrefix(optionName) != null) throw new ArgumentException("Do not prefix the optionname with an option prefix!");
            return m_Items.IndexOfA(optionName);
        }

        /// <summary>
        /// Obtains the index of the first option with the specified name.
        /// </summary>
        /// <param name="optionName">Name of the option</param>
        /// <param name="start">Start index to begin search at</param>
        /// <returns>Returns the index of the first option or -1 if no option with the specified name can be found</returns>
        int IndexOf(string optionName, int start)
        {
            if (optionName == null) throw new ArgumentNullException("optionName");
            if (Option.GetPrefix(optionName) != null) throw new ArgumentException("Do not prefix the optionname with an option prefix!");
            return m_Items.IndexOfA(optionName, start);
        }

        /// <summary>
        /// Obtains all option names
        /// </summary>
        public string[] Names
        {
            get
            {
                return m_Items.ItemsA;
            }
        }

        /// <summary>
        /// Allows direct access to the first<see cref="Option"/> with the specified name
        /// </summary>
        /// <param name="optionName">Name of the option</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Option this[string optionName]
        {
            get
            {
                int index = IndexOf(optionName);
                if (index < 0) throw new ArgumentOutOfRangeException(nameof(optionName));
                return this[index];
            }
        }

        /// <summary>
        /// Allows direct access to the first<see cref="Option"/> with the specified name
        /// </summary>
        /// <param name="optionIndex">Index of the option</param>
        /// <returns></returns>
        Option this[int optionIndex]
        {
            get { return m_Items.GetB(optionIndex); }
        }

        /// <summary>
        /// Obtains a string containing all options
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (Option option in this)
            {
                string optionString = option.ToString();
                if (result.Length > 0) result.Append(" ");
                bool containsSpace = (optionString.IndexOf(' ') > -1);
                if (containsSpace) result.Append('"');
                result.Append(optionString);
                if (containsSpace) result.Append('"');
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns an enumerator that iterates through all items
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Option> GetEnumerator()
        {
            return ((IEnumerable<Option>)m_Items.ItemsB).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through all items
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.ItemsB.GetEnumerator();
        }

        /// <summary>
        /// Determines whether the collection contains a specified element by using the default equality comparer. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Option item)
        {
            return m_Items.ContainsB(item);
        }

        /// <summary>
        /// Copies all elements of the collection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(Option[] array, int arrayIndex)
        {
            m_Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Obtains the number of items present.
        /// </summary>
        public int Count
        {
            get { return m_Items.Count; }
        }

        /// <summary>
        /// Returns false
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Obtains all options of the collection as one dimensional array
        /// </summary>
        /// <returns></returns>
        public Option[] ToArray()
        {
            Option[] result = new Option[Count];
            CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(OptionCollection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (Count != other.Count) return false;
            for (int i = 0; i < m_Items.Count; i++ )
            {
                if (!m_Items[i].Equals(other.m_Items[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object. (Inherited from Object.)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as OptionCollection);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_Items.GetHashCode();
        }
    }
}
