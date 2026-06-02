using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUnitView : MonoBehaviour
{
    [Header("유닛 정보")]
    [SerializeField] private SpriteRenderer SpriteRenderer_Character;
    // [SerializeField] private TextMeshPro Text_Name;

    //[Header("클릭 버튼")]
    //[SerializeField] private DaniTechUIButton Button_Target;

    [Header("애니메이터")]
    [SerializeField] private BattleUnitAnimatorController AnimController_Unit;

    // private BattleState _curState;

    private UnitModel _model;
    private Vector3 _originPosition;

    private void OnEnable()
    {

    }
    public void InitBattleUnit(UnitModel model)
    {
        // Text_Name.text = model.Data.Name;
        _model = model;
        _originPosition = transform.position;
    }

    private void OnMouseDown()
    {
        var curState = TurnManager.Inst.GetCurState();
        if (curState == BattleState.ChoiceTarget)
        {
            TurnManager.Inst.SaveTarget(_model);
        }
    }
    private void OnMouseOver()
    {
        var curState = TurnManager.Inst.GetCurState();
        if (curState == BattleState.ChoiceTarget)
        {
            SpriteRenderer_Character.color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        SpriteRenderer_Character.color = Color.white;
    }

    public async UniTask PlayAttackAction(Vector3 centerPosition, string animTrigger)
    {
        await MoveToPosition(centerPosition, 0.3f);

        // 이동 완료 후 한 프레임 더 대기
        await UniTask.Yield();
        await UniTask.Yield();

        await PlayAttackAnimation(animTrigger);
        await MoveToPosition(_originPosition, 0.3f);
    }

    private async UniTask MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (this == null) return;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            await UniTask.Yield();
        }

        if (this == null) return;
        transform.position = targetPosition;
    }

    private async UniTask PlayAttackAnimation(string animTrigger)
    {
        if (AnimController_Unit == null)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            return;
        }

        AnimController_Unit.SetAtkState(animTrigger);

        bool isAtkState = false;
        int frameCount = 0;
        while (isAtkState == false)
        {
            if (AnimController_Unit == null) break;  // return 대신 break
            frameCount++;
            if (frameCount > 300)
            {
                // Debug.LogWarning("애니메이션 전환 타임아웃");
                break;  // return 대신 break
            }
            isAtkState = AnimController_Unit.IsCurrentState(animTrigger);
            await UniTask.Yield();
        }

        if (isAtkState == true)
        {
            float atkLength = AnimController_Unit.GetCurrentStateLength();
            await UniTask.Delay(TimeSpan.FromSeconds(atkLength));
        }
    }

    public async UniTask PlayHitAnimation()
    {
        if (AnimController_Unit == null) return;

        AnimController_Unit.SetHitState(true);

        bool isHitState = false;
        while (isHitState == false)
        {
            if (this == null) return;
            isHitState = AnimController_Unit.IsCurrentState("Hit");
            await UniTask.Yield();
        }

        float hitLength = AnimController_Unit.GetCurrentStateLength();
        await UniTask.Delay(TimeSpan.FromSeconds(hitLength));

        AnimController_Unit.SetHitState(false);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
