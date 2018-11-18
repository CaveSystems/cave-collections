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
using System.Text;

namespace Cave.Collections
{
    /// <summary>
    /// Provides a simple integer range.
    /// </summary>
    public class Range : IEnumerable
    {
        #region operators

        /// <summary>Implements the operator ==.</summary>
        /// <param name="range1">The range1.</param>
        /// <param name="range2">The range2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Range range1, Range range2)
        {
            if (ReferenceEquals(null, range1)) return ReferenceEquals(null, range2);
            if (ReferenceEquals(null, range2)) return false;
            return range1.AllValuesString == range2.AllValuesString;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="range1">The range1.</param>
        /// <param name="range2">The range2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Range range1, Range range2)
        {
            if (ReferenceEquals(null, range1)) return !ReferenceEquals(null, range2);
            if (ReferenceEquals(null, range2)) return true;
            return range1.AllValuesString != range2.AllValuesString;
        }

        /// <summary>
        /// Adds two <see cref="Range"/>s
        /// </summary>
        /// <param name="range1"></param>
        /// <param name="range2"></param>
        /// <returns></returns>
        public static Range operator +(Range range1, Range range2)
        {
            if (range1 == null) throw new ArgumentNullException("range1");
            if (range2 == null) throw new ArgumentNullException("range2");
            Range result = new Range(Math.Min(range1.Minimum, range2.Minimum), Math.Max(range1.Maximum, range2.Maximum));
            result.Add(range1);
            result.Add(range2);
            return result;
        }
        #endregion

        #region static functionality
        /// <summary>
        /// Parses a <see cref="Range"/> from a specified string
        /// </summary>
        /// <param name="text">A <see cref="Range"/> string</param>
        /// <param name="min">Minimum value of the <see cref="Range"/></param>
        /// <param name="max">Maximum value of the <see cref="Range"/></param>
        /// <returns></returns>
        public static Range Parse(string text, int min, int max)
        {
            Range result = new Range(min, max);
            result.Parse(text);
            return result;
        }
        #endregion

        #region private functionality
        List<Counter> m_Counters = new List<Counter>();
        string m_CurrentString = null;
        string m_AllValuesString = "*";
        char m_RangeSeparator = '-';
        char m_ValueSeparator = ',';
        char m_RepetitionSeparator = '/';

        Counter m_ParseRangePart(string text, int minValue, int maxValue)
        {
            try
            {
                if (text.IndexOf(RangeSeparator) > -1)
                {
                    string[] parts = text.Split(RangeSeparator);
                    if (parts.Length != 2) throw new ArgumentException(string.Format("Expected 'start-end'!"), "text");
                    int start = int.Parse(parts[0]);
                    if (start < minValue) throw new ArgumentOutOfRangeException(nameof(minValue));
                    return Counter.Create(start, int.Parse(parts[1]));
                }
                if (text.IndexOf(RepetitionSeparator) > -1)
                {
                    string[] parts = text.Split(RepetitionSeparator);
                    if (parts.Length != 2) throw new ArgumentException("Expected 'start/repetition'!", "text");
                    if (parts[0] == AllValuesString)
                    {
                        return Counter.Create(minValue, maxValue, int.Parse(parts[1]));
                    }
                    int start = int.Parse(parts[0]);
                    if (start < minValue) throw new ArgumentOutOfRangeException(nameof(minValue));
                    return Counter.Create(start, maxValue, int.Parse(parts[1]));
                }
                if (text == AllValuesString)
                {
                    return null;
                }
                {
                    int start = int.Parse(text);
                    if (start < minValue) throw new ArgumentOutOfRangeException(nameof(minValue));
                    return new Counter(start, 1);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Invalid range string '{0}'.", text), "text", ex);
            }
        }

        Counter[] m_ParseRange(string text, int minValue, int maxValue)
        {
            bool allValueRangePart = false;
            List<Counter> result = new List<Counter>();
            string[] parts = text.Split(new char[] { ValueSeparator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                Counter counter = m_ParseRangePart(part, minValue, maxValue);
                if (counter == null)
                {
                    allValueRangePart = true;
                }
                else
                {
                    if (allValueRangePart) throw new ArgumentException(string.Format("You may not add an all-value-range and a normal range!"));
                    result.Add(counter);
                }
            }
            return result.ToArray();
        }
        #endregion

        #region public functionality
        /// <summary>
        /// Gets / sets the all values string
        /// </summary>
        public string AllValuesString
        {
            get { return m_AllValuesString; }
            set { m_AllValuesString = value; m_CurrentString = null; }
        }

        /// <summary>
        /// Gets / sets the range separator
        /// </summary>
        public char RangeSeparator
        {
            get { return m_RangeSeparator; }
            set { m_RangeSeparator = value; m_CurrentString = null; }
        }

        /// <summary>
        /// Gets / sets the value separator
        /// </summary>
        public char ValueSeparator
        {
            get { return m_ValueSeparator; }
            set { m_ValueSeparator = value; m_CurrentString = null; }
        }

        /// <summary>
        /// Gets / sets the repetition separator
        /// </summary>
        public char RepetitionSeparator
        {
            get { return m_RepetitionSeparator; }
            set { m_RepetitionSeparator = value; m_CurrentString = null; }
        }

        /// <summary>
        /// Obtains the minimum of the <see cref="Range"/>
        /// </summary>
        public int Minimum { get; }

        /// <summary>
        /// Obtains the maximum of the <see cref="Range"/>
        /// </summary>
        public int Maximum { get; }

        /// <summary>
        /// Creates a new full range with the specified minimum and maximum
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Range(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }

        /// <summary>
        /// Parses a string and sets all properties of the <see cref="Range"/> to the parsed values.
        /// </summary>
        /// <param name="text"></param>
        public void Parse(string text)
        {
            m_Counters.Clear();
            Counter[] counters = m_ParseRange(text, Minimum, Maximum);
            foreach (Counter counter in counters)
            {
                Add(counter);
            }
        }

        /// <summary>
        /// Adds a value to this <see cref="Range"/>
        /// </summary>
        /// <param name="value"></param>
        public void Add(int value)
        {
            if (Contains(value)) return;
            m_Counters.Add(new Counter(value, 1));
        }

        /// <summary>
        /// Adds a <see cref="Counter"/> to this <see cref="Range"/>
        /// </summary>
        /// <param name="counter"></param>
        public void Add(Counter counter)
        {
            if ((m_Counters.Count > 0) && Contains(counter)) return;
            m_CurrentString = null;
            m_Counters.Add(counter);
        }

        /// <summary>
        /// Adds a <see cref="Range"/> to this <see cref="Range"/>
        /// </summary>
        /// <param name="range"></param>
        public void Add(Range range)
        {
            if (range == null) throw new ArgumentNullException("range");
            m_CurrentString = null;
            foreach (Counter c in range.m_Counters)
            {
                if (!Contains(c))
                {
                    m_Counters.Add(c);
                }
            }
        }

        /// <summary>
        /// Checks whether a specified value is part of the <see cref="Range"/> or not
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(int value)
        {
            if (m_Counters.Count == 0)
            {
                return (value >= Minimum) && (value <= Maximum);
            }
            foreach (Counter counter in m_Counters)
            {
                if (counter.Contains(value)) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether a specified <see cref="Counter"/> is part of the <see cref="Range"/> or not
        /// </summary>
        /// <param name="counter"></param>
        /// <returns></returns>
        public bool Contains(Counter counter)
        {
            if (counter == null) throw new ArgumentNullException("counter");
            if (m_Counters.Count == 0)
            {
                return Contains(counter.Start) && Contains(counter.End);
            }
            foreach (Counter c in m_Counters)
            {
                if (counter.Contains(c)) return true;
            }
            foreach (Counter c in counter)
            {
                if (Contains(c)) return true;
            }
            return false;
        }

        /// <summary>
        /// Obtains the counter properties as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (m_CurrentString != null) return m_CurrentString;
            if (m_Counters.Count == 0) return AllValuesString;
            m_Counters.Sort();
            StringBuilder result = new StringBuilder();
            foreach (Counter counter in m_Counters)
            {
                if (result.Length > 0) result.Append(ValueSeparator);
                if (counter.Start < Minimum) return string.Format("Invalid");
                if (counter.Count > 1)
                {
                    if (counter.Step != 1)
                    {
                        if (counter.Start == Minimum) return AllValuesString + RepetitionSeparator + counter.Step;
                        result.Append(counter.Start.ToString() + RepetitionSeparator + counter.Step.ToString());
                        continue;
                    }
                    result.Append(counter.Start.ToString() + RangeSeparator + counter.End.ToString());
                    continue;
                }
                result.Append(counter.Start.ToString());
            }
            m_CurrentString = result.ToString();
            return m_CurrentString;
        }

        /// <summary>
        /// Obtains a hash code for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Checks two ranges for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Range other = (obj as Range);
            if (ReferenceEquals(other, null)) return false;
            return ToString().Equals(other.ToString());
        }

        #endregion

        #region IEnumerable Member

        class RangeEnumerator : IEnumerator
        {
            Range m_Range;
            long m_Current;

            public RangeEnumerator(Range range)
            {
                m_Range = range;
                Reset();
            }

            #region IEnumerator Member

            public object Current
            {
                get
                {
                    if (m_Current < m_Range.Minimum) throw new InvalidOperationException(string.Format("Invalid operation, use MoveNext() first!"));
                    if (m_Current > m_Range.Maximum) throw new InvalidOperationException(string.Format("Invalid operation, moved out of range!"));
                    return (int)m_Current;
                }
            }

            public bool MoveNext()
            {
                if (m_Current > m_Range.Maximum) throw new InvalidOperationException(string.Format("Invalid operation, use Reset() first!"));
                while (!m_Range.Contains((int)++m_Current))
                {
                    if (m_Current > m_Range.Maximum) return false;
                }
                return true;
            }

            public void Reset()
            {
                m_Current = m_Range.Minimum - 1L;
            }

            #endregion
        }

        /// <summary>
        /// Obtains an <see cref="IEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return new RangeEnumerator(this);
        }

        #endregion
    }
}
