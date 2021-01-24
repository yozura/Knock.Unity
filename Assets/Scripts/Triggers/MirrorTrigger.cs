using UnityEngine;

public class MirrorTrigger : MonoBehaviour
{
    public GameObject darkMirror;
    public bool isMirror;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player") && !isMirror)
        {
            isMirror = true;
            darkMirror.SetActive(true);
            SoundManager.instance.PlaySoundEffect("Panic_Breath");
            SoundManager.instance.PlaySoundEffect("Broken_Glass");
            PlayerScript.instance.SoundPrint("유리가 깨지는 소리");
            Destroy(gameObject, 1f);
        }

    }
}
