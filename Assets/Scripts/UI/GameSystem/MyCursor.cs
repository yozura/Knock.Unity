using System.Collections;
using UnityEngine;

public class MyCursor : MonoBehaviour
{
    public Texture2D cursorTexture;                     // 커서에 입힐 텍스쳐
    public Vector2 adjustHotSpot = Vector2.zero;        // 텍스쳐의 어느 부분을 마우스의 좌표로 할 것인지 텍스쳐의 좌표를 입력받습니다.
    private Vector2 hotSpot;                            // 화면에 실제로 표시할 좌표의 필드를 선언합니다.

    void Awake() => StartCoroutine("CustomCursor");    

    IEnumerator CustomCursor()
    {
        yield return new WaitForEndOfFrame();           // 모든 렌더링이 끝난 뒤 코루틴이 실행되도록 대기시킵니다.
        hotSpot = adjustHotSpot;                        // 수정된 마우스의 Vector값을 실제 필드에 대입합니다.
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto); // 마우스 커서의 텍스쳐, 위치를 옮깁니다.
    }

    // 커서 텍스쳐
    //<div>아이콘 제작자 <a href="https://www.flaticon.com/kr/authors/freepik" title="Freepik">Freepik</a>
    //from <a href="https://www.flaticon.com/kr/" title="Flaticon">www.flaticon.com</a></div>
}
