using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField]
    string _cardID;

    [SerializeField]
    CardItemClass _cardItem;

    [SerializeField]
    bool _cardFinished;

    [SerializeField]
    bool _cardFlipped;

    [SerializeField]
    SpriteRenderer _renderer;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Vector3 _assignedPosition;

    GameManagerScript _manager;

    [SerializeField]
    Collider _cardCollider;

    [SerializeField]
    Transform _cardBack;

    [SerializeField]
    Transform _cardFront;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckClick();
    }

    public string GetCardID()
    {
        return _cardID;
    }

    public CardItemClass GetCardItem()
    {
        return _cardItem;
    }

    public bool GetCardFlipped()
    {
        return _cardFlipped;
    }

    public bool GetCardFinished()
    {
        return _cardFinished;
    }

    public SpriteRenderer GetRenderer()
    {
        return _renderer;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public GameManagerScript GetManager()
    {
        return _manager;
    }

    public Vector3 GetAssignedPosition()
    {
        return _assignedPosition;
    }

    public Transform GetCardBackTransform()
    {
        return _cardBack;
    }

    public Transform GetCardFrontTransform()
    {
        return _cardFront;
    }

    public void SetCardID(string _input)
    {
        _cardID = _input;
    }

    public void SetCardItem(CardItemClass _input)
    {
        _cardItem = _input;
    }

    public void SetCardFlipped(bool _input)
    {
        _cardFlipped = _input;
    }

    public void SetCardFinished(bool _input)
    {
        _cardFinished = _input;
    }

    public void SetCamera(Camera _input)
    {
        _camera = _input;
    }

    public void SetManager(GameManagerScript _input)
    {
        _manager = _input;
    }

    public void SetAssignedPosition(Vector3 _input)
    {
        _assignedPosition = _input;
    }

    void CheckClick()
    {
        if(_camera == null || _manager == null || _cardFlipped || _cardCollider == null)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit _hit;

            if(Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider == _cardCollider)
                {
                    _manager.EvaluateCards(this);
                }
            }
        }

        if(Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                Vector2 _touchPos = _camera.ScreenToWorldPoint(_touch.position);

                Ray _ray = _camera.ScreenPointToRay(_touchPos);

                RaycastHit _hit;

                if(Physics.Raycast(_ray, out _hit))
                {
                    if(_hit.collider == _cardCollider)
                    {
                        _manager.EvaluateCards(this);
                    }
                }
            }            
        }        
    }
}
