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

    private Dictionary<string, InventorySlotUI> _SlotList = new Dictionary<string, InventorySlotUI>();
    private void OnEnable()
    {
        ReadItemListAndCreateSlot();
        Button_CloseInventory.BindOnClickButtonEvent(OnClick_CloseInventory);
    }

    private void OnClick_CloseInventory()
    {
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.InventoryUI);
    }

    private void ReadItemListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.ItemDataList;
        foreach (var dataKv in dataList)
        {
            var data = dataKv.Value;
            if (data == null) return;

            CreateInventorySlot(data.Id);
        }
    }

    private void CreateInventorySlot(string dataId)
    {
        var gObj = Instantiate(Prefab_Slot, Transform_SlotRoot);
        if(gObj == null) return;

        var slotComponent = gObj.GetComponent<InventorySlotUI>();
        if (slotComponent == null) return;

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
