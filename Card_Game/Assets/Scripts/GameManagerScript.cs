using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    static GameManagerScript _instance;

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

    [SerializeField]
    CardScript _cardTemplate;

    [SerializeField]
    Transform _cardSpaceTr;

    Vector3 _position = Vector3.zero;

    Vector3 _basePosition = Vector3.zero;

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

        //DontDestroyOnLoad(gameObject);

        StartGame();
    }

    public static GameManagerScript GetInstance()
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
        if(!(_input >= 1 && _input <= 4))
        {
            return;
        }

        _difficultyLevel = _input;
    }

    public void SetPlayingCanvas(PlayingCanvasScript _input)
    {
        _playingCanvas = _input;
    }

    public void AddSelectedItem(CardItemClass _input)
    {
        _selectedItems.Add(_input);
    }

    public void ClearGame()
    {
        foreach(CardScript _card in _cards)
        {
            Destroy(_card.gameObject);
        }

        _cards.Clear();

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

        if(_card1Input.GetCardItem().GetItemName() == _card2Input.GetCardItem().GetItemName())
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

    void StartGame()
    {
        //Certain variables MUST be assigned before proceeding in creating the game.

        if(_cardTemplate == null || _camera == null || _cardSpaceTr == null)
        {
            return;
        }

        int _choices = ItemsManagerScript.GetInstance().GetCardItems().Count;

        int _randIndex = 0;

        CardItemClass _selectedItem;

        CardScript _cardProperties;

        GameObject _instCard;

        int _numberOfCards = GetNumberOfCards();

        while (_cards.Count < _numberOfCards)
        {
            _randIndex = Random.Range(0, _choices);

            //Selecting any random tiem from the "Items" list from the "Items Manager" script.

            _selectedItem = ItemsManagerScript.GetInstance().GetCardItems()[_randIndex];

            //The current iteration will be skipped if the selected item is already in the "Selected Items" list. This is to ensure that each distinct item is selected once.

            if(_selectedItems.Contains(_selectedItem) || _selectedItem.GetItemSprite() == null)
            {
                continue;
            }

            _selectedItems.Add(_selectedItem);

            for (int _i = 0; _i < 2; _i++)
            {
                _instCard = Instantiate(_cardTemplate.gameObject);

                _cardProperties = _instCard.GetComponent<CardScript>();

                _cardProperties.GetRenderer().sprite = _selectedItem.GetItemSprite();

                _cardProperties.SetCamera(_camera);

                _cardProperties.SetManager(this);

                _cards.Add(_cardProperties);

                _instCard.transform.parent = _cardSpaceTr.transform;

                _instCard.transform.localPosition = _position;

                SetNextCardPosition();
            }
        }
    }

    int GetNumberOfCards()
    {
        switch (_difficultyLevel)
        {
            case 2:
                _position = new Vector3(-7.5f, 5.0f, 0.0f);

                _basePosition = _position;

                return 12;
            case 3:
                _position = new Vector3(-7.5f, 15.0f, 0.0f);

                _basePosition = _position;

                return 16;
            case 4:
                _position = new Vector3(-10.0f, 15.0f, 0.0f);

                _basePosition = _position;

                return 20;
            default:
                _position = new Vector3(-5.0f, 5.0f, 0.0f);

                _basePosition = _position;

                return 8;
        }
    }

    void SetNextCardPosition()
    {
        if(_difficultyLevel == 4)
        {
            if((_cards.Count % 5) == 0)
            {
                _position.x = _basePosition.x;

                _position.y -= _basePosition.y;
            }
            else
            {
                _position.x -= _basePosition.x;
            }
        }
        else
        {
            if((_cards.Count % 4) == 0)
            {
                _position.x = _basePosition.x;

                _position.y -= _basePosition.y;
            }
            else
            {
                _position.x -= _basePosition.x;
            }
        }
    }
}
