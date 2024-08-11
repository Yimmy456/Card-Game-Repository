using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandlerScript
{
    private string _fileDirPath = "";
    private string _fileFileName = "";

    bool _useEncryption = false;

    string _d;

    readonly string _encryptionCodeWord = "word";

    public FileDataHandlerScript(string _pathInput, string _nameInput, bool _useEncryptionInput)
    {
        _fileDirPath = _pathInput;
        _fileFileName = _nameInput;
        _useEncryption = _useEncryptionInput;
    }

    public void SaveData(GameDataScript _input)
    {
        string _fullPath = Path.Combine(_fileDirPath, _fileFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));
            string _dataToStore = JsonUtility.ToJson(_input, true);

            if (_useEncryption)
            {
                _dataToStore = EncryptDecrypt(_dataToStore);
            }

            using (FileStream _file = new FileStream(_fullPath, FileMode.Create))
            {
                using (StreamWriter _writer = new StreamWriter(_file))
                {
                    _writer.Write(_dataToStore);
                }
            }

            File.WriteAllText(_fullPath, _dataToStore);
        }
        catch (Exception e)
        {

        }
    }

    public GameDataScript LoadData()
    {
        GameDataScript _data = null;

        string _fullPath = Path.Combine(_fileDirPath, _fileFileName);

        Debug.Log("The file path is " + _fullPath + ".");

        if (File.Exists(_fullPath))
        {
            try
            {
                string _dataToLoad;
                using (FileStream _file = new FileStream(_fullPath, FileMode.Open))
                {
                    using (StreamReader _reader = new StreamReader(_file))
                    {
                        _dataToLoad = _reader.ReadToEnd();
                    }
                }
                if (_useEncryption)
                {
                    _dataToLoad = EncryptDecrypt(_dataToLoad);
                }

                _d = _dataToLoad;

                _data = JsonUtility.FromJson<GameDataScript>(_dataToLoad);
            }
            catch (Exception e)
            {

            }
        }
        return _data;
    }

    string EncryptDecrypt(string _input)
    {
        string _modifiedData = "";

        for (int _i = 0; _i < _input.Length; _i++)
        {
            _modifiedData += (char)(_input[_i] ^ _encryptionCodeWord[_i % _encryptionCodeWord.Length]);
        }

        return _modifiedData;
    }

    public string GetDataToLoad()
    {
        return _d;
    }
}
