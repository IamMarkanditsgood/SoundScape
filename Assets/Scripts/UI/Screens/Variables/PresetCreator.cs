using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetCreator : BasicScreen
{
    [Serializable]
    public class SoundData
    {
        public SoundTypes soundType;
        public Button button;
        public Image bg;
        public AudioSource audioSource;
        public Slider slider;
        public bool isActive = false;
    }

    [SerializeField] private SoundData[] sounds;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Sprite _activePanel;
    [SerializeField] private Sprite _deactivePanel;

    private void Start()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            int index = i;
            sounds[index].button.onClick.AddListener(() => SoundPanelPressed(sounds[index]));

            sounds[index].slider.onValueChanged.AddListener((value) => SliderChanged(sounds[index], value));
        }

        _saveButton.onClick.AddListener(Save);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            int index = i;
            sounds[index].button.onClick.RemoveListener(() => SoundPanelPressed(sounds[index]));

            sounds[index].slider.onValueChanged.RemoveAllListeners();
        }

        _saveButton.onClick.RemoveListener(Save);
    }

    public override void ResetScreen()
    {
        foreach (var sound in sounds)
        {
            sound.audioSource.Stop();
            sound.slider.value = 0.5f;
            sound.slider.gameObject.SetActive(false);
            sound.bg.sprite = _deactivePanel;
            sound.isActive = false;

        }
    }

    public override void SetScreen()
    {
    }

    private void SoundPanelPressed(SoundData panele)
    {
        if (!panele.isActive)
        {
            panele.audioSource.volume = panele.slider.value;
            panele.audioSource.Play();
            panele.slider.gameObject.SetActive(true);
            panele.bg.sprite = _activePanel;
            panele.isActive = true;
        }
        else
        {
            panele.audioSource.Stop();
            panele.slider.gameObject.SetActive(false);
            panele.bg.sprite = _deactivePanel;
            panele.isActive = false;
        }
    }

    private void SliderChanged(SoundData panele, float value)
    {
        if (value == 0)
        {
            SoundPanelPressed(panele);
        }
        else
        {
            panele.audioSource.volume = value;
        }
    }

    private void Save()
    {
        PresetSaver presetSaver = (PresetSaver)UIManager.Instance.GetPopup(PopupTypes.PresetSaver);

        List<SoundTypes> savedSounds = new();
        List<float> soundsVolume = new();

        foreach(var sound in sounds)
        {
            if(sound.isActive)
            {
                savedSounds.Add(sound.soundType);
                soundsVolume.Add(sound.audioSource.volume);
            }
        }

        if (savedSounds.Count > 0 && soundsVolume.Count == savedSounds.Count)
        {
            presetSaver.Init(savedSounds, soundsVolume);
            presetSaver.Show();

        }
    }

    private void SliderChanged(int index)
    {

    }
}
