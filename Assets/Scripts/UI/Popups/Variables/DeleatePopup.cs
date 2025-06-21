using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleatePopup : BasicPopup
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Button _saveButton;
    public Library library;

    private string _presetName;

    private bool _unlike;

    public void Init(string presetName, bool unlike)
    {
        _presetName = presetName;
        _unlike = unlike;
    }

    public override void Subscribe()
    {
        base.Subscribe();
        _saveButton.onClick.AddListener(Deleate);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _saveButton.onClick.RemoveListener(Deleate);
    }
    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
        _name.text = _presetName;
    }

    private void Deleate()
    {
        if (!_unlike)
        {
            GameManager.instance.DeletePreset(_presetName);
            library.DeleatPreset(_presetName, false);
        }
        else
        {
            List<string> liked = SaveManager.PlayerPrefs.LoadStringList(GameSaveKeys.LikedPreset);

            liked.Remove(_presetName);
            SaveManager.PlayerPrefs.SaveStringList(GameSaveKeys.LikedPreset, liked);

            library.DeleatPreset(_presetName, true);
        }
        
        Hide();
    }
}
