using UnityEngine;

public enum EntityAnimState
{
    None = 0,
    Idle,
    Walk,
    Jump,
    Atk,
    Hit
}
public class HJ_2DAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator Animator_Entity;

    private EntityAnimState _currentAnimState;

    public void SetState(EntityAnimState newstate)
    {
        if(newstate == EntityAnimState.Idle && _currentAnimState == EntityAnimState.Idle)
        {
            return;
        }

        _currentAnimState = newstate;

        switch (_currentAnimState)
        {
            case EntityAnimState.Idle:
                ResetAllAnimParameter();
                break;

            case EntityAnimState.Walk:
                Animator_Entity.SetBool("IsWalk", true);
                break;

            case EntityAnimState.Jump:
                // Debug.LogWarning("Jump SetTrigger 호출됨");
                Animator_Entity.SetTrigger("IsJump");
                break;

            case EntityAnimState.Atk:
                Animator_Entity.SetTrigger("IsAtk");
                break;
                
            case EntityAnimState.Hit:
                Animator_Entity.SetTrigger("IsHit");
                break;

            default:
                ResetAllAnimParameter();
                break;
        }
    }

    private void ResetAllAnimParameter()
    {
        Animator_Entity.SetBool("IsWalk", false);
    }
}
