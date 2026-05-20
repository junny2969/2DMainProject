using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : DaniTechUIBase
{
    [Header("동적 생성할 프리팹")]
    [SerializeField] private GameObject Prefab_Slot;
    [Header("버튼리스트")]
    [SerializeField] DaniTechUIButton Button_CloseInventory;
    [SerializeField] DaniTechUIButton Button_OpenSkill;
    [SerializeField] DaniTechUIButton Button_OpenPotion;
    [SerializeField] DaniTechUIButton Button_OpenEqupment;
    [SerializeField] DaniTechUIButton Button_QuestItem;
    [Header("슬롯 리스트 영역")]
    [SerializeField] private Transform Transform_SlotRoot;
    [Header("아이템 설명")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private Text Text_ItemName;
    [SerializeField] private Text Text_Description;
    // [Header("부가정보")] >> 추후 레이아웃을 껏다켰다 할때 사용
    // [SerializeField] private GameObject Layout_SubInfoSkill;
    private Dictionary<string, InventorySlotUI> _SlotList = new Dictionary<string, InventorySlotUI>();
    private void OnEnable()
    {
        ReadItemListAndCreateSlot(); // 이 UI가 열릴때 스스로 기본적으로 아이템 도감안에 있는 모든 데이터를 불러온다.
        Button_CloseInventory.BindOnClickButtonEvent(OnClick_CloseInventory);
    }
    private void OnClick_CloseInventory()
    {
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.InventoryUI);
    }
    private void OnDisable()
    {
        if (_SlotList.Count > 0)
        {
            foreach (var slotKv in _SlotList)
            {
                var slot = slotKv.Value;
                DestroyImmediate(slot.gameObject);
            }
            _SlotList.Clear();
        }
    }
    private void ReadItemListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.ItemDataList;
        foreach (var dataKv in dataList)
        {
            DNItemData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id);
        }
    }
    private void CreateInventorySlot(string dataId)
    {
        var gObj = Instantiate(Prefab_Slot, Transform_SlotRoot);
        if (gObj == null) return;

        var slotComponent = gObj.GetComponent<InventorySlotUI>();
        if (slotComponent == null) return;
        // 동적 생성된 자식슬롯(게임오브젝트)안에 있는 컴포넌트도 잘 가져왔다.
        slotComponent.InitSlot(dataId, OnClickChildSlotSelected);
        _SlotList.Add(dataId, slotComponent);
    }
    private void OnClickChildSlotSelected(string slotDataId)
    {
        var currentSelectedData = DaniTechGameDataManager.Instance.GetDNItemData(slotDataId);
        if (currentSelectedData == null) return;
        Text_ItemName.text = currentSelectedData.Name;
        Text_Description.text = currentSelectedData.Description;
        DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, currentSelectedData.IconPath).Forget();

    }
}