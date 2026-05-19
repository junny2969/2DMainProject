using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;


public class GameBookUI : DaniTechUIBase
{
    [Header("동적 생성할 프리팹")]

    [SerializeField] private GameObject Prefab_Slot;

    [Header("디테일 정보 영역")]
    [SerializeField] private Image Image_MainIcon;
    [SerializeField] private Text Text_MainName;
    [SerializeField] private Text Text_Description;

    //[Header("부가 정보")]
    //[SerializeField] private GameObject Layout_SubInfoSkill;

    [Header("슬롯 리스트 영역")]
    [SerializeField] private Transform Transform_SlotRoot;

    private Dictionary<string, GameBookSlotUI> _slotList = new Dictionary<string, GameBookSlotUI>();

    private void OnEnable()
    {
        // 이 UI가 열릴때 스스로 기본적으로 아이템 도감안에 있는 모든 데이터를 불러온다

        ReadItemListAndCreateSlot();
    }

    private void OnDisable()
    {
        if (_slotList.Count > 0)
        {
            foreach (var slotKv in _slotList)
            {
                var slot = slotKv.Value;
            }
        }
    }
    private void ReadItemListAndCreateSlot()
    {
        // 데이터를 읽어와서 순회(foreac)를 돌면서, 아이템들을 도감 리스트에 표기
        var dataList = DaniTechGameDataManager.Instance.ItemDataList;
        foreach (var dataKv in dataList) // 
        {
            var data = dataKv.Value;
            if (data == null) continue;

            CreateGameBookSlot(data.Id);
            
        }
    }


    private void CreateGameBookSlot(string dataId)
    {
        var gObj = Instantiate(Prefab_Slot, Transform_SlotRoot);
        if (gObj == null) return;

        var slotComponent = gObj.GetComponent<GameBookSlotUI>();
        if (slotComponent == null) return;

        slotComponent.InitSlot(dataId, OnClickChildSlotSelected);
        _slotList.Add(dataId, slotComponent);
    }

    private void OnClickChildSlotSelected(string slotDataId)
    {
        var currentSelectedData = DaniTechGameDataManager.Instance.GetDNItemData(slotDataId);
        if(currentSelectedData == null) return;

        // Image_MainIcon
        Text_MainName.text = currentSelectedData.Name;
        Text_Description.text = currentSelectedData.Description;

        DaniTechGameUtil.LoadAndSetSpriteImage(Image_MainIcon, currentSelectedData.IconPath).Forget();

        foreach (var slotKv in _slotList)
        {
            var slot = slotKv.Value;
            // var dataId = slot.GetSlotDataId();
            // slot.SetSelectedUI(slotDataId == dataId);
        }
    }
}
