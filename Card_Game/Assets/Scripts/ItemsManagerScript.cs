using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManagerScript : MonoBehaviour
{
    static ItemsManagerScript _instance;

    [SerializeField]
    List<CardItemClass> _cardItems;

    [SerializeField]
    Slider _difficultySlider;

    int _difficulty = 1;

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

        DontDestroyOnLoad(_instance);
    }

    // Update is called once per frame
    void Update()
    {
        if(_difficultySlider != null)
        {
            _difficulty = (int)_difficultySlider.value;
        }
    }

    public static ItemsManagerScript GetInstance()
    {
        return _instance;
    }

    public List<CardItemClass> GetCardItems()
    {
        return _cardItems;
    }

    public Slider GetDifficultySlider()
    {
        return _difficultySlider;
    }

    public int GetDifficulty()
    {
        return _difficulty;
    }

    public void SetDifficultySlider(Slider _input)
    {
        _difficultySlider = _input;
    }
}
