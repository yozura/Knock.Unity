using System.Collections;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    Light[] theLight = null;
    
    // 1단서를 주우면 발동 -- Inventory;
    // 스위치를 누르면 해제 -- furaction;

    public IEnumerator Flash()
    {
        int _random = Random.Range(0, 3);

        if (_random == 0)
        {
            theLight[0].intensity = 0.0f;
            theLight[1].intensity = 0.0f;
        }
        else
        {
            theLight[0].intensity = 0.8f;   
            theLight[1].intensity = 0.8f;
        }

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(Flash());
    }
}
