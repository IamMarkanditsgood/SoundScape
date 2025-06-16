using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcesManager
{
    public static ResourcesManager Instance { get; private set; }

    public event Action<ResourceTypes, float> OnResourceModified;

    private Dictionary<ResourceTypes, float> _resources = new Dictionary<ResourceTypes, float>();

    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitResourceDictionary(); 
    }

    public void Destroy()
    {
        if (Instance != null)
        {
            Instance = null;    
        }
    }

    public float GetResource(ResourceTypes resource)
    {
        return _resources[resource];
    }

    public void ModifyResource(ResourceTypes resource, float updateAmount, bool changeAmount = false)
    {
        if (!changeAmount)
        {
            _resources[resource] += updateAmount;
        }
        else
        {
            _resources[resource] = updateAmount;
        }
        SaveManager.Resources.SaveResource(resource, _resources[resource]);
        OnResourceModified?.Invoke(resource, _resources[resource]);
    }

    public bool IsEnoughResource(ResourceTypes resource, int price)
    {

        if (_resources[resource] >= price)
        {
            return true;
        }

        return false;
    }

    private void InitResourceDictionary()
    {
        _resources[ResourceTypes.Coins] = SaveManager.Resources.LoadResource(ResourceTypes.Coins);
        _resources[ResourceTypes.Score] = SaveManager.Resources.LoadResource(ResourceTypes.Score);
        _resources[ResourceTypes.SkillPoints] = SaveManager.Resources.LoadResource(ResourceTypes.SkillPoints);
    }

    
}