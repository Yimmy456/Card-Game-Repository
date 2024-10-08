using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionsScript : MonoBehaviour
{
    public void LoadScene(string _input)
    {
        SceneManager.LoadScene(_input);
    }

    public void LoadScene(int _input)
    {
        SceneManager.LoadScene(_input);
    }

    public void SaveGame(GameManagerScript _input)
    {
        if(_input == null || DataPersistenceManagerScript.GetInstance() == null)
        {
            return;
        }

        if(_input.GetGameSession() == null)
        {
            return;
        }

        Debug.Log("The game is being saved...");

        DataPersistenceManagerScript.GetInstance().GetGameData().SaveSession(_input.GetGameSession());
    }

    public void SetLoadGame(bool _input)
    {
        if(DataPersistenceManagerScript.GetInstance() == null)
        {
            return;
        }

        DataPersistenceManagerScript.GetInstance().SetLoadGame(_input);
    }

    public void ClearPersistence()
    {
        if(DataPersistenceManagerScript.GetInstance() == null)
        {
            return;
        }

        DataPersistenceManagerScript.GetInstance().GetGameData()._flippingStati.Clear();

        DataPersistenceManagerScript.GetInstance().GetGameData()._cardItems.Clear();

        DataPersistenceManagerScript.GetInstance().GetGameData()._cardPositions.Clear();
    }

    public void ChangeCardSize(Dropdown _input)
    {
        if(ItemsManagerScript.GetInstance() == null || _input == null)
        {
            return;
        }

        int _number = _input.value;

        CardSizeEnum _val = (CardSizeEnum)_number;

        ItemsManagerScript.GetInstance().SetCardSize(_val);
    }
}
