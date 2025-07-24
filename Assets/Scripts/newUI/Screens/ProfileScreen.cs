using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TextManager;

public class ProfileScreen : BasicScreen
{
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_InputField _name;

    [SerializeField] private Button _editName;
    [SerializeField] private Button _editPhoto;

    [SerializeField] private TMP_Text _allTime;
    [SerializeField] private TMP_Text _createdPresets;
    [SerializeField] private TMP_Text _favoritePreset;
    [SerializeField] private Image _favoriteSound;

    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private AvatarManager _avatarManager;

    private void Start()
    {
        if (!SaveManager.PlayerPrefs.IsSaved(GameSaveKeys.Vibro))
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Vibro, 1);
        }
        if (!SaveManager.PlayerPrefs.IsSaved(GameSaveKeys.Notification))
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Notification, 1);
        }

        _name.text = SaveManager.PlayerPrefs.LoadString(GameSaveKeys.Name, "User Name");

        _editName.onClick.AddListener(EditName);
        _editPhoto.onClick.AddListener(EditPhoto);
        _backButton.onClick.AddListener(Back);
    }

    private void OnDestroy()
    {
        _editName.onClick.RemoveListener(EditName);
        _editPhoto.onClick.RemoveListener(EditPhoto);
        
        _backButton.onClick.RemoveListener(Back);
    }

    private void OnApplicationQuit()
    {
        SaveManager.PlayerPrefs.SaveString(GameSaveKeys.Name, _name.text);
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        _name.interactable = false;
        _avatarManager.SetSavedPicture();
        SetAnalitic();

    }

    private void SetAnalitic()
    {
        TextManager textManager = new TextManager();
        int seconds = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalListenTime);
        textManager.SetTimerText(_allTime, seconds, showMinutes: true, showHours: true, format: TimeFormat.LettersSeparated);

        string likedPresetName = SaveManager.PlayerPrefs.LoadString(GameSaveKeys.LikedPreset);
        textManager.SetText(likedPresetName, _favoritePreset);

        int created = GetCreatedPresetsAmount();
        textManager.SetText(created, _createdPresets);

        SoundTypes favoriteSound = GetMostFrequentSoundType(GameManager.instance.Presets);
        _favoriteSound.sprite = _gameConfig.GetSoundData(favoriteSound).pinkSoundIcon;
    }

    private SoundTypes GetMostFrequentSoundType(List<PresetData> presets)
    {
        Dictionary<SoundTypes, int> frequencyMap = new();

        foreach (var preset in presets)
        {
            foreach (var sound in preset.sounds)
            {
                if (frequencyMap.ContainsKey(sound))
                    frequencyMap[sound]++;
                else
                    frequencyMap[sound] = 1;
            }
        }

        SoundTypes mostFrequent = default;
        int maxCount = -1;

        foreach (var kvp in frequencyMap)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                mostFrequent = kvp.Key;
            }
        }

        return mostFrequent;
    }

    private int GetCreatedPresetsAmount()
    {
        List<PresetData> presets = GameManager.instance.Presets;
        int count = 0;
        foreach (var preset in presets)
        {
            if (!IsPresetLikedOrDefault(preset.presetName))
                count++;
        }

        return count;
    }
    private bool IsPresetLikedOrDefault(string name)
    {
        foreach (var preset in _gameConfig._defaultPresets)
        {
            if (preset.presetName == name)
                return true;
        }
        List<string> likedSounds = SaveManager.PlayerPrefs.LoadStringList(GameSaveKeys.LikedPreset);
        foreach (var likedPreset in likedSounds)
        {
            if (likedPreset == name)
                return true;
        }

        return false;
    }

    

    private void EditName()
    {
        _name.interactable = !_name.interactable;
    }

    private void EditPhoto()
    {
        _avatarManager.PickFromGallery();
    }


    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }

}
