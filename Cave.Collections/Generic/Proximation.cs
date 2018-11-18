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

using System;
using System.Collections.Generic;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a basic moving average calculated with values based on a time axis (no continuous sampling needed)
    /// </summary>
    public class Proximation
    {
        class ProximationValue
        {
            public DateTime TimeStamp;
            public long Value;

            public ProximationValue(DateTime timeStamp, long value) { Value = value; TimeStamp = timeStamp; }

            public ProximationValue(long value) { Value = value; TimeStamp = DateTime.UtcNow; }
        }

        LinkedList<ProximationValue> m_Items = new LinkedList<ProximationValue>();

        /// <summary>
        /// Adds a value to the proximation
        /// </summary>
        /// <param name="value">The value to add</param>
        public void AddValue(long value)
        {
            m_Items.AddLast(new ProximationValue(value));
            if (value > Maximum) Maximum = value;
            else if (value < Minimum) Minimum = value;
        }

        /// <summary>Adds a value to the proximation</summary>
        /// <param name="timeStamp">The time stamp of the value.</param>
        /// <param name="value">The value to add</param>
        /// <exception cref="ArgumentOutOfRangeException">TimeStamp</exception>
        public void AddValue(DateTime timeStamp, long value)
        {
            if ((m_Items.Count > 0) && (m_Items.Last.Value.TimeStamp >= timeStamp)) throw new ArgumentOutOfRangeException(nameof(timeStamp));
            m_Items.AddLast(new ProximationValue(timeStamp, value));
            if (value > Maximum) Maximum = value;
            else if (value < Minimum) Minimum = value;
        }

        /// <summary>
        /// Clears all recorded values
        /// </summary>
        public void Clear()
        {
            m_Items.Clear();
        }

        /// <summary>
        /// Clears all values with a specified age or older
        /// </summary>
        /// <param name="age">The maximum age for values to keep</param>
        public void ClearOlderThan(TimeSpan age)
        {
            DateTime earliest = EndTime - age;
            while ((m_Items.Count > 0) && (m_Items.First.Value.TimeStamp < earliest)) m_Items.RemoveFirst();
        }

        /// <summary>
        /// Gets the maximum value
        /// </summary>
        public long Maximum { get; private set; }

        /// <summary>
        /// Gets the minimum value
        /// </summary>
        public long Minimum { get; private set; }

        /// <summary>
        /// Obtains the (local) datetime of the first recorded value
        /// </summary>
        public DateTime StartTime { get { return (m_Items.Count == 0) ? default(DateTime) : m_Items.First.Value.TimeStamp; } }

        /// <summary>
        /// Obtains the (local) datetime of the last recorded value
        /// </summary>
        public DateTime EndTime { get { return (m_Items.Count == 0) ? default(DateTime) : m_Items.First.Value.TimeStamp; } }

        /// <summary>
        /// Obtains the duration between StartTime and EndTime.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        /// <summary>
        /// Obtains the current moving average
        /// </summary>
        public long Average
        {
            get
            {
                if (m_Items.Count == 0) return 0;
                long result = 0;
                foreach(ProximationValue i in m_Items)
                {
                    result += i.Value;
                }
                return result / m_Items.Count;
            }
        }

        /// <summary>
        /// Obtains the weighted average of the whole recorded timeline. Startweight is 0% and endweight is 100%.
        /// </summary>
        public long WeightedAverage
        {
            get
            {
                if (m_Items.Count == 0) return 0;
                long duration = Duration.Ticks;
                double result = 0;
                double div = 0;
                foreach (ProximationValue i in m_Items)
                {
                    long pos = (i.TimeStamp - StartTime).Ticks;
                    long weight = duration / pos;
                    result += i.Value * weight;
                    div += weight;
                }
                return (long)(result / div);
            }
        }

        /// <summary>
        /// Obtains the reverse weighted average of the whole recorded timeline. Startweight is 100% and endweight is 0%.
        /// </summary>
        public long ReverseWeightedAverage
        {
            get
            {
                if (m_Items.Count == 0) return 0;
                long duration = Duration.Ticks;
                double result = 0;
                double div = 0;
                foreach (ProximationValue i in m_Items)
                {
                    long pos = (i.TimeStamp - StartTime).Ticks;
                    long weight = pos / duration;
                    result += i.Value * weight;
                    div += weight;
                }
                return (long)(result / div);
            }
        }
    }
}
