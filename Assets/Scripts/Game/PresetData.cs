using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PresetData
{
    public string presetName;
    public int presetSpriteIndex;
    public List<SoundTypes> sounds = new();
    public List<float> volume = new();
    public bool isLiked;
}