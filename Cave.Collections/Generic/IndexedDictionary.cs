using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cave.Collections.Generic
{
    /// <summary>
    /// Provides an indexed dictionary (a TKey, TValue dictionary supporting access to the KeyValuePair items by index)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>

    [DebuggerDisplay("Count={Count}")]
    public class IndexedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        Dictionary<TKey, TValue> m_Dictionary;
        List<TKey> m_Keys;

        #region IDictionary<T1, T2> implementation

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedDictionary{TKey, TValue}"/> class.
        /// </summary>
        public IndexedDictionary()
        {
            m_Dictionary = new Dictionary<TKey, TValue>();
            m_Keys = new List<TKey>();
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            m_Dictionary.Add(key, value);
            m_Keys.Add(key);
        }

        /// <summary>
        /// Determines whether the <see cref="Dictionary{TKey, TValue}"/> contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection containing the keys.
        /// </summary>
        public ICollection<TKey> Keys => m_Keys.AsReadOnly();

        /// <summary>
        /// Removes the value with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            return m_Dictionary.Remove(key) && m_Keys.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_Dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection containing the values.
        /// </summary>
        public ICollection<TValue> Values => m_Dictionary.Values;

        /// <summary>
        /// Gets/sets the value at the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get => m_Dictionary[key];
            set
            {
                if (m_Dictionary.ContainsKey(key))
                {
                    m_Dictionary[key] = value;
                    return;
                }
                Add(key, value);
            }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Determines whether the dictionary contains a specific key value combination.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return m_Dictionary.ContainsKey(item.Key) && Equals(item.Value, m_Dictionary[item.Key]);
        }

        /// <summary>
        /// Copies the elements of the dictionary (unsorted) to an array, starting at the specified array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            int i = arrayIndex;
            foreach (TKey k in m_Keys)
            {
                array[i++] = new KeyValuePair<TKey, TValue>(k, m_Dictionary[k]);
            }
        }

        /// <summary>
        /// Removes the value with the specified key from the dictionary.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return m_Dictionary.Remove(item.Key) && m_Keys.Remove(item.Key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the dictionary.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_Dictionary.GetEnumerator();
        }

        #endregion

        #region IList<T1> implementation
        /// <summary>
        /// Returns the zero-based index of the first occurrence of the specified value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int IndexOf(TKey key)
        {
            return m_Keys.IndexOf(key);
        }

        #endregion

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue GetValueAt(int index)
        {
            return m_Dictionary[m_Keys[index]];
        }

        /// <summary>
        /// Sets the value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetValueAt(int index, TValue value)
        {
            m_Dictionary[m_Keys[index]] = value;
        }

        /// <summary>
        /// Gets the key at the specified index.
        /// </summary>
        /// <param name="index">index to read</param>
        /// <returns>returns the key</returns>
        public TKey GetKeyAt(int index)
        {
            return m_Keys[index];
        }

        /// <summary>
        /// Gets the key and value at the specified index
        /// </summary>
        /// <param name="index">index to read</param>
        /// <param name="key">the key</param>
        /// <param name="value">the value</param>
        public void GetKeyValueAt(int index, out TKey key, out TValue value)
        {
            key = m_Keys[index];
            value = m_Dictionary[key];
        }

        /// <summary>
        /// Returns the number of elements in the dictionary.
        /// </summary>
        public int Count => m_Dictionary.Count;

        /// <summary>
        /// Removes all elements from the dictionary.
        /// </summary>
        public void Clear()
        {
            m_Dictionary.Clear();
            m_Keys.Clear();
        }

        /// <summary>
        /// Returns false
        /// </summary>
        public bool IsReadOnly => false;

        class Enumerator : IEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
        {
            IEnumerator<TKey> m_KeyEnumerator;
            IndexedDictionary<TKey, TValue> m_Dictionary;

            public Enumerator(IndexedDictionary<TKey, TValue> dictionary)
            {
                m_Dictionary = dictionary;
                m_KeyEnumerator = m_Dictionary.m_Keys.GetEnumerator();
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    TKey key = m_KeyEnumerator.Current;
                    return new KeyValuePair<TKey, TValue>(key, m_Dictionary[key]);
                }
            }

            public void Dispose()
            {
                m_KeyEnumerator.Dispose();
            }

            object IEnumerator.Current
            {
                get
                {
                    TKey key = m_KeyEnumerator.Current;
                    return new KeyValuePair<TKey, TValue>(key, m_Dictionary[key]);
                }
            }

            public bool MoveNext()
            {
                return m_KeyEnumerator.MoveNext();
            }

            public void Reset()
            {
                m_KeyEnumerator.Reset();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through all items.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}
