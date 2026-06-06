using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillPopupCategory
{
    None = 0,
    NormalSkill,
    SpeacialSkill,
    UseItem
}
public class BattleActionPopup : DaniTechUIBase
{
    [Header("동적 생성할 프리팹")]
    [SerializeField] private GameObject Prefab_Slot;

    [Header("상단 버튼")]
    [SerializeField] private DaniTechUIButton Btn_NormalSkill;
    [SerializeField] private DaniTechUIButton Btn_SpeacialSkill;
    [SerializeField] private DaniTechUIButton Btn_UseItem;
    [SerializeField] private Image Arrow_NormalSkill;
    [SerializeField] private Image Arrow_SpecialSkill;
    [SerializeField] private Image Arrow_Item;
    [SerializeField] private Text Text_CurCategory;


    [Header("슬롯 루트")]
    [SerializeField] private Transform Root_SlotList;

    //[Header("하단 버튼")]
    //[SerializeField] private Transform Root_SkillList;
    // [SerializeField] private DaniTechUIButton Btn_SkillSlot;

    private UnitModel _unitModel;
    private SkillPopupCategory _curCategory = SkillPopupCategory.None;

    private int _generatedKey = 0;
    private Dictionary<int, DaniTechUIButton> _slotList = new Dictionary<int, DaniTechUIButton>();

    private void OnEnable()
    {
        
        Btn_NormalSkill.BindOnClickButtonEvent(OnClick_NormalSkill);
        Btn_SpeacialSkill.BindOnClickButtonEvent(OnClick_SpeacialSkill);
        Btn_UseItem.BindOnClickButtonEvent(OnClick_UseItem);

        
    }
    public void RefreshSkillList()
    {
        var playerInfo = BattleManager.Inst.GetPlayerModel();
        // Debug.LogWarning("playerInfo: " + (playerInfo == null ? "null" : playerInfo.Data.Name));
        _unitModel = playerInfo;
        if (_unitModel == null) return;
        _curCategory = SkillPopupCategory.None;
        OnClick_NormalSkill();
    }

    private void OnClick_NormalSkill()
    {
        if (_curCategory == SkillPopupCategory.NormalSkill) return;
        if(Arrow_NormalSkill == null || Arrow_SpecialSkill == null || Arrow_Item == null) return;

        Arrow_NormalSkill.gameObject.SetActive(true);
        Arrow_SpecialSkill.gameObject.SetActive(false);
        Arrow_Item.gameObject.SetActive(false);

        Text_CurCategory.text = "일반기술";

        _curCategory = SkillPopupCategory.NormalSkill;

        ClearSlotList();
        RefreshNormalSkillAsync().Forget();
    }

    private void OnClick_SpeacialSkill()
    {
        if (_curCategory == SkillPopupCategory.SpeacialSkill) return;
        if (Arrow_NormalSkill == null || Arrow_SpecialSkill == null || Arrow_Item == null) return;

        Arrow_NormalSkill.gameObject.SetActive(false);
        Arrow_SpecialSkill.gameObject.SetActive(true);
        Arrow_Item.gameObject.SetActive(false);

        Text_CurCategory.text = "특수기술";



        _curCategory = SkillPopupCategory.SpeacialSkill;

        ClearSlotList();
        RefreshSpecialSkillAsync().Forget();
    }
    private void OnClick_UseItem()
    {
        Text_CurCategory.text = "아이템 사용";

        Debug.LogWarning("아이템 탭 클릭됨");
        if (Arrow_NormalSkill == null || Arrow_SpecialSkill == null || Arrow_Item == null) return;

        Arrow_NormalSkill.gameObject.SetActive(false);
        Arrow_SpecialSkill.gameObject.SetActive(false);
        Arrow_Item.gameObject.SetActive(true);

        if (_curCategory == SkillPopupCategory.UseItem) return;
        _curCategory = SkillPopupCategory.UseItem;

        ClearSlotList();
        RefreshItemAsync().Forget();
    }

    private void ClearSlotList()
    {
        foreach (var slot in _slotList)
        {
            Destroy(slot.Value.gameObject);
        }
        _slotList.Clear();
        _generatedKey = 0;
    }

    private async UniTaskVoid RefreshNormalSkillAsync()
    {
        var skillIdList = DaniTechGameManager.Inst.GetPlayerSkillListByType("Normal");
        foreach(string skillId in skillIdList)
        {
            await CreateSkillSlot(skillId);
        }
    }
    private async UniTaskVoid RefreshSpecialSkillAsync()
    {
        var skillIdList = DaniTechGameManager.Inst.GetPlayerSkillListByType("Special");
        // Debug.LogWarning("Special 스킬 개수 : " + skillIdList);

        foreach (string skillId in skillIdList)
        {
            var skillData = DaniTechGameDataManager.Instance.GetSkill(skillId);
            // Debug.LogWarning(skillId + "skillType::" + (skillData == null ? "null" : skillData.SkillType));
            await CreateSkillSlot(skillId);
        }
    }

    private async UniTaskVoid RefreshItemAsync()
    {
        var itemList = DaniTechGameManager.Inst.GetPlayerItemList();
        Debug.LogWarning("보유 아이템 개수 : " + itemList.Count);

        foreach (var itemModel in itemList)
        {
            Debug.LogWarning("아이템 슬롯 생성 시도 :" + itemModel);

            await CreateItemSlot(itemModel);
        }
    }

    private async UniTask CreateSkillSlot(string skillId)
    {
        var skillData = DaniTechGameDataManager.Instance.GetSkill(skillId);
        if (skillData == null) return;

        var gObj = Instantiate(Prefab_Slot, Root_SlotList);
        if (gObj == null) return;

        var slotButton = gObj.GetComponent<DaniTechUIButton>();
        if(slotButton == null) return;

        slotButton.ChangeButtonText(skillData.Name);
        await slotButton.ChangeButtonImage(skillData.IconPath);

        string capturedSkillId = skillId;
        void OnClickSkill()
        {
            TurnManager.Inst.OnClick_SkillSlot(capturedSkillId);
        }
        slotButton.BindOnClickButtonEvent(OnClickSkill);

        _generatedKey++;
        _slotList.Add(_generatedKey, slotButton);
    }

    private async UniTask CreateItemSlot(DaniTechItemModel itemModel)
    {
        var itemData = DaniTechGameDataManager.Instance.GetDNItemData(itemModel.ItemDataId);
        if (itemData == null) return;

        var gObj = Instantiate(Prefab_Slot, Root_SlotList);
        if (gObj == null) return;

        var slotButton = gObj.GetComponent<DaniTechUIButton>();
        if (slotButton == null) return;

        slotButton.ChangeButtonText(itemData.Name);
        await slotButton.ChangeButtonImage(itemData.IconPath);

        string capturedItemId = itemModel.ItemDataId;

        void OnClickItem()
        {
            Debug.Log($"아이템 사용 : {capturedItemId}");
        }

        slotButton.BindOnClickButtonEvent(OnClickItem);

        _generatedKey++;
        _slotList.Add(_generatedKey,slotButton);
    }
    
}
