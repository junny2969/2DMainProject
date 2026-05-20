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
    public void InitSlot(string dataId, EInventoryCategory curCategory, Action<string> onClickCallback)
    {
        if(curCategory == EInventoryCategory.SkillCategory)
        {
            var skillData = DaniTechGameDataManager.Instance.GetDNItemData(dataId);
            if (skillData == null) return;

            Text_ItemName.text = skillData.Name;
            string iconPath = skillData.IconPath;
            if (string.IsNullOrEmpty(iconPath) == true) return;

            DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();

        }

        else if (curCategory == EInventoryCategory.PotionCategory)
        {
            var potionData = DaniTechGameDataManager.Instance.GetPotionData(dataId);
            if (potionData == null) return;

            Text_ItemName.text = potionData.Name;
            string iconPath = potionData.IconPath;
            if (string.IsNullOrEmpty(iconPath) == true) return;

            DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();
        }

        else if (curCategory == EInventoryCategory.EqupmentCategory)
        {
            var epupmentData = DaniTechGameDataManager.Instance.GetEqupmentData(dataId);
            if (epupmentData == null) return;

            Text_ItemName.text = epupmentData.Name;
            string iconPath = epupmentData.IconPath;
            if (string.IsNullOrEmpty(iconPath) == true) return;

            DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();
        }

        else if (curCategory == EInventoryCategory.WeaponCategory)
        {
            var weaponData = DaniTechGameDataManager.Instance.GetWeaponData(dataId);
            if (weaponData == null) return;

            Text_ItemName.text = weaponData.Name;
            string iconPath = weaponData.IconPath;
            if (string.IsNullOrEmpty(iconPath) == true) return;

            DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();
        }


        // Image에 아이콘, Sprite 리소스 불러올때 일단 암기하고 사용하기
        _slotDataId = dataId;
        _onClickSlot += onClickCallback;

    }
    public void SetSelectedUI(bool isSelect)
    {
        GObj_Selected.SetActive(isSelect);
    }
}