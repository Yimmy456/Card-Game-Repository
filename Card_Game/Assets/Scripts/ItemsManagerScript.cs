using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManagerScript : MonoBehaviour
{
    static ItemsManagerScript _instance;

    [SerializeField]
    List<CardItemClass> _cardItems;

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
        
    }

    public static ItemsManagerScript GetInstance()
    {
        return _instance;
    }
}
