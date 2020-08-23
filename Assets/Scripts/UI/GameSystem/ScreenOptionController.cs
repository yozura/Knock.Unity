using UnityEngine;

public class ScreenOptionController : MonoBehaviour
{
    public bool isFullScreen = false;

    public void FullScreen() { isFullScreen = !isFullScreen; SoundManager.instance.PlaySoundEffect("Button_Click"); }
    
    // 1920 x 1080
    public void OnClickSetScreenFHD()
    {
        Screen.SetResolution(1920, 1080, isFullScreen);
        SoundManager.instance.PlaySoundEffect("Button_Click");
    }

    // 1600x900
    public void OnClickSetScreenWSXGA()
    {
        Screen.SetResolution(1600, 900, isFullScreen);
        SoundManager.instance.PlaySoundEffect("Button_Click");
    }

    // 1280 x 720
    public void OnClickSetScreenHD()
    {
        Screen.SetResolution(1280, 720, isFullScreen);
        SoundManager.instance.PlaySoundEffect("Button_Click");
    }

}
