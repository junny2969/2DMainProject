using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlotUI : MonoBehaviour
{
    [Header("슬롯 기본 정보")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private Text Text_ItemName;
    [SerializeField] private GameObject GObj_Selected;
    [SerializeField] private DaniTechUIButton Button_SlotClick;

    private event Action<string> _onClickSlot;
    private string _slotDataId;
    private void OnEnable()
    {
        Button_SlotClick.BindOnClickButtonEvent(OnClick_InventorySlot);
    }
    private void OnClick_InventorySlot()
    {
        // 자식이 눌러졌는데 부모에게 알림
        _onClickSlot?.Invoke(_slotDataId);
    }
    private void OnDisable()
    {
        _onClickSlot = null;
    }
    public void InitSlot(string dataId, Action<string> onClickCallback)
    {
        // dataId를 받아 엑셀데이터의 한줄 받아오는 부분
        var itemData = DaniTechGameDataManager.Instance.GetDNItemData(dataId);
        _slotDataId = dataId;
        Text_ItemName.text = itemData.Name;
        string iconPath = itemData.IconPath;
        if (string.IsNullOrEmpty(iconPath) == true) return;
        // Image에 아이콘, Sprite 리소스 불러올때 일단 암기하고 사용하기
        DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();
        _slotDataId = dataId;
        _onClickSlot += onClickCallback;

    }
    public void SetSelectedUI(bool isSelect)
    {
        GObj_Selected.SetActive(isSelect);
    }
}