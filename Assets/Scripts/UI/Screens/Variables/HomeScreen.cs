using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BasicScreen
{
    [SerializeField] private Transform _content;
    [SerializeField] private DefaultSoundPanel _soundPanelPref;
    [SerializeField] private GameConfig _gameConfig;

    [SerializeField] private List<DefaultSoundPanel> _panels;

    public override void ResetScreen()
    {
        for (int i = 0; i < _panels.Count; i++)
        {
            int index = i;
            _panels[index].gameObject.GetComponent<Button>().onClick.RemoveListener(() => PresetPressed(_panels[index].presetName));
            Destroy(_panels[index].gameObject);
        }
        _panels.Clear();
    }

    public override void SetScreen()
    {
        ResetScreen();
        ConfigScreen();
    }

    private void ConfigScreen()
    {
        for(int i = 0; i < _gameConfig._defaultPresets.Count; i++)
        {
            DefaultSoundPanel panel = Instantiate(_soundPanelPref, _content);
            panel = SetPanel(panel, _gameConfig._defaultPresets[i]);
            _panels.Add(panel);

            int index = i;
            panel.gameObject.GetComponent<Button>().onClick.AddListener(() => PresetPressed(_panels[index].presetName));
        }
    }

    private DefaultSoundPanel SetPanel(DefaultSoundPanel panel, PresetData presetData )
    {
        List<Sprite> icons = new();
        for(int i = 0; i< presetData.sounds.Count; i++)
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
}
