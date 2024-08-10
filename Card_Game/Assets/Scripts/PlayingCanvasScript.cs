using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingCanvasScript : MonoBehaviour
{
    [SerializeField]
    Text _matchesCountText;

    [SerializeField]
    Text _turnsCountText;

    [SerializeField]
    RectTransform _mainPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Text MatchCountText()
    {
        return _matchesCountText;
    }

    public Text GetTurnsCountText()
    {
        return _turnsCountText;
    }

    public RectTransform GetMainPanel()
    {
        return _mainPanel;
    }

}
