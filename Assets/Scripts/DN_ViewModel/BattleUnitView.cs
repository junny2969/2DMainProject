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
    [SerializeField] private Animator Animator_Unit;

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
        await MoveToPosition(centerPosition, 0.3f);

        await PlayAttackAnimation();

        await MoveToPosition(_originPosition, 0.3f);
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
        if(Animator_Unit == null)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            return;
        }

        Animator_Unit.SetTrigger("Attack");

        await UniTask.Yield();
        var stateInfo = Animator_Unit.GetCurrentAnimatorStateInfo(0);
        await UniTask.Delay(TimeSpan.FromSeconds(stateInfo.length));
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
