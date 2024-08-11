using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionaryScript<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> _keys = new List<TKey>();

    [SerializeField]
    List<TValue> _values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();

        _values.Clear();

        foreach (KeyValuePair<TKey, TValue> _p in this)
        {
            _keys.Add(_p.Key);
            _values.Add(_p.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (_keys.Count != _values.Count)
        {
            return;
        }

        for (int _i = 0; _i < _keys.Count; _i++)
        {
            this.Add(_keys[_i], _values[_i]);
        }
    }
}
