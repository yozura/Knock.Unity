using UnityEngine;

public class ToiletMirrorTrigger : MonoBehaviour
{
    private Inventory theInven;
    private FurnitureAction furAct;

    private bool isDone;

    void Start()
    {
        furAct = FindObjectOfType<FurnitureAction>();
        theInven = FindObjectOfType<Inventory>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && theInven.c_NoteSlots[4].isAcquired && !isDone)
        {
            isDone = true;
            SoundManager.instance.PlaySoundEffect("Close_Door");
            furAct.toilet_Door00.GetComponent<Animator>().SetBool("IsOpen", false);
            furAct.toilet_Door00.GetComponent<OpenManager>().isLocked = true;
            
            furAct.toilet_Glass.SetActive(true);
            furAct.toilet_Key.SetActive(true);
            furAct.toilet_Door01.GetComponent<OpenManager>().isLocked = false;
            SoundManager.instance.PlaySoundEffect("Panic_Breath");
            SoundManager.instance.PlaySoundEffect("Broken_Glass");
            PlayerScript.instance.SoundPrint("유리가 깨지는 소리");
            Destroy(gameObject, 1f);
        }
    }
}
