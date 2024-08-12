using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvasScript : MonoBehaviour
{
    [SerializeField]
    Slider _difficultySlider;

    [SerializeField]
    Button _resumeButton;

    [SerializeField]
    Dropdown _cardSizeDropdown;

    // Start is called before the first frame update
    void Start()
    {
        if(ItemsManagerScript.GetInstance() != null && _difficultySlider != null)
        {
            ItemsManagerScript.GetInstance().SetDifficultySlider(_difficultySlider);
        }

        if(_resumeButton != null && DataPersistenceManagerScript.GetInstance() != null)
        {
            _resumeButton.interactable = DataPersistenceManagerScript.GetInstance().GetGameData()._validSession;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Slider GetDifficultySlider()
    {
        return _difficultySlider;
    }

    public Button GetResumeButton()
    {
        return _resumeButton;
    }

    public Dropdown GetCardSizeDropdown()
    {
        return _cardSizeDropdown;
    }
}
