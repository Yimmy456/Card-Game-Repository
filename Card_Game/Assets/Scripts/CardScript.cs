using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
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

    GameManagerScript _manager;

    [SerializeField]
    Collider _cardCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckClick();
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
                    Debug.Log("The card is touched.");
                }
            }            
        }        
    }
}
