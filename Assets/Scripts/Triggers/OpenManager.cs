using UnityEngine;

public class OpenManager : MonoBehaviour
{
    public bool isOpen = false;     // 열고 닫고 가능한 오브젝트
    public bool hasKey = false;     // 열쇠가 있으면 열 수 있는 오브젝트.
    public bool isSwitchOn = true;  // 켜고 끌 수 있는 오브젝트.
    public bool isMoving = false;   // 움직일 수 있는 오브젝트.
    public bool isLocked = false;   // 아예 열리지 않는 오브젝트.
}
