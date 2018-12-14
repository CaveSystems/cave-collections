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
        public int Current => m_Counter.Current;

        #endregion

        #region IEnumerator Member

        /// <summary>
        /// Obtains the the curent value
        /// </summary>
        object IEnumerator.Current => m_Counter.Current;

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
