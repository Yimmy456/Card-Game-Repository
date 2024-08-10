using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    GameManagerScript _instance;

    [SerializeField]
    List<CardItemClass> _selectedItems;

    [SerializeField]
    List<CardScript> _cards;

    [SerializeField]
    PlayingCanvasScript _playingCanvas;

    [SerializeField]
    float _rotationAnimationSpeed = 5.0f;

    [SerializeField]
    Camera _camera;

    CardScript _card1;

    CardScript _card2;

    int _difficultyLevel = 1;

    int _turns = 0;

    int _matches = 0;

    Coroutine _rotationAnimationFCoroutine1;

    Coroutine _rotationAnimationBCoroutine1;

    Coroutine _rotationAnimationFCoroutine2;

    Coroutine _rotationAnimationBCoroutine2;

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

    public List<CardScript> GetCards()
    {
        return _cards;
    }

    public int GetDifficultyLevel()
    {
        return _difficultyLevel;
    }

    public int GetTurns()
    {
        return _turns;
    }

    public int GetMatches()
    {
        return _matches;
    }

    public PlayingCanvasScript GetPlayingCanvas()
    {
        return _playingCanvas;
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

    public void EvaluateCards(CardScript _cardInput)
    {
        if(_card1 == null)
        {
            _card1 = _cardInput;

            _card1.SetCardFlipped(true);

            _rotationAnimationFCoroutine1 = StartCoroutine(RotateCardForward(_card1));
        }
        else if(_card2 == null)
        {
            _card2 = _cardInput;

            _card2.SetCardFlipped(true);

            _rotationAnimationFCoroutine2 = StartCoroutine(RotateCardForward(_card2));

            StartCoroutine(MatchCards(_card1, _card2));

            _turns++;

            _playingCanvas.GetTurnsCountText().text = _turns.ToString();

            _card1 = null;

            _card2 = null;
        }
    }

    IEnumerator MatchCards(CardScript _card1Input, CardScript _card2Input)
    {
        yield return new WaitForSeconds(1.0f);

        if(_rotationAnimationFCoroutine1 != null)
        {
            StopCoroutine(_rotationAnimationFCoroutine1);
        }

        if(_rotationAnimationFCoroutine2 != null)
        {
            StopCoroutine(_rotationAnimationFCoroutine2);
        }

        if(_card1Input.GetCardItem() == _card2Input.GetCardItem())
        {
            _card1Input.SetCardFinished(true);

            _card2Input.SetCardFinished(true);

            _card1Input.gameObject.SetActive(false);

            _card2Input.gameObject.SetActive(false);

            _matches++;

            _playingCanvas.GetMatchCountText().text = _matches.ToString();
        }
        else
        {
            _card1Input.SetCardFlipped(false);

            _card2Input.SetCardFlipped(false);

            _rotationAnimationBCoroutine1 = StartCoroutine(RotateCardBack(_card1Input));

            _rotationAnimationBCoroutine2 = StartCoroutine(RotateCardBack(_card2Input));
        }
    }

    IEnumerator RotateCardForward(CardScript _cardInput)
    {
        float _rotationSpeed = Time.deltaTime * _rotationAnimationSpeed;

        Vector3 _currentRot = Vector3.zero;

        Quaternion _q = Quaternion.identity;

        for(float _t = 0.0f; _t < 180.0f; _t += _rotationSpeed)
        {
            if(_t >= 180.0f)
            {
                _t = 180.0f;
            }

            _currentRot.y = _t;

            _q = Quaternion.Euler(_currentRot);

            _cardInput.gameObject.transform.rotation = _q;

            yield return null;
        }
    }

    IEnumerator RotateCardBack(CardScript _cardInput)
    {
        float _rotationSpeed = Time.deltaTime * _rotationAnimationSpeed;

        Vector3 _currentRot = Vector3.zero;

        Quaternion _q = Quaternion.identity;

        for(float _t = 180.0f; _t > 0.0f; _t -= _rotationSpeed)
        {
            if(_t <= 0.0f)
            {
                _t = 0.0f;
            }

            _currentRot.y = _t;

            _q = Quaternion.Euler(_currentRot);

            _cardInput .gameObject.transform.rotation = _q;

            yield return null;
        }
    }
}
