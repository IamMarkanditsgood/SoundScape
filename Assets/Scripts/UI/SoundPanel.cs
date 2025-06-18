using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour
{
    public string presetName;
    [SerializeField] private Image _mainImage;
    [SerializeField] private TMP_Text _presetName;
    [SerializeField] private Image[] _soundIcons;

    public void SetPanel(Sprite image, string name, List<Sprite> icons)
    {
        presetName = name;
        _mainImage.sprite = image;
        _presetName.text = name;


        for(int i = 0; i < icons.Count; i++)
        {
            _soundIcons[i].sprite = icons[i];
            _soundIcons[i].gameObject.SetActive(true);
            if(i + 1 == _soundIcons.Length - 1)
                break;
        }

        if(icons.Count > _soundIcons.Length - 1)
            _soundIcons[^1].gameObject.SetActive(true);
        else
            _soundIcons[^1].gameObject.SetActive(false);
    }

}
