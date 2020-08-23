using System.Collections;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject scope;

    [SerializeField]
    private GunController gc = null;

    public IEnumerator ScopeActivate()
    {
        yield return new WaitForSeconds(0.5f);
        gc.GetGun().gameObject.layer = 2;
        playerCam.GetComponent<Camera>().fieldOfView = 20;
        scope.SetActive(true);
    }
    
    public IEnumerator ScopeDeActivate()
    {
        scope.SetActive(false);
        playerCam.GetComponent<Camera>().fieldOfView = 60;
        gc.GetGun().gameObject.layer = 8;
        yield return new WaitForSeconds(0.01f);
    }
}
