using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    static GameManagerScript _instance;

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

    [SerializeField]
    float _distanceScale = 1.0f;

    [SerializeField]
    AudioClip _flipClip;

    [SerializeField]
    AudioClip _correctClip;

    [SerializeField]
    AudioClip _wrongClip;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    GameSessionScript _gameSession;

    Vector3 _position = Vector3.zero;

    Vector3 _basePosition = Vector3.zero;

    CardScript _card1;

    CardScript _card2;

    int _difficultyLevel = 1;

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

        if (ItemsManagerScript.GetInstance() != null)
        {
            _difficultyLevel = ItemsManagerScript.GetInstance().GetDifficulty();
        }

        bool _load = DataPersistenceManagerScript.GetInstance().GetLoadSession();

        if (_load)
        {
            LoadGame();
        }
        else
        {
            StartNewGame();
        }
    }

    void Update()
    {
        if (_gameSession != null)
        {
            UpdateScores();

            if(_gameSession.GetCompletionPercentage() == 100.0f)
            {
                DataPersistenceManagerScript.GetInstance().ClearGameData();

                SceneManager.LoadScene("Game Over Scene");
            }
        }
    }

    public static GameManagerScript GetInstance()
    {
        return _instance;
    }

    public int GetDifficultyLevel()
    {
        return _difficultyLevel;
    }

    public PlayingCanvasScript GetPlayingCanvas()
    {
        return _playingCanvas;
    }

    public GameSessionScript GetGameSession()
    {
        return _gameSession;
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

    public void EvaluateCards(CardScript _cardInput)
    {
        if(_cardInput.GetCardFlipped() || _cardInput.GetCardFinished())
        {
            return;
        }

        if (_card1 == null)
        {
            _card1 = _cardInput;

            _card1.SetCardFlipped(true);

            _rotationAnimationFCoroutine1 = StartCoroutine(RotateCardForward(_card1));

            PlayAudio(_flipClip);
        }
        else if(_card2 == null)
        {
            _card2 = _cardInput;

            _card2.SetCardFlipped(true);

            _rotationAnimationFCoroutine2 = StartCoroutine(RotateCardForward(_card2));

            PlayAudio(_flipClip);

            StartCoroutine(MatchCards(_card1, _card2));

            _gameSession.IncTurns();

            _playingCanvas.GetTurnsCountText().text = _gameSession.GetTurns().ToString();

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

            _gameSession.IncMatches();

            _playingCanvas.GetMatchCountText().text = _gameSession.GetMatches().ToString();

            PlayAudio(_correctClip);
        }
        else
        {
            _card1Input.SetCardFlipped(false);

            _card2Input.SetCardFlipped(false);

            _rotationAnimationBCoroutine1 = StartCoroutine(RotateCardBack(_card1Input));

            _rotationAnimationBCoroutine2 = StartCoroutine(RotateCardBack(_card2Input));

            PlayAudio(_wrongClip);
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

    void StartNewGame()
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

        _gameSession = new GameSessionScript();

        _gameSession.SetGameManager(this);

        string _newCardID;

        while (_gameSession.GetCards().Count < _numberOfCards)
        {
            _randIndex = Random.Range(0, _choices);

            //Selecting any random tiem from the "Items" list from the "Items Manager" script.

            _selectedItem = ItemsManagerScript.GetInstance().GetCardItems()[_randIndex];

            //The current iteration will be skipped if the selected item is already in the "Selected Items" list. This is to ensure that each distinct item is selected once.

            if(_gameSession.GetSelectedItems().Contains(_selectedItem) || _selectedItem.GetItemSprite() == null)
            {
                continue;
            }

            _gameSession.GetSelectedItems().Add(_selectedItem);

            for (int _i = 0; _i < 2; _i++)
            {
                _instCard = Instantiate(_cardTemplate.gameObject);

                _newCardID = System.Guid.NewGuid().ToString();

                _cardProperties = _instCard.GetComponent<CardScript>();

                _cardProperties.GetRenderer().sprite = _selectedItem.GetItemSprite();

                _cardProperties.SetCamera(_camera);

                _cardProperties.GetRenderer().gameObject.transform.localScale = ((new Vector3(0.05f, 0.05f, 1.0f)) * _selectedItem.GetItemSpriteIconScale());

                _cardProperties.SetManager(this);

                _cardProperties.SetCardItem(_selectedItem);

                _cardProperties.SetCardID(_newCardID);

                _gameSession.AddCard(_cardProperties);

                _instCard.transform.parent = _cardSpaceTr.transform;

                _instCard.transform.localPosition = _position;

                SetNextCardPosition();
            }
        }        
        ShuffleCards();
    }

    int GetNumberOfCards()
    {
        switch (_difficultyLevel)
        {
            case 2:
                _position = new Vector3((-7.5f * _distanceScale), (5.0f * _distanceScale), 0.0f);

                _basePosition = _position;

                return 12;
            case 3:
                _position = new Vector3((-7.5f * _distanceScale), (7.5f * _distanceScale), 0.0f);

                _basePosition = _position;

                return 16;
            case 4:
                _position = new Vector3((-10.0f * _distanceScale), (7.5f * _distanceScale), 0.0f);

                _basePosition = _position;

                return 20;
            default:
                _position = new Vector3((-7.5f * _distanceScale), (2.5f * _distanceScale), 0.0f);

                _basePosition = _position;

                return 8;
        }
    }

    void SetNextCardPosition()
    {
        if(_gameSession == null)
        {
            return;
        }

        if(_difficultyLevel == 4)
        {
            if((_gameSession.GetCards().Count % 5) == 0)
            {
                _position.x = _basePosition.x;

                _position.y -= (5.0f * _distanceScale);
            }
            else
            {
                _position.x += (5.0f * _distanceScale);
            }
        }
        else
        {
            if((_gameSession.GetCards().Count % 4) == 0)
            {
                _position.x = _basePosition.x;

                _position.y -= (5.0f * _distanceScale);
            }
            else
            {
                _position.x += (5.0f * _distanceScale);
            }
        }
    }

    void ShuffleCards()
    {
        List<Vector3> _positions = new List<Vector3>();

        List<CardScript> _newList = new List<CardScript>();

        List<int> _indexes = new List<int>();

        for(int _i = 0; _i < _gameSession.GetCards().Count; _i++)
        {
            _indexes.Add(_i);

            _positions.Add(_gameSession.GetCards()[_i].gameObject.transform.localPosition);
        }

        int _j, _selectedIndex;

        int _k = 0;

        while(_indexes.Count > 0)
        {
            _j = Random.Range(0, _indexes.Count);

            _selectedIndex = _indexes[_j];

            _newList.Add(_gameSession.GetCards()[_selectedIndex]);

            _gameSession.GetCards()[_selectedIndex].gameObject.transform.localPosition = _positions[_k];

            _gameSession.GetCards()[_selectedIndex].SetAssignedPosition(_positions[_k]);

            _indexes.RemoveAt(_j);

            _k++;
        }

        for(int _i = (_newList.Count - 1); _i >= 0; _i--)
        {
            _newList[_i].gameObject.name = "Card No. " + (_i + 1);

            _newList[_i].gameObject.transform.SetSiblingIndex(0);
        }

        _gameSession.SetCards(_newList);
    }

    void PlayAudio(AudioClip _input)
    {
        if(_audioSource == null || _input == null)
        {
            return;
        }

        _audioSource.clip = _input;

        _audioSource.Play();
    }

    void UpdateScores()
    {
        if(_gameSession == null)
        {
            return;
        }

        _gameSession.UpdateScore();

        if (_playingCanvas != null)
        {
            if (_playingCanvas.GetCompletionText() != null)
            {
                _playingCanvas.GetCompletionText().text = _gameSession.GetCompletionPercentage().ToString("0.00") + "%";
            }


            if (_playingCanvas.GetProgressText() != null)
            {
                _playingCanvas.GetProgressText().text = _gameSession.GetProgressPercentage().ToString("0.00") + "%";
            }
        }
    }

    void LoadGame()
    {
        int _count = DataPersistenceManagerScript.GetInstance().GetGameData()._flippingStati.Count;

        CardScript _cardInst;

        GameObject _cardInstGO;

        string _key;

        bool _flipS;

        string _itemKey;

        Vector3 _assignedP = Vector3.zero;

        bool _itemFound;

        int _turnsV, _matchesV;

        float _progP, _compP;

        List<CardScript> _cards = new List<CardScript>();

        _gameSession = new GameSessionScript();

        _gameSession.SetGameManager(this);

        CardItemClass _currentItem = new CardItemClass();

        for(int _i = 0; _i < _count; _i++)
        {
            _cardInstGO = Instantiate(_cardTemplate.gameObject);

            _itemFound = false;

            _cardInst = _cardInstGO.GetComponent<CardScript>();

            _key = DataPersistenceManagerScript.GetInstance().GetGameData()._flippingStati.ElementAt(_i).Key;

            _flipS = DataPersistenceManagerScript.GetInstance().GetGameData()._flippingStati.ElementAt(_i).Value;

            _itemKey = DataPersistenceManagerScript.GetInstance().GetGameData()._cardItems.ElementAt(_i).Value;

            _assignedP = DataPersistenceManagerScript.GetInstance().GetGameData()._cardPositions.ElementAt(_i).Value;

            _cardInst.SetCardID(_key);

            _cardInst.SetCardFinished(_flipS);

            _cardInst.SetCardFlipped(_flipS);

            for(int _j = 0; _j < ItemsManagerScript.GetInstance().GetCardItems().Count && !_itemFound; _j++)
            {
                if (ItemsManagerScript.GetInstance().GetCardItems()[_j].GetItemID() == _itemKey)
                {
                    _currentItem = ItemsManagerScript.GetInstance().GetCardItems()[_j];

                    _cardInst.SetCardItem(_currentItem);

                    _itemFound = true;
                }
            }

            _cardInst.SetAssignedPosition(_assignedP);

            _cardInst.SetCamera(_camera);

            _cardInst.GetRenderer().sprite = _currentItem.GetItemSprite();

            _cardInst.SetCamera(_camera);

            _cardInst.GetRenderer().gameObject.transform.localScale = ((new Vector3(0.05f, 0.05f, 1.0f)) * _currentItem.GetItemSpriteIconScale());

            _cardInst.SetManager(this);

            _cards.Add(_cardInst);

            _cardInstGO.transform.parent = _cardSpaceTr;

            _cardInstGO.transform.localPosition = _assignedP;

            _cardInstGO.SetActive(!_flipS);
        }

        for(int _i = (_cards.Count - 1); _i >= 0; _i--)
        {
            _cardInstGO = _cards[_i].gameObject;

            _cardInstGO.name = "Card No. " + (_i + 1);

            _cardInstGO.transform.SetSiblingIndex(0);
        }

        _gameSession.SetCards(_cards);

        _turnsV = DataPersistenceManagerScript.GetInstance().GetGameData()._turns;

        _matchesV = DataPersistenceManagerScript.GetInstance().GetGameData()._matches;

        _progP = DataPersistenceManagerScript.GetInstance().GetGameData()._progressPercentage;

        _compP = DataPersistenceManagerScript.GetInstance().GetGameData()._completionPercentage;

        _gameSession.SetTurns(_turnsV);

        _gameSession.SetMatches(_matchesV);

        _gameSession.SetProgressPercentage(_progP);

        _gameSession.SetCompletionPercentage(_compP);

        _playingCanvas.GetTurnsCountText().text = _turnsV.ToString();

        _playingCanvas.GetMatchCountText().text = _matchesV.ToString();

        _playingCanvas.GetProgressText().text = _progP.ToString("0.00") + "%";

        _playingCanvas.GetCompletionText().text = _compP.ToString("0.00") + "%";
    }
}
