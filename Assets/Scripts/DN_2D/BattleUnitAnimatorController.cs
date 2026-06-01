using UnityEngine;

public enum BattleUnitAnimState
{
    None = 0,
    Idle,
    Atk
}
public class BattleUnitAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator Animator_Unit;
    private BattleUnitAnimState _cureentAnimState;

    public void SetState(BattleUnitAnimState newState)
    {
        if (newState == _cureentAnimState) return;

        _cureentAnimState = newState;
        switch (_cureentAnimState)
        {
            case BattleUnitAnimState.Idle:
                ResetAllAnimParameters();
                break;
                case BattleUnitAnimState.Atk:
                Animator_Unit.SetTrigger("isAtk");
                break;
            default:
                ResetAllAnimParameters();
                break;
        }
    }

    public bool IsCurrentState(string stateName)
    {
        bool result = Animator_Unit.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        Debug.LogWarning("IsName 체크 - 찾는 이름: " + stateName + "/ 결과 :" + result);
        return result;
    }

    public float GetCurrentStateLength()
    {
        return Animator_Unit.GetCurrentAnimatorStateInfo(0).length;

    }

    private void ResetAllAnimParameters()
    {
        Animator_Unit.ResetTrigger("isAtk");
    }

    public void SetAtkState(string animTrigger)
    {
        if (string.IsNullOrEmpty(animTrigger) == true) return;
        _cureentAnimState = BattleUnitAnimState.None;
        _cureentAnimState = BattleUnitAnimState.Atk;
        Animator_Unit.SetTrigger(animTrigger);
        Debug.LogWarning("트리거 발동 : " + animTrigger);
    }

    public string GetCurrentStateName()
    {
        return Animator_Unit.GetCurrentAnimatorStateInfo(0).IsName("IsAtk_Swing").ToString();
    }

    public void SetIdleState()
    {
        _cureentAnimState = BattleUnitAnimState.None;
        SetState(BattleUnitAnimState.Idle);
    }
}
