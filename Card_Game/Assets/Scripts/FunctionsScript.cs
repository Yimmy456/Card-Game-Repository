using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionsScript : MonoBehaviour
{
    public void LoadScene(string _input)
    {
        SceneManager.LoadScene(_input);
    }

    public void LoadScene(int _input)
    {
        SceneManager.LoadScene(_input);
    }
}
