using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("슬롯 기본 정보")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private Text Text_ItemName;
    [SerializeField] private GameObject Gobj_Selected;
    [SerializeField] private DaniTechUIButton Button_SlotClick;
    

    private event Action<string> _onClickSlot;

    private string _slotDataId;

    private void OnEnable()
    {
        Button_SlotClick.BindOnClickButtonEvent(OnClick_InventorySlot);
    }

    private void OnClick_InventorySlot()
    {
        _onClickSlot?.Invoke(_slotDataId);
    }

    private void OnDisable()
    {
        _onClickSlot = null;
    }

    public void InitSlot(string dataId, Action<string> onClickCallback)
    {
        var itemData = DaniTechGameDataManager.Instance.GetDNItemData(dataId);
        _slotDataId = dataId;

        Text_ItemName.text = itemData.Name;
        string iconPath = itemData.IconPath;
        if (string.IsNullOrEmpty(iconPath) == true) return;

        DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();

        _slotDataId = dataId;

        _onClickSlot += onClickCallback;
        
    }

    public void SetSelectedUI(bool isSelect)
    {
        Gobj_Selected.SetActive(isSelect);
    }

}
