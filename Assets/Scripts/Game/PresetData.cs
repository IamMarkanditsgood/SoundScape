using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PresetData
{
    public string presetName;
    public string presetSpriteAdres;
    public string presetDesciption;
    public List<SoundTypes> sounds = new();
    public List<float> volume = new();
    public bool isLiked;
}