using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides a list implementation for string parameters
    /// </summary>

    [DebuggerDisplay("Count={Count}")]
    public class ParameterCollection : IEnumerable<string>, IEquatable<ParameterCollection>
    {
        string[] m_Items;

        /// <summary>
        /// Creates a new empty ParameterCollection
        /// </summary>
        public ParameterCollection(params string[] items)
        {
            m_Items = items;
        }

        /// <summary>Ruft die Anzahl der Elemente ab, die in <see cref="T:System.Collections.Generic.ICollection`1" /> enthalten sind.</summary>
        public int Count => m_Items.Length;

        /// <summary>Ruft einen Wert ab, der angibt, ob <see cref="T:System.Collections.Generic.ICollection`1" /> schreibgesch�tzt ist.</summary>
        public bool IsReadOnly => true;

        /// <summary>Gets or sets the <see cref="string"/> at the specified index.</summary>
        /// <value>The <see cref="string"/>.</value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public string this[int index] => m_Items[index];

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ParameterCollection other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (Count != other.Count)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (!this[i].Equals(other[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Bestimmt den Index eines bestimmten Elements in der <see cref="T:System.Collections.Generic.IList`1" />.</summary>
        /// <param name="item">Das im <see cref="T:System.Collections.Generic.IList`1" /> zu suchende Objekt.</param>
        /// <returns>Der Index von <paramref name="item" />, wenn das Element in der Liste gefunden wird, andernfalls -1.</returns>
        int IndexOf(string item)
        {
            return Array.IndexOf(m_Items, item);
        }

        /// <summary>Bestimmt, ob <see cref="T:System.Collections.Generic.ICollection`1" /> einen bestimmten Wert enth�lt.</summary>
        /// <param name="item">Das im <see cref="T:System.Collections.Generic.ICollection`1" /> zu suchende Objekt.</param>
        /// <returns>
        /// true, wenn sich <paramref name="item" /> in <see cref="T:System.Collections.Generic.ICollection`1" /> befindet, andernfalls false.
        /// </returns>
        public bool Contains(string item)
        {
            return IndexOf(item) > -1;
        }

        /// <summary>
        /// Kopiert die Elemente von <see cref="T:System.Collections.Generic.ICollection`1" /> in ein <see cref="T:System.Array" />, beginnend bei einem bestimmten <see cref="T:System.Array" />-Index.
        /// </summary>
        /// <param name="array">Das eindimensionale <see cref="T:System.Array" />, das das Ziel der aus <see cref="T:System.Collections.Generic.ICollection`1" /> kopierten Elemente ist.F�r <see cref="T:System.Array" /> muss eine nullbasierte Indizierung verwendet werden.</param>
        /// <param name="arrayIndex">Der nullbasierte Index in <paramref name="array" />, an dem das Kopieren beginnt.</param>
        void CopyTo(string[] array, int arrayIndex)
        {
            m_Items.CopyTo(array, arrayIndex);
        }

        /// <summary>Gibt einen Enumerator zur�ck, der die Auflistung durchl�uft.</summary>
        /// <returns>Ein <see cref="T:System.Collections.Generic.IEnumerator`1" />, der zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)m_Items).GetEnumerator();
        }

        /// <summary>Gibt einen Enumerator zur�ck, der eine Auflistung durchl�uft.</summary>
        /// <returns>Ein <see cref="T:System.Collections.IEnumerator" />-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        /// <summary>
        /// Obtains a string containing all parameters
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (string parameter in this)
            {
                if (result.Length > 0)
                {
                    result.Append(' ');
                }

                bool containsSpace = parameter.IndexOf(' ') >= 0;
                if (containsSpace)
                {
                    result.Append('"');
                }
                result.Append(parameter);
                if (containsSpace)
                {
                    result.Append('"');
                }
            }
            return result.ToString();
        }
    }
}
