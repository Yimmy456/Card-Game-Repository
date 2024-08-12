using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameDataScript
{
    public bool _validSession;

    public SerializableDictionaryScript<string, bool> _flippingStati;

    public SerializableDictionaryScript<string, string> _cardItems;

    public SerializableDictionaryScript<string, Vector3> _cardPositions;

    public int _turns;

    public int _matches;

    public float _completionPercentage;

    public float _progressPercentage;

    public GameDataScript()
    {
        _validSession = false;

        _flippingStati = new SerializableDictionaryScript<string, bool>();

        _cardItems = new SerializableDictionaryScript<string, string>();

        _cardPositions = new SerializableDictionaryScript<string, Vector3>();

        _turns = 0;

        _matches = 0;

        _completionPercentage = 0.0f;

        _progressPercentage = 0.0f;
    }

    public void SaveSession(GameSessionScript _input)
    {
        _validSession = _input != null;

        if(!_validSession)
        {
            _flippingStati.Clear();

            _cardItems.Clear();

            _cardPositions.Clear();

            _turns = 0;

            _matches = 0;

            _completionPercentage = 0.0f;

            _progressPercentage = 0.0f;

            Debug.Log("There is nothing to save.");

            return;
        }

        CardScript _currentCard;

        for(int _i = 0; _i < _input.GetCards().Count; _i++)
        {
            _currentCard = _input.GetCards()[_i];

            _flippingStati.Add(_currentCard.GetCardID(), _currentCard.GetCardFinished());

            _cardItems.Add(_currentCard.GetCardID(), _currentCard.GetCardItem().GetItemID());

            _cardPositions.Add(_currentCard.GetCardID(), _currentCard.GetAssignedPosition());
        }

        _turns = _input.GetTurns();

        _matches = _input.GetMatches();

        _completionPercentage = _input.GetCompletionPercentage();

        _progressPercentage = _input.GetProgressPercentage();

        Debug.Log("The game is saved.");
    }
}
