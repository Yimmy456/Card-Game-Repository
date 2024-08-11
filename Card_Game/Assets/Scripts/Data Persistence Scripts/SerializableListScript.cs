using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableListScript<T> : List<T>, ISerializationCallbackReceiver
{
    List<T> _list = new List<T>();

    public void OnBeforeSerialize()
    {
        _list.Clear();

        foreach(T _v in this)
        {
            _list.Add(_v);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for(int _i = 0; _i < _list.Count; _i++)
        {
            this.Add(_list[_i]);
        }
    }
}
