using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetSaver : BasicPopup
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Image _mainImage;
    [SerializeField] private TMP_InputField _presetName;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Image[] _icons;
    [SerializeField] private Button[] _imageButtons;
    [SerializeField] private Image[] _bgButtons;
    [SerializeField] private Sprite[] _images;

    private List<SoundTypes> _sounds = new();
    private List<float> _soundsVolume = new();
    private int _currentImage;

    public override void Subscribe()
    {
        _cancelButton.onClick.AddListener(Cancel);
        _saveButton.onClick.AddListener(Save); 

        for(int i = 0; i < _imageButtons.Length; i++)
        {
            int index = i;
            _imageButtons[index].onClick.AddListener(()=> SetCurrentImage(index));
        }
    }

    public override void Unsubscribe()
    {
        _cancelButton.onClick.RemoveListener(Cancel);
        _saveButton.onClick.RemoveListener(Save);

        for (int i = 0; i < _imageButtons.Length; i++)
        {
            int index = i;
            _imageButtons[index].onClick.RemoveListener(() => SetCurrentImage(index));
        }
    }

    public void Init(List<SoundTypes> sounds, List<float> soundsVolume)
    {
        _sounds = sounds;
        _soundsVolume = soundsVolume;
    }

    public override void SetPopup()
    {
        _presetName.text = "Preset";
        SetIcons();
        SetCurrentImage(0);
    }

    public override void ResetPopup()
    {
        _presetName.text = "Preset";
        SetCurrentImage(0);
        foreach (var icon in _icons)
        {
            icon.gameObject.SetActive(false);
        }
    }

    private void SetIcons()
    {
        for (int i = 0; i < _sounds.Count; i++)
        {
            _icons[i].gameObject.SetActive(true);
            _icons[i].sprite = _gameConfig.GetSoundData(_sounds[i]).blackSoundIcon;
        }
    }

    private void SetCurrentImage(int index)
    {
        _currentImage = index;
        foreach (var bg in _bgButtons)
        {
            bg.enabled = false;
        }
        _bgButtons[_currentImage].enabled = true;
        _mainImage.sprite = _images[_currentImage];
    }

    private void Cancel()
    {
        UIManager.Instance.HidePopup(PopupTypes.PresetSaver);
    }

    private void Save()
    {
        PresetData newPreset = new PresetData();
        newPreset.presetName = GetUniqueName();
       // newPreset.presetSpriteIndex = _currentImage;
        newPreset.sounds = _sounds;
        newPreset.volume = _soundsVolume;
        newPreset.isLiked = false;

        GameManager.instance.SetNewPreset(newPreset);

        Library library = (Library)UIManager.Instance.GetScreen(ScreenTypes.Library);
        library.SetYourPresets();

        Hide();
    }

    private string GetUniqueName()
    {
        string baseName = _presetName.text.Trim(); // Clean input
        string uniqueName = baseName;
        List<PresetData> presets = GameManager.instance.Presets;

        int counter = 1;

        // Check for conflicts and generate a unique name
        while (presets.Any(p => p.presetName == uniqueName))
        {
            uniqueName = $"{baseName} ({counter})";
            counter++;
        }

        return uniqueName;
    }
}
