using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider bgmSlider;
    public Slider effectSlider;

    public void BGMControl()
    {
        float sound = bgmSlider.value;
        if (sound == -40f)
        {
            masterMixer.SetFloat("BGM_Sound", -80);
        }
        else
        {
            masterMixer.SetFloat("BGM_Sound", sound);
        }
    }

    public void EffectControl()
    {
        float sound = effectSlider.value;
        if (sound == -40f)
        {
            masterMixer.SetFloat("Effect_Sound", -80);
        }
        else
        {
            masterMixer.SetFloat("Effect_Sound", sound);
        }
    }
}
