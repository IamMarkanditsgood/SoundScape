using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private List<PresetData> _presets;

    [SerializeField] private AudioSource _audioSourcePrefab;

    private List<AudioSource> _audioSources = new List<AudioSource>();

    public List<PresetData> Presets => _presets;

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

    public void DeletePreset(string presetName)
    {
        for (int i = _presets.Count - 1; i >= 0; i--)
        {
            if (_presets[i].presetName == presetName)
            {
                _presets.RemoveAt(i);
            }
        }
    }


    public void SetNewPreset(PresetData newPreset)
    {
        _presets.Add(newPreset);
        SaveData();
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

    public void CleanSounds()
    {
        foreach(var audio in _audioSources)
        {
            audio.Stop();
            Destroy(audio.gameObject);
        }
        _audioSources.Clear();
    }

    public void SetSounds(string presetName)
    {
        CleanSounds();

        PresetData preset = GetPresetData(presetName);
        for (int i =0; i< preset.sounds.Count; i++)
        {
            SoundData soundData = _gameConfig.GetSoundData(preset.sounds[i]);
            AudioSource newSource = Instantiate(_audioSourcePrefab);
            float volume = GetSoundVolume(preset.sounds[i], preset);

            newSource.clip = soundData.soundClip;
            newSource.volume = volume;

            _audioSources.Add(newSource);
        }
    }

    public void PlaySounds()
    {
        foreach(var audioSource in _audioSources)
        {
            audioSource.Play(); 
        }
    }

    public void StopSounds()
    {
        foreach (var audioSource in _audioSources)
        {
            audioSource.Stop();
        }
    }

    private float GetSoundVolume(SoundTypes soundType, PresetData currentPreset)
    {
        for(int i = 0; i < currentPreset.sounds.Count; i++)
        {
            return currentPreset.volume[i];
        }

        return 0;
    }
}
