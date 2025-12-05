using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerupIconUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image timerFillImage;
    
    [Header("Blink Settings")]
    [SerializeField] private float blinkThreshold = 3f; 
    [SerializeField] private float blinkSpeed = 0.2f;
    
    private float duration;
    private float elapsed;
    private bool isActive;
    private Coroutine updateCoroutine;
    
    public void Initialize(Sprite icon, float effectDuration)
    {
        if (iconImage != null)
            iconImage.sprite = icon;
        
        duration = effectDuration;
        elapsed = 0f;
        isActive = true;
        
        if (timerFillImage != null)
            timerFillImage.fillAmount = 1f;
        
        if (updateCoroutine != null)
            StopCoroutine(updateCoroutine);
        
        updateCoroutine = StartCoroutine(UpdateTimer());
    }
    
    private IEnumerator UpdateTimer()
    {
        while (elapsed < duration && isActive)
        {
            elapsed += Time.deltaTime;
            
            if (timerFillImage != null)
            {
                timerFillImage.fillAmount = 1f - (elapsed / duration);
            }
            
            float remaining = duration - elapsed;
            if (remaining <= blinkThreshold)
            {
                float alpha = Mathf.PingPong(Time.time / blinkSpeed, 1f);
                SetAlpha(alpha);
            }
            
            yield return null;
        }
        
        yield return StartCoroutine(FadeOut());
        Destroy(gameObject);
    }
    
    private IEnumerator FadeOut()
    {
        float fadeTime = 0.3f;
        float timer = 0f;
        
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float alpha = 1f - (timer / fadeTime);
            SetAlpha(alpha);
            
            float scale = Mathf.Lerp(1f, 0.5f, timer / fadeTime);
            transform.localScale = Vector3.one * scale;
            
            yield return null;
        }
    }
    
    private void SetAlpha(float alpha)
    {
        if (iconImage != null)
        {
            Color c = iconImage.color;
            c.a = alpha;
            iconImage.color = c;
        }
        
        if (timerFillImage != null)
        {
            Color c = timerFillImage.color;
            c.a = alpha;
            timerFillImage.color = c;
        }
    }
    
    public void ForceRemove()
    {
        isActive = false;
        if (updateCoroutine != null)
            StopCoroutine(updateCoroutine);
        Destroy(gameObject);
    }
}