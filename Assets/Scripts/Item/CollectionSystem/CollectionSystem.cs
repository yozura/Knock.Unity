using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 추상 클래스를 선언해서 아이템들을 종류별로 보관합니다.
// 클릭 및 툴팁 생성을 위해 IPointer 인터페이스 구현
public abstract class CollectionSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 도감에 보관될 아이템
    public Item item_Collection;
    // 도감에 보관될 아이템의 이미지
    public Image item_Collection_Image;
    // 획득 미획득을 구분하는 진리값
    public bool isAcquired = false;

    // protected 접근제한자로 상속받은 자식 클래스만 사용할 수 있도록 구현.
    // 필요한 클래스 컴포넌트들, 툴팁을 표시하기위해 툴팁 클래스를, 아이템의 세부정보를 입력할 수 있도록 인포메이션 클래스를 가져옴.
    protected SlotToolTip theSlotToolTip;
    protected Collection_Information c_Info;

    protected void Start()
    {
        theSlotToolTip = FindObjectOfType<SlotToolTip>();
        c_Info = FindObjectOfType<Collection_Information>();
    }

    // 추상 메서드를 생성해서 자식 클래스에서 입력하도록 구현.
    // 획득 미획득을 시각적으로 표현하기 위한 색구분 함수
    public abstract void SetColor(float _rgb);

    // 아이템을 획득하게 되면 어디로 저장할 것인지, 어떤 변수를 건드려 바꿀 것인지 체크.
    public abstract void AddCollection(Item _item);

    // 도감 아이템 클릭시 실행되는 인터페이스
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 왼쪽 클릭으로 UI를 클릭했을 때 실행
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 아이템 정보창 호출.
            c_Info.c_InformationBox.SetActive(true);
            // 도감에 보이는 아이템이 실루엣이 아닌 컬러일 때에만
            if (item_Collection_Image.color == new Color(1f,1f,1f))
                // 안에 있는 정보를 동적으로 바꿔줍니다.
                c_Info.Edit_Information(item_Collection);
            else
                // 실루엣일 경우에는 디폴트값을 출력해줍니다.
                c_Info.Default_Information();
        }
    }

    // 도감 아이템에 마우스를 갖다댔을 때 실행되는 인터페이스
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템을 획득했다면
        if (isAcquired)
            // 아이템의 이름을 출력해주고
            ShowToolTip(item_Collection, transform.position, true);
        else
            // 미획득 상태라면 디폴트 값을 넘겨줍니다.
            ShowToolTipDefault(transform.position);
    }

    // 도감 아이템에서 마우스를 뗐을 때 실행되는 인터페이스
    // 한 줄짜리 메서드는 람다식을 이용해 코드를 간결화함.
    public void OnPointerExit(PointerEventData eventData) => HideToolTip();

    protected void ShowToolTip(Item _item, Vector3 _pos, bool isCollection) => theSlotToolTip.ShowToolTip(_item, _pos, isCollection);

    protected void HideToolTip() => theSlotToolTip.HideToolTip();

    protected void ShowToolTipDefault(Vector3 _pos) => theSlotToolTip.ShowToolTipDefault(_pos);
}
