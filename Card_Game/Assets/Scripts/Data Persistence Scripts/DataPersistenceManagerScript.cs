using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DataPersistenceManagerScript : MonoBehaviour
{
    [SerializeField]
    string _fileName;

    [SerializeField]
    GameDataScript _data;

    [SerializeField]
    bool _useEncryption;

    List<IDataPersistenceScript> _dataPersistentObjects;

    FileDataHandlerScript _fileHandler;

    static DataPersistenceManagerScript _instance;

    bool _fileFound = true;

    bool _loadSession = true;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _fileHandler = new FileDataHandlerScript(Application.persistentDataPath, _fileName, _useEncryption);

        _dataPersistentObjects = new List<IDataPersistenceScript>();

        Debug.Log("File path is: " + _fileHandler + ".");

        LoadGame();
    }

    private void OnEnable()
    {
        if (_dataPersistentObjects == null)
        {
            _dataPersistentObjects = new List<IDataPersistenceScript>();
        }
    }

    public void NewGame()
    {
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/" + _fileName, "");
        _data = new GameDataScript();
    }

    public bool GetLoadSession()
    {
        return _loadSession;
    }

    public void SetLoadGame(bool _input)
    {
        _loadSession = _input;
    }

    public void LoadGame()
    {
        _data = _fileHandler.LoadData();

        if (_data == null)
        {
            NewGame();
        }

        foreach (IDataPersistenceScript _object in _dataPersistentObjects)
        {
            if (_object != null)
            {
                _object.LoadData(_data);
            }
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistenceScript _object in _dataPersistentObjects)
        {
            if (_object != null)
            {
                _object.SaveData(ref _data);
            }
        }

        if (_fileHandler != null && _data != null)
        {
            _fileHandler.SaveData(_data);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveGame();
    }

    /*private void OnApplicationPause(bool pause)
    {
        SaveGame();
    }*/

    private List<IDataPersistenceScript> FindAllDataPersistenceObject()
    {
        IEnumerable<IDataPersistenceScript> _dataPersistenceObject = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceScript>();

        return new List<IDataPersistenceScript>(_dataPersistenceObject);
    }

    public static DataPersistenceManagerScript GetInstance()
    {
        return _instance;
    }

    public GameDataScript GetGameData()
    {
        return _data;
    }

    string GetFilePath()
    {
        string _path = Application.persistentDataPath;

        if (Application.platform == RuntimePlatform.Android)
        {
            _path = "/Android/data/com.DefaultCompany.Card_Game/files/";
        }

        return _path;
    }

    public void ClearGameData()
    {
        if(_data == null)
        {
            return;
        }

        _data._validSession = false;

        _data._flippingStati.Clear();

        _data._cardItems.Clear();

        _data._cardPositions.Clear();
    }
}
