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

    private event Action<string, EInventoryCategory> _onClickSlot;
    private string _slotDataId;
    private EInventoryCategory _curSlotCategory;

    public string GetSlotDataId()
    {
        return _slotDataId;
    }

    private void OnEnable()
    {
        Button_SlotClick.BindOnClickButtonEvent(OnClick_InventorySlot);
    }

    
    public void OnClick_InventorySlot()
    {
        // 자식이 눌러졌는데 부모에게 알림
        _onClickSlot?.Invoke(_slotDataId, _curSlotCategory);
    }
    private void OnDisable()
    {
        _onClickSlot = null;
    }

    private void SetSlotUI(string dataName, string iconPath)
    {
        Text_ItemName.text = dataName;
        
        if (string.IsNullOrEmpty(iconPath) == false)
        {
            DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();
        }
    }

    public void InitSlot(string dataId, EInventoryCategory curCategory, Action<string, EInventoryCategory> onClickCallback)
    {
        if(curCategory == EInventoryCategory.SkillCategory)
        {
            var skillData = DaniTechGameDataManager.Instance.GetSkill(dataId);
            if (skillData == null) return;

            SetSlotUI(skillData.Name, skillData.IconPath);

        }

        else if (curCategory == EInventoryCategory.PotionCategory)
        {
            var potionData = DaniTechGameDataManager.Instance.GetPotionData(dataId);
            if (potionData == null) return;

            SetSlotUI(potionData.Name, potionData.IconPath);

        }

        else if (curCategory == EInventoryCategory.EqupmentCategory)
        {
            var equpmentData = DaniTechGameDataManager.Instance.GetEqupmentData(dataId);
            if (equpmentData == null) return;

            SetSlotUI(equpmentData.Name, equpmentData.IconPath);

        }

        else if (curCategory == EInventoryCategory.WeaponCategory)
        {
            var weaponData = DaniTechGameDataManager.Instance.GetWeaponData(dataId);
            if (weaponData == null) return;

            SetSlotUI(weaponData.Name, weaponData.IconPath);

        }

        // Image에 아이콘, Sprite 리소스 불러올때 일단 암기하고 사용하기
        _slotDataId = dataId;
        _curSlotCategory = curCategory;
        _onClickSlot += onClickCallback;

    }
    public void SetSelectedUI(bool isSelect)
    {
        GObj_Selected.SetActive(isSelect);
    }
}