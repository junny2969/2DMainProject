using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameBookSlotUI : MonoBehaviour
{
    [Header("슬롯 기본 정보")]
    [SerializeField] private Image Image_MainIcon;
    [SerializeField] private Text Text_MainName;
    [SerializeField] private GameObject Gobj_Selected;
    [SerializeField] private DaniTechUIButton Button_SlotClick;

    private event Action<string> _onClickSlot;

    private string _slotDataId; // 슬롯이 살아있는동안 어떤 슬롯인이 DataId를 보관

    private void OnEnable()
    {
        Button_SlotClick.BindOnClickButtonEvent(OnClick_GameBookSlot);
    }

    
    
    public void OnClick_GameBookSlot()
    {
        // 자식이 눌러졌는데 부모에게 알림
        _onClickSlot?.Invoke(_slotDataId);
    }

    private void OnDisable()
    {
        _onClickSlot = null;

    }

    public void InitSlot(string dataId, Action<string> onClickCallback) // TODO : 카테고리에 따라 다른 데이터를 받아올 수 있도록 구별할 파라미터 추가 필요
    {
        var itemData = DaniTechGameDataManager.Instance.GetDNItemData(dataId);
        _slotDataId = dataId;

        Text_MainName.text = itemData.Name;
        string iconPath = itemData.IconPath;
        if (string.IsNullOrEmpty(iconPath) == true) return;

        DaniTechGameUtil.LoadAndSetSpriteImage(Image_MainIcon, iconPath).Forget();

        _slotDataId = dataId;

        _onClickSlot += onClickCallback;
        
    }

    public void SetSelectedUI(bool isSelect)
    {
        Gobj_Selected.SetActive(isSelect);
    }

}
