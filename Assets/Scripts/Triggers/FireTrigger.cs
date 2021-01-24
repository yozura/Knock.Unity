using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    // 필요한 오브젝트
    public GameObject fire = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            fire.SetActive(true);
            SoundManager.instance.PlaySoundEffect("Fire_Ignite");
            PlayerScript.instance.SoundPrint("불이 붙는 소리");
            Destroy(gameObject);
        }
    }
}
