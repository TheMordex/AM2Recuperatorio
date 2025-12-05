using UnityEngine;
using System.Collections.Generic;

public class PowerupUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform iconsContainer;
    
    [Header("Layout Settings")]
    [SerializeField] private float iconSpacing = 10f;
    [SerializeField] private float iconSize = 50f;
    
    private Dictionary<string, PowerupIconUI> activeIcons = new Dictionary<string, PowerupIconUI>();
    
    private static PowerupUIManager instance;
    public static PowerupUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PowerupUIManager>();
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    
    public void AddPowerupIcon(string powerupType, Sprite icon, float duration)
    {
        if (activeIcons.ContainsKey(powerupType))
        {
            RemovePowerupIcon(powerupType);
        }
        
        GameObject iconObj = Instantiate(iconPrefab, iconsContainer);
        PowerupIconUI iconUI = iconObj.GetComponent<PowerupIconUI>();
        
        if (iconUI != null)
        {
            iconUI.Initialize(icon, duration);
            activeIcons[powerupType] = iconUI;
            
            RepositionIcons();
        }
    }
    
    public void RemovePowerupIcon(string powerupType)
    {
        if (activeIcons.ContainsKey(powerupType))
        {
            PowerupIconUI icon = activeIcons[powerupType];
            if (icon != null)
                icon.ForceRemove();
            
            activeIcons.Remove(powerupType);
            RepositionIcons();
        }
    }
    
    private void RepositionIcons()
    {
        int index = 0;
        foreach (var kvp in activeIcons)
        {
            if (kvp.Value != null)
            {
                RectTransform rt = kvp.Value.GetComponent<RectTransform>();
                if (rt != null)
                {
                    float xPos = index * (iconSize + iconSpacing);
                    rt.anchoredPosition = new Vector2(xPos, 0f);
                }
                index++;
            }
        }
    }
    
    public bool HasActivePowerup(string powerupType)
    {
        return activeIcons.ContainsKey(powerupType) && activeIcons[powerupType] != null;
    }
    
    public void ClearAllIcons()
    {
        foreach (var kvp in activeIcons)
        {
            if (kvp.Value != null)
                kvp.Value.ForceRemove();
        }
        activeIcons.Clear();
    }
}