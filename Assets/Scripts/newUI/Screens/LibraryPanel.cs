using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LibraryPanel : MonoBehaviour
{    
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private TMP_Text _presetName;
    [SerializeField] private TMP_Text _presetDescription;
    [SerializeField] private Image[] _soundIcons;
    [SerializeField] private RawImage _presetImage;
    [SerializeField] private Texture2D _defaultPresetImage;
    [SerializeField] private Button _deletePresetButton;

    private PresetIamgeManager _presetImageManager;
    private PresetData _currentPreset;

    private void Start()
    {
        _deletePresetButton.onClick.AddListener(DeleatePreset);
    }

    private void OnDestroy()
    {
        _deletePresetButton.onClick.RemoveListener(DeleatePreset);
    }

    public void SetPreset(PresetData preset, PresetIamgeManager presetIamgeManager)
    {
        _currentPreset = preset;
        _presetImageManager = presetIamgeManager;
        SetPanel();
    }

    private void SetPanel()
    {
        if (_currentPreset != null)
        {
            _presetName.text = _currentPreset.presetName;
            _presetDescription.text = _currentPreset.presetDesciption;
            _presetImageManager.SetSavedPicture(_currentPreset.presetSpriteAdres, _presetImage, _defaultPresetImage);
            for (int i = 0; i < _soundIcons.Length; i++)
            {
                if (_currentPreset.volume[i] > 0)
                {
                    SoundData soundData = GetSoundData(_currentPreset.sounds[i]);
                    _soundIcons[i].sprite = soundData.pinkSoundIcon;
                }
                else
                {
                    SoundData soundData = GetSoundData(_currentPreset.sounds[i]);
                    _soundIcons[i].sprite = soundData.blackSoundIcon;
                }
            }
        }
    }
    private SoundData GetSoundData(SoundTypes sound)
    {
        for (int i = 0; i < _gameConfig.sounds.Length; i++)
        {
            if (_gameConfig.sounds[i].soundType == sound)
            {
                return _gameConfig.sounds[i];
            }
        }
        return null;
    }

    private void DeleatePreset()
    {
        GameManager.instance.DeletePreset(_currentPreset.presetName);
        LibraryScreen libraryScreen = UIManager.Instance.GetScreen(ScreenTypes.Library) as LibraryScreen;
        if (libraryScreen != null)
        {
            libraryScreen.CreatePanels();
        }
    }
}
