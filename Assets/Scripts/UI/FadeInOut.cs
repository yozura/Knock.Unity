using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private Image fade_Image = null;
    [Range(0, 1)] public float color_changeSpeed;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    // 이미지의 color의 alpha값이 1에서 0으로 수렴
    public IEnumerator FadeIn()
    {
        Color color = fade_Image.color;
        color.a = 1f;
        fade_Image.color = color;
        while (true)
        {
            fade_Image.color = new Color(0f, 0f, 0f, Mathf.Lerp(fade_Image.color.a, 0f, color_changeSpeed));
            if (fade_Image.color.a <= 0.01f)
            {
                fade_Image.color = new Color(0f, 0f, 0f, 0f);
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        Color color = fade_Image.color;
        color.a = 0f;
        fade_Image.color = color;
        while (true)
        {
            fade_Image.color = new Color(0f, 0f, 0f, Mathf.Lerp(fade_Image.color.a, 1f, color_changeSpeed));
            if (fade_Image.color.a <= 0.01f)
            {
                fade_Image.color = new Color(0f, 0f, 0f, 1f);
                yield break;
            }
            yield return null;
        }
    }
}
