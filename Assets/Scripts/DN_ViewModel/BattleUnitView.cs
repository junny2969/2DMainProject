using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUnitView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("유닛 정보")]
    [SerializeField] private Image Image_Character;
    [SerializeField] private Text Text_Name;

    [Header("클릭 버튼")]
    [SerializeField] private DaniTechUIButton Button_Target;

    [Header("애니메이터")]
    [SerializeField] private BattleUnitAnimatorController AnimController_Unit;

    private BattleState _curState;
    
    private UnitModel _model;
    private Vector3 _originPosition;

    private void OnEnable()
    {
        if (Button_Target == null) return;
        Button_Target.BindOnClickButtonEvent(OnClick_Target);
    }
    public void InitBattleUnit(UnitModel model)
    {
        Text_Name.text = model.Data.Name;
        _model = model;
        _originPosition = transform.position;
    }

    private void OnClick_Target()
    {
        var curState = TurnManager.Inst.GetCurState();
        if(curState == BattleState.ChoiceTarget)
        {
            TurnManager.Inst.SaveTarget(_model);
        }
    }


    public async UniTask PlayAttackAction(Vector3 centerPosition)
    {
        if(AnimController_Unit == null)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            return;
        }

        AnimController_Unit.SetState(BattleUnitAnimState.Atk);

        bool isAtkState = false;
        while (isAtkState == false)
        {
            isAtkState = AnimController_Unit.IsCurrentState("Atk");
            await UniTask.Yield();
        }

        float atkLength = AnimController_Unit.GetCurrentStateLength();
        await UniTask.Delay(TimeSpan.FromSeconds(atkLength));
    }

    private async UniTask MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            await UniTask.Yield();
        }

        transform.position = targetPosition;
    }

    private async UniTask PlayAttackAnimation()
    {
        if(AnimController_Unit == null)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            return;
        }

        AnimController_Unit.SetState(BattleUnitAnimState.Atk);

        bool isAtkState = false;
        while(isAtkState == false)
        {
            if (AnimController_Unit == null) return;

            isAtkState = AnimController_Unit.IsCurrentState("Atk");
        }

        await UniTask.Yield();

        var atkLength = AnimController_Unit.GetCurrentStateLength();
        await UniTask.Delay(TimeSpan.FromSeconds(atkLength));
    }






    public void OnPointerEnter(PointerEventData eventData)
    {
        var curState = TurnManager.Inst.GetCurState();
        if (curState == BattleState.ChoiceTarget)
        {
            Image_Character.color = Color.yellow;

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image_Character.color = Color.white;

    }

}
