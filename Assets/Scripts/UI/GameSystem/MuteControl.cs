using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MuteControl : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Sprite mute_on;
    public Sprite mute_off;
    public Slider bgmSlider;
    public Slider effectSlider;

    public bool isBgmMute;
    public bool isEftMute;

    private Image mute_Image;

    void Start()
    {
        mute_Image = GetComponent<Image>();
        mute_Image.sprite = mute_off;
    }

    public void OnClickBGMMute()
    {
        float sound = bgmSlider.value;
        isBgmMute = !isBgmMute;
        if (isBgmMute || sound == -40f)
        {
            mute_Image.sprite = mute_on;
            masterMixer.SetFloat("BGM_Sound", -80f);
        }
        else
        {
            mute_Image.sprite = mute_off;
            masterMixer.SetFloat("BGM_Sound", sound);
        }
    }

    public void OnClickEffectMute()
    {
        float sound = effectSlider.value;
        isEftMute = !isEftMute;
        if (isEftMute || sound == -40f)
        {
            mute_Image.sprite = mute_on;
            masterMixer.SetFloat("Effect_Sound", -80f);
        }
        else
        {
            mute_Image.sprite = mute_off;
            masterMixer.SetFloat("Effect_Sound", sound);
        }
    }

}
