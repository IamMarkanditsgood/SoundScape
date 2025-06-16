using System.Collections.Generic;
using UnityEngine;

public class ResourceSaveManager
{
    private readonly Dictionary<ResourceTypes, string> ResourceKeys = new Dictionary<ResourceTypes, string>
        {
            { ResourceTypes.Coins, GameSaveKeys.Coins },
            { ResourceTypes.Score, GameSaveKeys.Score },
             { ResourceTypes.SkillPoints, GameSaveKeys.CurrentSkillPoints },
        };

    public void SaveResource(ResourceTypes resource, float amount)
    {
        if (ResourceKeys.TryGetValue(resource, out string key))
        {
            SaveManager.PlayerPrefs.SaveFloat(key, amount);

        }
        else
        {
            Debug.LogWarning($"Invalid resource type: {resource}");
        }
    }

    public float LoadResource(ResourceTypes resource)
    {
        if (ResourceKeys.TryGetValue(resource, out string key) && SaveManager.PlayerPrefs.IsSaved(key))
        {
            return SaveManager.PlayerPrefs.LoadFloat(key, 0f);
        }

        Debug.LogWarning($"Resource not found or not saved: {resource}");
        return 0;
    }
}