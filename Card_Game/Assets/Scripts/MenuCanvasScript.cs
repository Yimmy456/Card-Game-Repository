using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvasScript : MonoBehaviour
{
    [SerializeField]
    Slider _difficultySlider;

    // Start is called before the first frame update
    void Start()
    {
        if(ItemsManagerScript.GetInstance() != null && _difficultySlider != null)
        {
            ItemsManagerScript.GetInstance().SetDifficultySlider(_difficultySlider);
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
}
