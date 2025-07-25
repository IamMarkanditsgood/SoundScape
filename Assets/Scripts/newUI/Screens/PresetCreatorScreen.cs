using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetCreatorScreen : BasicScreen
{
    [SerializeField] private Button backButton;
    [SerializeField] private PresetIamgeManager presetImageManager;
    [SerializeField] private RawImage image;
    [SerializeField] private Texture2D defaultImage;
    [SerializeField] private Button setImage;
    [SerializeField] private Button savePreset;
    [SerializeField] private TMP_InputField _presetName;
    [SerializeField] private TMP_InputField _presetDescription;
    [SerializeField] private List<SoundTypes> SoundTypes;
    [SerializeField] private Slider[] sliders;
    [SerializeField] private Image[] icons;

    [SerializeField] protected GameConfig gameConfig;

    private string presetImageAdres;

    private void Start()
    {
        setImage.onClick.AddListener(PresetImage);
        savePreset.onClick.AddListener(Save);
        backButton.onClick.AddListener(Back);

        for(int i = 0; i < sliders.Length; i++) 
        {
            int index = i;
            sliders[index].onValueChanged.AddListener((value) => SliderChanged(index, value));
        }
    }

    private void OnDestroy()
    {
        setImage.onClick.RemoveListener(PresetImage);
        savePreset.onClick.RemoveListener(Save);
        backButton.onClick.RemoveListener(Back);

        for (int i = 0; i < sliders.Length; i++)
        {
            int index = i;
            sliders[index].onValueChanged.RemoveListener((value) => SliderChanged(index, value));
        }
    }
    public override void ResetScreen()
    {


    }

    public override void SetScreen()
    {
        foreach(var slider in sliders)
        {
            slider.value = 0f;
        }
        image.texture = defaultImage;
        _presetName.text = string.Empty;
        _presetDescription.text = string.Empty;
        presetImageAdres = string.Empty;
    }

    private void PresetImage()
    {
        presetImageAdres =  presetImageManager.PickFromGallery(image, defaultImage);
    }

    private void Save()
    {
        PresetData newPreset = new PresetData();
        newPreset.presetName = GetUniqueName(_presetName.text);
        newPreset.presetDesciption = _presetDescription.text;
        newPreset.presetSpriteAdres = presetImageAdres;
        newPreset.sounds = SoundTypes;

        List<float> volume = new List<float>();
        for(int i = 0;i< sliders.Length; i++)
        {
            volume.Add(sliders[i].value);
        }
        newPreset.volume = volume;
        GameManager.instance.SetNewPreset(newPreset);

        Back();
    }

    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }

    private string GetUniqueName(string currentName)
    {
        string baseName = currentName;
        if (currentName == string.Empty)
        {
            baseName = "Default";
        }
        // Clean input
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
    private void SliderChanged(int index, float value)
    {
        if(value > 0)
        {
            icons[index].sprite = GetSoundData(SoundTypes[index]).pinkSoundIcon;
        }
        else
        {
            icons[index].sprite = GetSoundData(SoundTypes[index]).blackSoundIcon;
        }
    }

    private SoundData GetSoundData(SoundTypes sound)
    {
        for (int i = 0; i < gameConfig.sounds.Length; i++)
        {
            if (gameConfig.sounds[i].soundType == sound)
            {
                return gameConfig.sounds[i];
            }
        }
        return null;
    }
}
