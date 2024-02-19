using System;
using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// This is a serializable dictionary.
    /// </summary>
    [Serializable]
    public sealed class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        KeyValuePair[] data;

        /// <inheritdoc/>
        public void OnBeforeSerialize()
        {
            data = new KeyValuePair[Count];
            int index = 0;
            foreach (var kv in this)
            {
                data[index++] = new KeyValuePair(kv.Key, kv.Value);
            }
        }

        /// <inheritdoc/>
        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var kv in data)
            {
                Add(kv.Key, kv.Value);
            }

            data = null;
        }

        [Serializable]
        struct KeyValuePair
        {
            [SerializeField]
            public TKey Key;

            [SerializeField]
            public TValue Value;

            public KeyValuePair(TKey k, TValue v)
            {
                Key = k;
                Value = v;
            }
        }
    }
}