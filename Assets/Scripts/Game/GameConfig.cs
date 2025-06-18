using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public List<PresetData> _defaultPresets;
    public SoundData[] sounds;
    public Sprite[] presetImages;
}
[Serializable]
public class SoundData
{
    public SoundTypes soundType;
    public AudioClip soundClip;
    public Sprite pinkSoundIcon;
    public Sprite blackSoundIcon;
}