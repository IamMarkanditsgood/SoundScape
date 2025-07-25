using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Home : BasicScreen
{
    [SerializeField] private PresetIamgeManager _presetImageManager;
    [SerializeField] private GameConfig _gameConfig;

    [Header("Autostop")]
    [SerializeField] private Image _autostopBg;
    [SerializeField] private Sprite _autostopActiveSprite;
    [SerializeField] private Sprite _autostopInactiveSprite;
    [SerializeField] private TMP_Text _autostopTimeText;
    [SerializeField] private Color _autostopActiveColor;
    [SerializeField] private Color _autostopInactiveColor;

    private Coroutine _autostopCoroutine;
    private int _autostopTime; 

    [Header("CurrentPreset")]
    [SerializeField] private TMP_Text _currentPresetName;
    [SerializeField] private TMP_Text _currentPresetDescription;
    [SerializeField] private RawImage _presetImage;
    [SerializeField] private Texture2D _defaultPresetImage;
    [SerializeField] private Image[] _presetSounds;
    [SerializeField] private Button _nextPresetButton;
    [SerializeField] private Button _previousPresetButton;
    private int _currentPresetIndex = 0;

    [Header("Footer")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _profileButton;
    [SerializeField] private Button _createPresetButton;
    [SerializeField] private Button _libraryButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Sprite _playButtonActive;
    [SerializeField] private Sprite _playButtonInactive;

    private bool _isPlaying = false;    

    private TextManager textManager = new TextManager();

    private PresetData currentPreset;


    private void Start()
    {
        _nextPresetButton.onClick.AddListener(NextPreset);
        _previousPresetButton.onClick.AddListener(PreviousPreset);
        _settingsButton.onClick.AddListener(Settings);
        _profileButton.onClick.AddListener(Profile);
        _createPresetButton.onClick.AddListener(CreatePreset);
        _libraryButton.onClick.AddListener(Library);
        _playButton.onClick.AddListener(SwitchMusic);

    }

    private void OnDestroy()
    {
        _nextPresetButton.onClick.RemoveListener(NextPreset);
        _previousPresetButton.onClick.RemoveListener(PreviousPreset);   
        _settingsButton.onClick.RemoveListener(Settings);
        _profileButton.onClick.RemoveListener(Profile);
        _createPresetButton.onClick.RemoveListener(CreatePreset);
        _libraryButton.onClick.RemoveListener(Library);
        _playButton.onClick.RemoveListener(SwitchMusic);
    }

    public override void ResetScreen()
    {
        _isPlaying = false;
        Debug.Log("Stop");
        _playButton.gameObject.GetComponent<Image>().sprite = _playButtonInactive;
        StopAllCoroutines();
        GameManager.instance.StopSounds();
    }

    public override void SetScreen()
    {
        _currentPresetIndex = 0;
        _autostopTime = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop);
        SetAutostop();
        SetCurrentPreset();
    }

    private void SetAutostop()
    {
        textManager.SetTimerText(_autostopTimeText, _autostopTime, showMinutes: true);
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop) > 0)
        {
            _autostopBg.sprite = _autostopActiveSprite;
            _autostopTimeText.color = _autostopActiveColor;
        }
        else
        {
            _autostopBg.sprite = _autostopInactiveSprite;
            _autostopTimeText.color = _autostopInactiveColor;
        }
    }

    private void SetCurrentPreset()
    {
        currentPreset = GameManager.instance.Presets[_currentPresetIndex];

        GameManager.instance.SetSounds(currentPreset.presetName);

        
        _currentPresetName.text = currentPreset.presetName;
        _currentPresetDescription.text = currentPreset.presetDesciption;
        _presetImageManager.SetSavedPicture(currentPreset.presetSpriteAdres, _presetImage, _defaultPresetImage);

        for (int i = 0; i < _presetSounds.Length; i++)
        {
            if (currentPreset.volume[i] > 0)
            {
                SoundData soundData = GetSoundData(currentPreset.sounds[i]);
                _presetSounds[i].sprite = soundData.pinkSoundIcon;
            }
            else
            {
                SoundData soundData = GetSoundData(currentPreset.sounds[i]);
                _presetSounds[i].sprite = soundData.blackSoundIcon;
            }
        }
    }

    private SoundData GetSoundData(SoundTypes sound)
    {
        for(int i = 0; i < _gameConfig.sounds.Length; i++)
        {
            if (_gameConfig.sounds[i].soundType == sound)
            {
                return _gameConfig.sounds[i];
            }
        }
        return null;
    }

    private void SwitchMusic()
    {
        Vibrate();
        _isPlaying =! _isPlaying;
        textManager.SetTimerText(_autostopTimeText,_autostopTime, showMinutes: true);
        if (_isPlaying)
        {
            Debug.Log("Start");
            _playButton.gameObject.GetComponent<Image>().sprite = _playButtonActive;
            if(SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop, 0) > 0)
                _autostopCoroutine = StartCoroutine(Autostop());
            GameManager.instance.PlaySounds();
        }
        else
        {
            Debug.Log("Stop");
            _playButton.gameObject.GetComponent<Image>().sprite = _playButtonInactive;
            if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop, 0) > 0)
                StopAllCoroutines();
            GameManager.instance.StopSounds();
        }
    }

    private void NextPreset()
    {
        if(_currentPresetIndex < GameManager.instance.Presets.Count - 1)
        {
            _currentPresetIndex++;
            SetCurrentPreset();

            _isPlaying = false;
            _playButton.gameObject.GetComponent<Image>().sprite = _playButtonInactive;
            if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop, 0) > 0)
                StopAllCoroutines();
            GameManager.instance.StopSounds();
        }
    }

    private void PreviousPreset()
    {
        if(_currentPresetIndex > 0)
        {
            _currentPresetIndex--;
            SetCurrentPreset();

            _isPlaying = false;
            _playButton.gameObject.GetComponent<Image>().sprite = _playButtonInactive;
            if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop, 0) > 0)
                StopAllCoroutines();
            GameManager.instance.StopSounds();
        }
    }

    private void Settings()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Settings);
    }

    private void Profile()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Profile);
    }
    private void CreatePreset()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.PresetCreator);
    }
    private void Library()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Library);
    }

    private IEnumerator Autostop()
    {
        int time = _autostopTime;
        while (time > 0)
        {
            textManager.SetTimerText(_autostopTimeText, time, showMinutes: true);
            yield return new WaitForSeconds(1f);
            time--;

            int timeRecord = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalListenTime);
            timeRecord++;
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.TotalListenTime, timeRecord);
        }

        SwitchMusic();
    }

    private void Vibrate()
    {
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Vibro, 1) == 1)
        {
            Debug.Log("Vibrate");
            Handheld.Vibrate();
        }
    }
}
