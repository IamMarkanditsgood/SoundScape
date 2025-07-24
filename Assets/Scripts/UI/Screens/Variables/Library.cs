using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Library : BasicScreen
{
    [Serializable]
    public class PresetCollection
    {
        public Transform content;
        public DefaultSoundPanel panelPrefab;
        public TMP_Text presetAmount;
        public Button openButton;
        public Image buttonImage;
        public List<DefaultSoundPanel> panels = new List<DefaultSoundPanel>();
        public bool isOpened;
    }

    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private PresetCollection _defaultPreset;
    [SerializeField] private PresetCollection _likedPreset;
    [SerializeField] private PresetCollection _myPreset;
    [SerializeField] private Sprite _openButton;
    [SerializeField] private Sprite _closeButton;

    public override void Init()
    {
        base.Init();
        _defaultPreset.openButton.onClick.AddListener(() => PresetCollectionPresed(_defaultPreset));
        _likedPreset.openButton.onClick.AddListener(() => PresetCollectionPresed(_likedPreset));
        _myPreset.openButton.onClick.AddListener(() => PresetCollectionPresed(_myPreset));
    }

    private void OnDestroy()
    {
        _defaultPreset.openButton.onClick.RemoveListener(() => PresetCollectionPresed(_defaultPreset));
        _likedPreset.openButton.onClick.RemoveListener(() => PresetCollectionPresed(_likedPreset));
        _myPreset.openButton.onClick.RemoveListener(() => PresetCollectionPresed(_myPreset));
    }

    public override void ResetScreen()
    {
        if(_defaultPreset.isOpened)
            PresetCollectionPresed(_defaultPreset);
        if (_likedPreset.isOpened)
            PresetCollectionPresed(_likedPreset);
        if (_myPreset.isOpened)
            PresetCollectionPresed(_myPreset);
    }

    public override void SetScreen()
    {
        SetLikedPresets();
        SetDefaultPresets();
        SetYourPresets();
    }

    public void DeleatPreset(string presetName, bool like)
    {
        if (like)
        {
            foreach (var preset in _likedPreset.panels)
            {
                if (preset.presetName == presetName)
                {
                    Destroy(preset);
                    SetLikedPresets();
                    return;
                }
            }
        }

        else
        {
            foreach (var preset in _likedPreset.panels)
            {
                if (preset.presetName == presetName)
                {
                    Destroy(preset);
                    SetLikedPresets();
                    return;
                }
            }
            foreach (var preset in _myPreset.panels)
            {
                if (preset.presetName == presetName)
                {
                    Destroy(preset);
                    SetYourPresets();
                    return;
                }
            }
        }
    }

    public void SetLikedPresets()
    {
        for (int i = 0; i < _likedPreset.panels.Count; i++)
        {
            if (_likedPreset.panels[i] != null)
            {
                int ind = i;
                _likedPreset.panels[i].gameObject.GetComponent<Button>().onClick.RemoveListener(() => PresetPressed(_likedPreset.panels[ind].presetName));
                Destroy(_likedPreset.panels[i].gameObject);
            }
                
        }
        _likedPreset.panels.Clear();

        List<string> likedSounds = SaveManager.PlayerPrefs.LoadStringList(GameSaveKeys.LikedPreset);

        for (int i = 0; i < likedSounds.Count; i++)
        {
            
            DefaultSoundPanel panel = Instantiate(_likedPreset.panelPrefab, _likedPreset.content);
            panel = SetPanel(panel, GameManager.instance.GetPresetData(likedSounds[i]));
            SoundPanel soundPanel = (SoundPanel)panel;
            soundPanel.isLikeCollection = true;
            _likedPreset.panels.Add(panel);

            int index = i;
            panel.gameObject.GetComponent<Button>().onClick.AddListener(() => PresetPressed(_likedPreset.panels[index].presetName));
        }
        _likedPreset.presetAmount.text = _likedPreset.panels.Count + " presets";
        Canvas.ForceUpdateCanvases();
    }

    public void SetDefaultPresets()
    {
        for (int i = 0; i < _defaultPreset.panels.Count; i++)
        {
            if (_defaultPreset.panels[i] != null)
            {
                int ind = i;
                _defaultPreset.panels[i].gameObject.GetComponent<Button>().onClick.RemoveListener(() => PresetPressed(_defaultPreset.panels[ind].presetName));
                Destroy(_defaultPreset.panels[i].gameObject);
            }  
        }
        _defaultPreset.panels.Clear();

        for (int i = 0; i < _gameConfig._defaultPresets.Count; i++)
        {
            DefaultSoundPanel panel = Instantiate(_defaultPreset.panelPrefab, _defaultPreset.content);
            panel = SetPanel(panel, _gameConfig._defaultPresets[i]);
            _defaultPreset.panels.Add(panel);

            int index = i;
            panel.gameObject.GetComponent<Button>().onClick.AddListener(() => PresetPressed(_defaultPreset.panels[index].presetName));
        }
        _defaultPreset.presetAmount.text = _defaultPreset.panels.Count + " presets";
        Canvas.ForceUpdateCanvases();
    }

    public void SetYourPresets()
    {
        for (int i = 0; i < _myPreset.panels.Count; i++)
        {
            if (_myPreset.panels[i] != null)
            {
                int ind = i;
                _myPreset.panels[i].gameObject.GetComponent<Button>().onClick.RemoveListener(() => PresetPressed(_myPreset.panels[ind].presetName));
                Destroy(_myPreset.panels[i].gameObject);
            }
        }
        _myPreset.panels.Clear();

        List<PresetData> presets = GameManager.instance.Presets;

        for (int i = 0; i < presets.Count; i++)
        {
            if (!IsPresetLikedOrDefault(presets[i].presetName))
            {
                DefaultSoundPanel panel = Instantiate(_myPreset.panelPrefab, _myPreset.content);
                panel = SetPanel(panel, GameManager.instance.GetPresetData(presets[i].presetName));
                _myPreset.panels.Add(panel);

                Debug.Log(_myPreset.panels.Count);

                string presetName = presets[i].presetName;
                panel.gameObject.GetComponent<Button>().onClick.AddListener(() => PresetPressed(presetName));
            }
        }

        _myPreset.presetAmount.text = _myPreset.panels.Count + " presets";
        Canvas.ForceUpdateCanvases();
    }

    private bool IsPresetLikedOrDefault(string name)
    {
        foreach(var preset in _gameConfig._defaultPresets)
        {
            if(preset.presetName == name)
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

    private DefaultSoundPanel SetPanel(DefaultSoundPanel panel, PresetData presetData)
    {
        List<Sprite> icons = new();
        for (int i = 0; i < presetData.sounds.Count; i++)
        {
            icons.Add(GetPinkSprite(presetData.sounds[i]));
        }

        //panel.SetPanel(_gameConfig.presetImages[presetData.presetSpriteIndex], presetData.presetName, icons);

        return panel;
    }

    private Sprite GetPinkSprite(SoundTypes sound)
    {
        for (int i = 0; i < _gameConfig.sounds.Length; i++)
        {
            if (_gameConfig.sounds[i].soundType == sound)
                return _gameConfig.sounds[i].pinkSoundIcon;
        }
        return null;
    }

    private void PresetPressed(string name)
    {
        Debug.Log("name = " + name);
        SoundPlayer soundPlayer = (SoundPlayer)UIManager.Instance.GetPopup(PopupTypes.SoundPlayer);
        soundPlayer.Init(name);
        soundPlayer.Show();
    }

    private void PresetCollectionPresed(PresetCollection presetCollection)
    {
        if (presetCollection.isOpened)
        {
            presetCollection.buttonImage.sprite = _openButton;
            presetCollection.isOpened = false;
            presetCollection.content.gameObject.SetActive(false);
        }
        else
        {
            presetCollection.buttonImage.sprite = _closeButton;
            presetCollection.isOpened = true;
            presetCollection.content.gameObject.SetActive(true);
        }
    }

}
