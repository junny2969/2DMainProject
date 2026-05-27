using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class HudUI : DaniTechUIBase
{
    [SerializeField] private GameObject Prefab_HudSlot;
    [SerializeField] private Transform Transform_SlotRoot;

     private Dictionary<int, HudSlotUI> _slotList = new Dictionary<int, HudSlotUI>();

    public void AddHudSlot(int instanceId, Transform targetTransform)
    {
        CreateHudSlot(instanceId, targetTransform);
    }

    private void CreateHudSlot(int instanceId, Transform targetTransform)
    {
        var gObj = Instantiate(Prefab_HudSlot, Transform_SlotRoot);
        if (gObj == null) return;

        var slotComponent = gObj.GetComponent<HudSlotUI>();
        if (slotComponent == null) return;

        //// 동적 생성된 자식슬롯(게임오브젝트)안에 있는 컴포넌트도 잘 가져왔다.
        slotComponent.InitSlot(instanceId, targetTransform);
        
        if (_slotList.ContainsKey(instanceId) == true )
        {
            Debug.LogWarning("이미 동일한 키가 있습니다.");
            return;
        }
        _slotList.Add(instanceId, slotComponent);
    }

    public void RemoveHudSlot() 
    {

    }
  
}
