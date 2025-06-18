using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private List<PresetData> _presets;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Init();
        LoadData();
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveData();
        }
    }

    private void Init()
    {
        for(int i = 0; i < _gameConfig._defaultPresets.Count; i++)
        {
            string key = _gameConfig._defaultPresets[i].presetName;
            if (!SaveManager.JsonStorage.Exists(key))
            {
                _presets.Add(_gameConfig._defaultPresets[i]);
            }
        }
    }

    private void SaveData()
    { 
        if (_presets.Count > 0)
        {
            List<string> saveKeys = new();

            for (int i = 0; i < _presets.Count; i++)
            {
                string saveKey = _presets[i].presetName;
                saveKeys.Add(saveKey);
                SaveManager.JsonStorage.SaveToJson<PresetData>(saveKey, _presets[i]);
            }

            SaveManager.PlayerPrefs.SaveStringList(GameSaveKeys.Presets, saveKeys);
        }
    }

    private void LoadData()
    {
        List<string> presetNames = SaveManager.PlayerPrefs.LoadStringList(GameSaveKeys.Presets);

        if(presetNames.Count > 0)
        {
            LoadPresets(presetNames);
        }
    }
    private void LoadPresets(List<string> presetNames)
    {
        for(int i = 0; i < presetNames.Count; i++)
        {
            string saveKey = presetNames[i];
            if (SaveManager.JsonStorage.Exists(saveKey))
            {
                PresetData presetData = SaveManager.JsonStorage.LoadFromJson<PresetData>(saveKey);
                _presets.Add(presetData);
            }
        }
    }

    public PresetData GetPresetData(string presetName)
    {
        foreach (PresetData presetData in _presets)
        {
            if(presetData.presetName == presetName)
                return presetData;
        }
        Debug.LogError($"You do not have {presetName} preset");
        return null;
    }
}
