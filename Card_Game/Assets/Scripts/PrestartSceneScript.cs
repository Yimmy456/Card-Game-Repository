using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrestartSceneScript : MonoBehaviour
{
    [SerializeField]
    float _loadingTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMenuScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(_loadingTime);

        SceneManager.LoadScene("Menu Scene");
    }
}
