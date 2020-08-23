using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    // 습득 가능한 최대 거리
    [SerializeField] private float range = 0.0f;
    // 습득이 가능할 시 true;
    private bool ActionActivated = false;
    // 충돌체 정보 저장
    private RaycastHit hitInfo;     

    // 레이어 마스크로 상호작용이 가능한 아이템인지 구분
    [SerializeField] private LayerMask layerMask = 0;  

    // 마우스로 해당 아이템에 갖다댔을 때 나오는 툴팁의 배경
    [SerializeField] private GameObject actionPanel = null;
    // 마우스로 해당 아이템에 갖다댔을 때 나오는 툴팁의 텍스트
    [SerializeField] private Text actionText = null;     
    // 아이템을 저장할 인벤토리
    [SerializeField] private Inventory theInventory = null;
    // 조준점을 빨갛게 하기위한 조준점 클래스
    [SerializeField] private Crosshair crosshair_anim = null;
    // 가구들의 트리거를 불러오기 위한 액션 클래스
    private FurnitureAction furAction;

    private void Start()
    {
        // 씬 실행시 캐싱
        furAction = FindObjectOfType<FurnitureAction>();
    }

    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        // 조준점을 아이템에 올린 상태에서 키코드 입력시 메서드 실행
        if (Input.GetKeyDown(KeyCode.E))
        {
            Action();
        }
    }

    // 어떤 종류의 상호작용인지 확인
    private void CheckItem()
    {
        // RayCast를 이용해 어떤건지 확인하고 ray의 결과물인 hit으로 정보 파악
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            // 태그가 종류별로 있으며 종류마다 출력되는 툴팁이 다름.
            if (hitInfo.transform.CompareTag("Item"))
                ItemInfoAppear();
            else if (hitInfo.transform.CompareTag("Draw"))
                FurnitureInfoAppear();
            else if (hitInfo.transform.CompareTag("Door"))
                FurnitureInfoAppear();
            else if (hitInfo.transform.CompareTag("Path"))
                FurnitureInfoAppear();
            else if (hitInfo.transform.CompareTag("Path1"))
                FurnitureInfoAppear();
            else if (hitInfo.transform.CompareTag("Switch"))
                SwitchInfoAppear();
            else if (hitInfo.transform.CompareTag("TV"))
                SwitchInfoAppear();
            else if (hitInfo.transform.CompareTag("Chair"))
                PushInfoAppear();
            else if (hitInfo.transform.CompareTag("Pillow"))
                PushInfoAppear();
            else if (hitInfo.transform.CompareTag("Untagged"))
                InfoDisAppear();
        }
        else
            // 캐치 가능한 레이어가 아닐 경우 툴팁 제거
            InfoDisAppear();
    }

    private void ItemInfoAppear()
    {
        ActionActivated = true;
        actionPanel.gameObject.SetActive(true);
        actionText.gameObject.SetActive(true);
        crosshair_anim.AcquireAnimation(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemRealName + " 획득 " + "<color=Yellow>" + " (E)" + "</color>";
    }

    private void FurnitureInfoAppear()
    {
        ActionActivated = true;
        actionPanel.gameObject.SetActive(true);
        actionText.gameObject.SetActive(true);
        crosshair_anim.AcquireAnimation(true);
        actionText.text = "열기 / 닫기" + "<color=Yellow>" + " (E)" + "</color>";
    }

    private void SwitchInfoAppear()
    {
        ActionActivated = true;
        actionPanel.gameObject.SetActive(true);
        actionText.gameObject.SetActive(true);
        crosshair_anim.AcquireAnimation(true);
        actionText.text = "켜기 / 끄기" + "<color=Yellow>" + " (E)" + "</color>";
    }
    
    private void PushInfoAppear()
    {
        ActionActivated = true;
        actionPanel.gameObject.SetActive(true);
        actionText.gameObject.SetActive(true);
        crosshair_anim.AcquireAnimation(true);
        actionText.text = "밀기" + "<color=Yellow>" + " (E)" + "</color>";
    }

    private void InfoDisAppear()
    {
        ActionActivated = false;
        actionText.gameObject.SetActive(false);
        actionPanel.gameObject.SetActive(false);
        crosshair_anim.AcquireAnimation(false);
    }

    private void Action()
    {
        if(ActionActivated)
        {
            if(hitInfo.transform != null)
            {
                // 아이템
                if (hitInfo.transform.CompareTag("Item"))
                {
                    if(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Key")
                    {
                        if(theInventory.c_ETCSlots[0].isAcquired)
                        {
                            PlayerScript.instance.DesPrint("가진 열쇠부터 쓰고 다시 주워보자");
                            SoundManager.instance.PlaySoundEffect("Pick_Up_Item");
                            return;
                        }
                        else
                        {
                            theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                            Destroy(hitInfo.transform.gameObject);
                            InfoDisAppear();
                        }
                    }
                    else
                    {
                        theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                        Destroy(hitInfo.transform.gameObject);
                        InfoDisAppear();
                    }
                }
                // 서랍
                else if (hitInfo.transform.CompareTag("Draw"))
                {
                    if (hitInfo.transform.GetComponent<OpenManager>().hasKey)
                    {
                        if (theInventory.c_ETCSlots[0].isAcquired)
                        {
                            SoundManager.instance.PlaySoundEffect("Key_Open");
                            furAction.DrawAction(hitInfo.transform.gameObject);
                        }
                        else
                        {
                            PlayerScript.instance.DesPrint("서랍이 잠겨져있다.");
                            SoundManager.instance.PlaySoundEffect("Locked_Draw");
                        }
                    }
                    else if (hitInfo.transform.GetComponent<OpenManager>().isLocked)
                    {
                        PlayerScript.instance.DesPrint("서랍이 열리지 않는다.");
                        SoundManager.instance.PlaySoundEffect("Locked_Door");
                    }
                    else
                        furAction.DrawAction(hitInfo.transform.gameObject);
                }
                // 여닫이
                else if (hitInfo.transform.CompareTag("Door"))
                {
                    if (hitInfo.transform.GetComponent<OpenManager>().hasKey)
                    {
                        if (theInventory.c_ETCSlots[0].isAcquired)
                        {
                            SoundManager.instance.PlaySoundEffect("Key_Open");
                            furAction.DoorAction(hitInfo.transform.gameObject);
                        }
                        else
                        {
                            PlayerScript.instance.DesPrint("문이 잠겨져있다.");
                            SoundManager.instance.PlaySoundEffect("Locked_Door");
                        }
                    }
                    else if (hitInfo.transform.GetComponent<OpenManager>().isLocked)
                    {
                        PlayerScript.instance.DesPrint("문이 열리지 않는다.");
                        SoundManager.instance.PlaySoundEffect("Locked_Door");
                    }
                    else
                        furAction.DoorAction(hitInfo.transform.gameObject);
                }
                // 이동 가능한 문
                else if (hitInfo.transform.CompareTag("Path"))
                {
                    if (hitInfo.transform.GetComponent<OpenManager>().hasKey)
                    {
                        if (theInventory.c_NoteSlots[0].isAcquired &&
                            theInventory.c_NoteSlots[1].isAcquired &&
                            theInventory.c_NoteSlots[2].isAcquired &&
                            theInventory.c_ETCSlots[0].isAcquired)
                        {
                            SoundManager.instance.PlaySoundEffect("Key_Open");
                            furAction.PathAction(hitInfo.transform.gameObject);
                        }
                        else
                        {
                            PlayerScript.instance.DesPrint("문이 굳게 닫혀있다.");
                            SoundManager.instance.PlaySoundEffect("Locked_Door");
                        }
                    }
                    else
                        furAction.PathAction(hitInfo.transform.gameObject);
                }
                // 이동 가능한 문2
                else if (hitInfo.transform.CompareTag("Path1"))
                {
                    if (hitInfo.transform.GetComponent<OpenManager>().hasKey)
                    {
                        if (theInventory.c_NoteSlots[5].isAcquired &&
                            theInventory.c_NoteSlots[6].isAcquired &&
                            theInventory.c_NoteSlots[7].isAcquired &&
                            theInventory.c_NoteSlots[8].isAcquired &&
                            theInventory.c_NoteSlots[9].isAcquired &&
                            theInventory.c_ETCSlots[0].isAcquired)
                        {
                            SoundManager.instance.PlaySoundEffect("Key_Open");
                            furAction.Path1Action(hitInfo.transform.gameObject);
                        }
                        else
                        {
                            PlayerScript.instance.DesPrint("문이 굳게 닫혀있다.");
                            SoundManager.instance.PlaySoundEffect("Locked_Door");
                        }
                    }
                    else
                        furAction.PathAction(hitInfo.transform.gameObject);
                }
                // 전기 스위치
                else if (hitInfo.transform.CompareTag("TV"))
                {
                    furAction.TVAction(hitInfo.transform.gameObject);
                }
                else if (hitInfo.transform.CompareTag("Switch"))
                {
                    furAction.SwitchAction(hitInfo.transform.gameObject);
                }
                // 의자
                else if (hitInfo.transform.CompareTag("Chair"))
                {
                    if(!hitInfo.transform.GetComponent<OpenManager>().isMoving)
                        furAction.ChairAction(hitInfo.transform.gameObject);
                    else
                        PlayerScript.instance.DesPrint("움직일 필요가 없어보인다.");
                }
                else if (hitInfo.transform.CompareTag("Pillow"))
                {
                    if (!hitInfo.transform.GetComponent<OpenManager>().isMoving)
                        furAction.PillowAction(hitInfo.transform.gameObject);
                    else
                        PlayerScript.instance.DesPrint("움직일 필요가 없어보인다.");
                }
            }
        }
    }
}
