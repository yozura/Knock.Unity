using UnityEngine;

public class FlashTrigger : MonoBehaviour
{
    public FlashLight fl;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            fl.StartCoroutine(fl.Flash());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            fl.StopAllCoroutines();
        }
    }   

}

