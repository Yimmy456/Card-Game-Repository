using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    GameManagerScript _instance;

    [SerializeField]
    List<CardItemClass> _selectedItems;

    int _difficultyLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public GameManagerScript GetInstance()
    {
        return _instance;
    }

    public List<CardItemClass> GetSelectedItems()
    {
        return _selectedItems;
    }

    public int GetDifficultyLevel()
    {
        return _difficultyLevel;
    }

    public void SetDifficultyLevel(int _input)
    {
        if(!(_input >= 1 && _input <= 5))
        {
            return;
        }

        _difficultyLevel = _input;
    }

    public void AddSelectedItem(CardItemClass _input)
    {
        _selectedItems.Add(_input);
    }

    public void ClearGame()
    {
        _selectedItems.Clear();
    }
}
