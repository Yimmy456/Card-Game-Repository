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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public Renderer GetRenderer()
    {
        return _renderer;
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
}
