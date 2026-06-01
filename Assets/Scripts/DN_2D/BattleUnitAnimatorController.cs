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
        return Animator_Unit.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public float GetCurrentStateLength()
    {
        return Animator_Unit.GetCurrentAnimatorStateInfo(0).length;

    }

    private void ResetAllAnimParameters()
    {
        Animator_Unit.ResetTrigger("isAtk");
    }

}
