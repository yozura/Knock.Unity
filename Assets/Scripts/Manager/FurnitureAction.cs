using System.Collections;
using UnityEngine;

public class FurnitureAction : MonoBehaviour
{
    #region Objects
    public GameObject lightsGroup;
    public GameObject art01;
    public GameObject first_Phase;
    public GameObject second_Phase;
    public Light[] theLight;
    public GameObject[] light_Mesh;
    public GameObject k_Draw3;
    public GameObject toilet_Door00;
    public GameObject note04;
    public GameObject toilet_Glass;
    public GameObject toilet_Key;
    public GameObject toilet_Door01;
    public Light tv_Light;
    public GameObject key_end;
    public GameObject book05;
    #endregion

    #region Components
    private Inventory theinven;
    public MeshRenderer tv_MeshRenderer;
    #endregion

    #region Boolean
    private bool isDone = false;
    private bool isDrawOpen = false;
    private bool isDoorOpen = false;
    private bool isTvOnOff = false;
    #endregion
    
    private const string lightStatic = "Texture/LightStatic/";
    public IEnumerator fl_Coroutine;
    public IEnumerator kn_Coroutine;

    public void Start()
    {
        theinven = FindObjectOfType<Inventory>();
        fl_Coroutine = Flash();
        kn_Coroutine = KnockDoor();
    }

    private void Update()
    {
        if (k_Draw3.GetComponent<OpenManager>().isOpen && !isDone)
        {
            StartCoroutine(KitchenDoorOddTrigger());
            second_Phase.SetActive(true);
        }
    }

    public void DrawAction(GameObject go_draw)
    {
        if (go_draw.GetComponent<OpenManager>().hasKey)
        {
            theinven.c_ETCSlots[0].isAcquired = false;
            go_draw.GetComponent<OpenManager>().hasKey = false;
            PlayerScript.instance.SoundPrint("잠금장치가 풀리는 소리");
            PlayerScript.instance.DesPrint("열쇠가 부러졌다.");
        }
        else
        {
            if (!go_draw.GetComponent<OpenManager>().isOpen)
            {
                isDrawOpen = true;
                SoundManager.instance.PlaySoundEffect("Open_Draw");
                PlayerScript.instance.SoundPrint("서랍 열리는 소리");
                go_draw.GetComponent<Animator>().SetBool("IsOpen", isDrawOpen);
                go_draw.GetComponent<OpenManager>().isOpen = true;
                go_draw.GetComponent<OpenManager>().hasKey = false;
            }
            else
            {
                isDrawOpen = false;
                SoundManager.instance.PlaySoundEffect("Close_Draw");
                PlayerScript.instance.SoundPrint("서랍 닫히는 소리");
                go_draw.GetComponent<Animator>().SetBool("IsOpen", isDrawOpen);
                go_draw.GetComponent<OpenManager>().isOpen = false;
            }
        }
    }

    public void PathAction(GameObject go_Path)
    {
        if(go_Path.GetComponent<OpenManager>().hasKey)
            ObjectManager.instance.KineToDyna(note04);
        DoorAction(go_Path);
        StopCoroutine("KnockDoor");
    }

    public void Path1Action(GameObject go_Path1)
    { 
        DoorAction(go_Path1);
    }

    public void DoorAction(GameObject go_door)
    {
        if (go_door.GetComponent<OpenManager>().hasKey)
        {
            theinven.c_ETCSlots[0].isAcquired = false;
            go_door.GetComponent<OpenManager>().hasKey = false;
            PlayerScript.instance.SoundPrint("잠금장치가 풀리는 소리");
            PlayerScript.instance.DesPrint("열쇠가 부러졌다.");
        }
        else
        {
            if (!go_door.GetComponent<OpenManager>().isOpen)
            {
                isDoorOpen = true;
                SoundManager.instance.PlaySoundEffect("Open_Door");
                PlayerScript.instance.SoundPrint("문 열리는 소리");
                go_door.GetComponent<Animator>().SetBool("IsOpen", isDoorOpen);
                go_door.GetComponent<OpenManager>().isOpen = true;
                go_door.GetComponent<OpenManager>().hasKey = false;
            }
            else
            {
                isDoorOpen = false;
                SoundManager.instance.PlaySoundEffect("Close_Door");
                PlayerScript.instance.SoundPrint("문 닫히는 소리");
                go_door.GetComponent<Animator>().SetBool("IsOpen", isDoorOpen);
                go_door.GetComponent<OpenManager>().isOpen = false;
            }
        }
    }

    public void SwitchAction(GameObject go_Switch)
    {
        if (theinven.c_ETCSlots[1].isAcquired)
        {
            if (go_Switch.GetComponent<OpenManager>().isSwitchOn)
            {
                StopCoroutine("Flash");
                lightsGroup.SetActive(false);
                go_Switch.GetComponent<OpenManager>().isSwitchOn = false;

                SoundManager.instance.PlaySoundEffect("Switch_Off");
                SoundManager.instance.PlaySoundEffect("First_Phase");
                PlayerScript.instance.SoundPrint("천둥 치는 소리");
                PlayerScript.instance.DesPrint("무언가 떨어졌다.");
                PlayerScript.instance.NPCPrint("전등이 꺼졌다.");
                ObjectManager.instance.KineToDyna(art01);        
                SoundManager.instance.PlaySoundEffect("ArtFrame_Drop");
                StartCoroutine(kn_Coroutine);
                first_Phase.SetActive(true);
            }
            else
            {
                SoundManager.instance.PlaySoundEffect("Switch_On");
                PlayerScript.instance.DesPrint("더 이상 작동하지 않는다.");
            }
        }
        else
        {
            SoundManager.instance.PlaySoundEffect("Switch_Off");
            PlayerScript.instance.DesPrint("작동하지 않는다.");
        }
    }

    public void ChairAction(GameObject go_Chair)
    {
        PlayerScript.instance.SoundPrint("바닥이 끌리는 소리");
        PlayerScript.instance.DesPrint("의자를 옆으로 밀어냈다.");
        SoundManager.instance.PlaySoundEffect("Dragging_Chair");
        go_Chair.GetComponent<Animator>().SetTrigger("IsMoving");
        go_Chair.GetComponent<OpenManager>().isMoving = true;
    }

    public void PillowAction(GameObject go_Pillow)
    {
        PlayerScript.instance.DesPrint("베개를 옮겼다");
        go_Pillow.GetComponent<Animator>().SetTrigger("IsMoving");
        go_Pillow.GetComponent<OpenManager>().isMoving = true;
    }

    public void TVAction(GameObject go_TV)
    {
        if(!go_TV.GetComponent<OpenManager>().isSwitchOn)
        {
            isTvOnOff = true;
            go_TV.GetComponent<OpenManager>().isSwitchOn = true;
            PlayerScript.instance.SoundPrint("TV 소리");
            PlayerScript.instance.DesPrint("TV를 켰다.");
            SoundManager.instance.PlaySoundBGM("TV_LightStatic");
            tv_Light.gameObject.SetActive(true);
            StartCoroutine("SetMaterials");
        }
        else
        {
            isTvOnOff = false;
            tv_Light.gameObject.SetActive(false);
            go_TV.GetComponent<OpenManager>().isSwitchOn = false;
            PlayerScript.instance.DesPrint("TV를 껐다");
            SoundManager.instance.StopBGM("TV_LightStatic");
        }
    }

    IEnumerator KnockDoor()
    {
        yield return new WaitForSeconds(1f);
        int _random = Random.Range(0, 4);

        if(_random == 0)
            SoundManager.instance.PlaySoundEffect("Hard_Knocking");
        else
            SoundManager.instance.PlaySoundEffect("Soft_Knocking");

        PlayerScript.instance.SoundPrint("문 두드리는 소리");
        PlayerScript.instance.NPCPrint("누군가 문을 두드린다.");
        yield return new WaitForSeconds(3f);
        StartCoroutine("KnockDoor");
    }

    // 첫번쨰 단서 획득시 켜짐
    public IEnumerator Flash()
    {
        int _random = Random.Range(0, 5);

        if (_random == 0)
        {
            for (int i = 0; i < theLight.Length; i++)
            {
                theLight[i].intensity = 0.3f;
                light_Mesh[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.gray * -5.0f);
            }
        }
        else
        {
            for (int i = 0; i < theLight.Length; i++)
            {
                theLight[i].intensity = 0.8f;
                light_Mesh[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white * 3.0f);
            }
        }

        yield return new WaitForSeconds(0.2f);

        StartCoroutine("Flash");
    }

    public IEnumerator KitchenDoorOddTrigger()
    {
        yield return new WaitForEndOfFrame();
        isDone = true;
        toilet_Door00.GetComponent<OpenManager>().hasKey = false;
        SoundManager.instance.PlaySoundEffect("Key_Open");
        toilet_Door00.GetComponent<OpenManager>().isOpen = true;
        toilet_Door00.GetComponent<Animator>().SetBool("IsOpen", isDone);
        SoundManager.instance.PlaySoundEffect("Open_Door");
        PlayerScript.instance.SoundPrint("문 열리는 소리");
        PlayerScript.instance.DesPrint("화장실 문이 열렸다.");
    }

    public IEnumerator SetMaterials()
    {
        while(true)
        {
            int _random = Random.Range(0, 4);
            if(_random == 0)
                tv_MeshRenderer.material = ObjectManager.instance.GetMatrialDic(lightStatic + "LightStatic_01");
            else if(_random == 1)
                tv_MeshRenderer.material = ObjectManager.instance.GetMatrialDic(lightStatic + "LightStatic_02");
            else if(_random == 2)
                tv_MeshRenderer.material = ObjectManager.instance.GetMatrialDic(lightStatic + "LightStatic_03");
            else if(_random == 3)
                tv_MeshRenderer.material = ObjectManager.instance.GetMatrialDic(lightStatic + "LightStatic_04");

            if (!isTvOnOff)
            {
                tv_MeshRenderer.material = ObjectManager.instance.GetMatrialDic("Prop/Item/FlashLight/Materials/FlashlightReflectorMaterial");
                yield break;
            }

            yield return null; 
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}