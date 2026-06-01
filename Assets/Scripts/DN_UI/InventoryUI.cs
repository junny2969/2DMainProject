using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EInventoryCategory
{
    None = 0,
    SkillCategory,
    PotionCategory,
    EqupmentCategory,
    WeaponCategory,
    QuestItemCategory
}
public class InventoryUI : DaniTechUIBase
{
    [Header("동적 생성할 프리팹")]
    [SerializeField] private GameObject Prefab_Slot;

    [Header("버튼리스트")]
    [SerializeField] DaniTechUIButton Button_CloseInventory;
    [SerializeField] DaniTechUIButton Button_OpenSkill;
    [SerializeField] DaniTechUIButton Button_OpenPotion;
    [SerializeField] DaniTechUIButton Button_OpenEqupment;
    [SerializeField] DaniTechUIButton Button_Weapon;
    [SerializeField] DaniTechUIButton Button_UseItem;



    [Header("슬롯 리스트 영역")]
    [SerializeField] private Transform Transform_SlotRoot;

    [Header("아이템 설명")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private Text Text_ItemName;
    [SerializeField] private Text Text_Description;

    // [Header("부가정보")] >> 추후 레이아웃을 껏다켰다 할때 사용
    // [SerializeField] private GameObject Layout_SubInfoSkill;
    private Dictionary<string, InventorySlotUI> _slotList = new Dictionary<string, InventorySlotUI>();


    private void OnEnable()

    {
        OnClick_OpenPotion(); // 이 UI가 열릴때 스스로 기본적으로 아이템 도감안에 있는 모든 데이터를 불러온다.

        Button_CloseInventory.BindOnClickButtonEvent(OnClick_CloseInventory);
        Button_OpenSkill.BindOnClickButtonEvent(OnClick_OpenSkill);
        Button_OpenPotion.BindOnClickButtonEvent(OnClick_OpenPotion);
        Button_OpenEqupment.BindOnClickButtonEvent(OnClick_OpenEqupment);
        Button_Weapon.BindOnClickButtonEvent(OnClick_Weapon);

        // Button_UseItem.gameObject.SetActive(false);
    }

    private void OnClick_CloseInventory()
    {
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.InventoryUI);
    }

    private void OnClick_UseItem()
    {

    }

    private void OnDisable()
    {
        //ClearSlotList();

        //if (_slotList.Count > 0)
        //{
        //    foreach (var slotKv in _SlotList)
        //    {
        //        var slot = slotKv.Value;
        //        DestroyImmediate(slot.gameObject);
        //    }
        //    _SlotList.Clear();
        //}
    }

    private void OnClick_OpenSkill()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.SkillCategory);
    }

    private void OnClick_OpenPotion()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.PotionCategory);

    }

    private void OnClick_OpenEqupment()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.EqupmentCategory);

    }

    private void OnClick_Weapon()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.WeaponCategory);

    }
    //private void OnClick_UseItem()
    //{
        

    //}

    private void RequestSelectUseItm()
    {

    }

    private void RemoveItemSlot()
    {
        // 저장 정보에서 먼저 아이템이 제거된 후에 슬롯이 제거되어야 한다
    }

    private void SetInventoryLayoutByCategory(EInventoryCategory category)
    {
        DestroyAndClearSlotList();
        switch (category)
        {
            case EInventoryCategory.SkillCategory:
                ReadSkillListAndCreateSlot();
                break;
            case EInventoryCategory.PotionCategory:
                ReadPotionListAndCreateSlot();
                break;
            case EInventoryCategory.EqupmentCategory:
                ReadEqupmentListAndCreateSlot();
                break;
            case EInventoryCategory.WeaponCategory:
                ReadWeaponListAndCreateSlot();
                break;
            default:
                break;
        }
    }

    private void DestroyAndClearSlotList()
    {
        if(_slotList.Count > 0)
        {
            foreach(var slotKv in _slotList)
            {
                var slot = slotKv.Value;
                DestroyImmediate(slot.gameObject);
            }

            _slotList.Clear();
        }
    }
    private void ReadSkillListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.SkillDataList;
        foreach (var dataKv in dataList)
        {
            DNSkillData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.SkillCategory);
        }
        SelectFirstSlot();
    }

    private void ReadPotionListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.PotionDataList;
        foreach (var dataKv in dataList)
        {
            PotionData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.PotionCategory);
        }

        SelectFirstSlot();
    }

    private void ReadEqupmentListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.EqupmentDataList;
        foreach (var dataKv in dataList)
        {
            EqupmentData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.EqupmentCategory);
        }

        SelectFirstSlot();
    }

    private void ReadWeaponListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.WeaponDataList;
        foreach (var dataKv in dataList)
        {
            DNWeaponData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.WeaponCategory);
        }

        SelectFirstSlot();
    }

    private void SelectFirstSlot()
    {
        if(_slotList.Count > 0)
        {
            foreach (var slotKv in _slotList)
            {
                var slot = slotKv.Value;
                slot.OnClick_InventorySlot();
            }
        }
    }


    private void CreateInventorySlot(string dataId, EInventoryCategory curCategory)
    {
        var gObj = Instantiate(Prefab_Slot, Transform_SlotRoot);
        if (gObj == null) return;

        var slotComponent = gObj.GetComponent<InventorySlotUI>();
        if (slotComponent == null) return;

        // 동적 생성된 자식슬롯(게임오브젝트)안에 있는 컴포넌트도 잘 가져왔다.
        slotComponent.InitSlot(dataId, curCategory, OnClickChildSlotSelected);
        _slotList.Add(dataId, slotComponent);
    }

    private void SetDetailInforUI(string dataName, string dataDescription = "", string iconPath = "")
    {
       
        Text_ItemName.text = dataName;
        Text_Description.text = dataDescription;

        if (string.IsNullOrEmpty(iconPath) == false)
        {
            DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();
        }

        Image_ItemIcon.gameObject.SetActive(string.IsNullOrEmpty(iconPath) == false);

    }
    private void OnClickChildSlotSelected(string slotDataId, EInventoryCategory selectedCatogory)
    {
       

        if (selectedCatogory == EInventoryCategory.SkillCategory)
        {


            var currentSelectedData = DaniTechGameDataManager.Instance.GetSkill(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);
        }

        else if(selectedCatogory == EInventoryCategory.PotionCategory)
        {
            var currentSelectedData = DaniTechGameDataManager.Instance.GetPotionData(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);

        }

        else if(selectedCatogory == EInventoryCategory.EqupmentCategory)
        {
            var currentSelectedData = DaniTechGameDataManager.Instance.GetEqupmentData(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);
        }

        else if(selectedCatogory == EInventoryCategory.WeaponCategory)
        {
            var currentSelectedData = DaniTechGameDataManager.Instance.GetWeaponData(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);
        }

        foreach (var slotKv in _slotList)
        {
            var slot = slotKv.Value;
            var dataId = slot.GetSlotDataId();
            slot.SetSelectedUI(slotDataId == dataId);

            

        }

    }
}