using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameSessionScript
{
    [SerializeField]
    List<CardScript> _cards;

    [SerializeField]
    List<CardItemClass> _selectedItems;

    [SerializeField]
    GameManagerScript _gameManager;

    float _completionPercentage;

    float _progressPercentage;

    int _turns = 0;

    int _matches = 0;

    int _difficultyLevel;

    public GameSessionScript()
    {
        _completionPercentage = 0.0f;

        _progressPercentage = 0.0f;

        _turns = 0;

        _matches = 0;

        _cards = new List<CardScript>();

        _selectedItems = new List<CardItemClass>();
    }

    public List<CardScript> GetCards()
    {
        return _cards;
    }

    public List <CardItemClass> GetSelectedItems()
    {
        return _selectedItems;
    }

    public float GetCompletionPercentage()
    {
        return _completionPercentage;
    }

    public float GetProgressPercentage()
    {
        return _progressPercentage;
    }

    public GameManagerScript GetGameManager()
    {
        return _gameManager;
    }

    public int GetTurns()
    {
        return _turns;
    }

    public int GetMatches()
    {
        return _matches;
    }

    public int GetDifficultyLevel()
    {
        return _difficultyLevel;
    }

    public void SetCards(List<CardScript> _input)
    {
        _cards = _input;
    }

    public void AddCard(CardScript _input)
    {
        _cards.Add(_input);
    }

    public void SetGameManager(GameManagerScript _input)
    {
        _gameManager = _input;
    }

    public void SetTurns(int _input)
    {
        _turns = _input;
    }

    public void SetMatches(int _input)
    {
        _matches = _input;
    }

    public void SetProgressPercentage(float _input)
    {
        _progressPercentage = _input;
    }

    public void SetCompletionPercentage(float _input)
    {
        _completionPercentage = _input;
    }

    public void IncTurns(int _input = 1)
    {
        _turns += _input;
    }

    public void IncMatches(int _input = 1)
    {
        _matches += _input;
    }

    public void SetDifficultyLevel(int _input)
    {
        if(!(_input >= 1 && _input <=4))
        {
            return;
        }

        _difficultyLevel = _input;
    }

    public void UpdateScore()
    {
        if(_turns <= 0)
        {
            _progressPercentage = 0.0f;
        }
        else
        {
            _progressPercentage = ((float)_matches) / ((float)_turns);

            _progressPercentage *= 100.0f;
        }

        if (_cards.Count > 0)
        {
            _completionPercentage = (_matches * 2.0f) / _cards.Count;

            _completionPercentage *= 100.0f;
        }
    }
}
