using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayer : BasicPopup
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _timer;

    [SerializeField] private Image _presetImage;

    [SerializeField] private Button _likeButton;
    [SerializeField] private Button _playButton;

    [SerializeField] private Sprite _playButtonOn;
    [SerializeField] private Sprite _playButtonOff;

    [SerializeField] private GameObject[] _icons;

    [SerializeField] private GameConfig gameConfig;

    private PresetData _currentPreset;
    private TextManager _textManager = new TextManager();

    private bool _isPlaying;
    private int seconds;

    private Coroutine timer;

    public override void Subscribe()
    {
        base.Subscribe();
        _playButton.onClick.AddListener(Play);
        _likeButton.onClick.AddListener(Like);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(Play);
        _likeButton.onClick.RemoveListener(Like);
    }

    public void Init(string currentPreset)
    {
        _currentPreset = GameManager.instance.GetPresetData(currentPreset);
    }

    public override void ResetPopup()
    {
        _playButton.gameObject.GetComponent<Image>().sprite = _playButtonOn;

        _isPlaying = false;

        GameManager.instance.StopSounds();

        StopAllCoroutines();
        timer = null;

        int totalTime = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.TotalListenTime) + seconds;
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.TotalListenTime, totalTime);


        seconds = 0; 
        foreach (var icon in _icons)
        {
            icon.GetComponent<Image>().enabled = false;
        }
    }

    public override void SetPopup()
    {
        _name.text = _currentPreset.presetName;
        _timer.text = "00:00:00";
        _presetImage.sprite = gameConfig.presetImages[_currentPreset.presetSpriteIndex];

        SetSounds();
    }

    private void SetSounds()
    {
        List<SoundTypes> soundType = _currentPreset.sounds;

        for(int i = 0; i < soundType.Count; i++)
        {
            _icons[i].GetComponent<Image>().enabled = true;
            _icons[i].GetComponent<Image>().sprite = gameConfig.GetSoundData(soundType[i]).blackSoundIcon;
        }

        GameManager.instance.SetSounds(_currentPreset.presetName);
    }   

    private void Like()
    {
        List<string> likedPresets = SaveManager.PlayerPrefs.LoadStringList(GameSaveKeys.LikedPreset);
        foreach (string presetName in likedPresets)
        {
            if(presetName == _currentPreset.presetName)
                return;
        }
        likedPresets.Add(_currentPreset.presetName);
        SaveManager.PlayerPrefs.SaveStringList(GameSaveKeys.LikedPreset, likedPresets);

        Library library = (Library)UIManager.Instance.GetScreen(ScreenTypes.Library);
        library.SetLikedPresets();
    }

    private void Play()
    {
        Vibrate();
        if (timer == null)
        {
            timer = StartCoroutine(Timer());
        }

        if(_isPlaying)
        {
            _playButton.gameObject.GetComponent<Image>().sprite = _playButtonOn;
            _isPlaying = false;
            GameManager.instance.StopSounds();
        }
        else
        {
            _playButton.gameObject.GetComponent<Image>().sprite = _playButtonOff;
            _isPlaying = true;
            GameManager.instance.PlaySounds();
        }
    }

    private IEnumerator Timer()
    {
        while(true)
        {
            if(_isPlaying)
            {
                seconds++;
                _textManager.SetTimerText(_timer, seconds, showMinutes: true, showHours: true);
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void Vibrate()
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }
}
