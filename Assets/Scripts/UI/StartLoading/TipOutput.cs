using UnityEngine;
using UnityEngine.UI;

public class TipOutput : MonoBehaviour
{
    private Text tip;

    void Start()
    {
        int _random = Random.Range(0, 5);
        tip = GetComponent<Text>();

        if (_random == 0)
            tip.text = "Tip : Tab을 누르시면 획득한 아이템의 정보를 확인할 수 있습니다.";
        else if (_random == 1)
            tip.text = "Tip : Esc를 누르시면 옵션 변경 및 조작법을 확인하실 수 있습니다.";
        else if (_random == 2)
            tip.text = "Tip : 퀵슬롯 아이템은 4, 5번을 누르시면 슬롯 순서대로 사용하실 수 있습니다.";
        else if (_random == 3)
            tip.text = "Tip : 옵션에서 사운드 및 해상도 조절을 하실 수 있습니다.";
        else
            tip.text = "Tip : 단서와 일지를 모아 정보를 얻고 방에서 탈출하십시오.";
    }
}
