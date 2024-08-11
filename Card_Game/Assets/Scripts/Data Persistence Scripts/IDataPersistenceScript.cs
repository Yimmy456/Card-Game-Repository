using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistenceScript
{
    void LoadData(GameDataScript _input);

    void SaveData(ref GameDataScript _input);
}
