using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class HudSlotUI : MonoBehaviour
{
    [SerializeField] private int SlotOffsetY;
    [SerializeField] private GameObject Layount_TextArea;

    [SerializeField] private Text Text_Name;

    [SerializeField] private Slider Slider_Hp;
    [SerializeField] private Slider Slider_Mp;

    private int _instanceId;

    // 참조형을  기록(캐싱)
    private Transform _targetTransform;

    public void InitSlot(int instanceId, Transform tragetTransform)
    {
        _instanceId = instanceId;
        _targetTransform = tragetTransform;
        SlotOffsetY = 120;

        TryBindStatChangedEvent(tragetTransform.gameObject);
    }

    private void TryBindStatChangedEvent(GameObject gObj)
    {
        // gObj가 몬스터거나 플레이어라면 GetComponent를 시도해보고 잘 되면 거기 안에 있는 이벤트를 구독하자
        var player = gObj.GetComponent<DaniTech_2DPlayer>(); // 
        if (player != null) 
        {

            // player.BindOnStatChangedEvent(OnTargetEntityHpChanged, OnTargetEntityHpChanged);
            return;
        }

        var monster = gObj.GetComponent<GameMonster>();
        if (monster != null)
        {

            return;
        }
    }

    private void OnTargetEntityHpChanged(int curHp, int maxHp)
    {
        Slider_Hp.value = (curHp / (float)maxHp);
    }

    private void OnTargetEntityMpChanged(int curMp, int maxMp)
    {
        Slider_Mp.value = (curMp / (float)maxMp);

    }

    public void Update()
    {
        // 참조형을 캐싱할때는 꼭 널체크를 사용부에서 체크하기
        if(_targetTransform != null)
        {
            // this.gameObject.transform.position = _targetTransform.position;

            // World > 스크린 좌표
            Vector2 screenPos = Camera.main.WorldToScreenPoint(_targetTransform.position);


            // UGUI에서 사용하기 위해
            var rectTransform = this.GetComponent<RectTransform>();
            if (rectTransform != null) 
            {
                Vector2 finalScreenPos = new Vector2(screenPos.x, screenPos.y - SlotOffsetY);
                rectTransform.anchoredPosition = finalScreenPos;
            }
        }
    }
}
