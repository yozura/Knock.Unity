using UnityEngine;

public class SlidingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<Animator>().SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<Animator>().SetBool("IsOpen", false);
        }
    }
}
