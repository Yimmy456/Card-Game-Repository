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
    Text _progressText;

    [SerializeField]
    Text _completionText;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManagerScript.GetInstance() != null)
        {
            GameManagerScript.GetInstance().SetPlayingCanvas(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Text GetMatchCountText()
    {
        return _matchesCountText;
    }

    public Text GetTurnsCountText()
    {
        return _turnsCountText;
    }

    public Text GetProgressText()
    {
        return _progressText;
    }

    public Text GetCompletionText()
    {
        return _completionText;
    }
}
