using UnityEngine;

public enum EilieAnimState
{
    None = 0,
    Idle,
    Walk
    

}
public class Eilie_AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator Animator_Eilie;

    private EilieAnimState _currentAnimState;
    private Vector2 _lastDirection = Vector2.down;

    public void SetState(EilieAnimState newstate)
    {
        if (newstate == _currentAnimState) return;
        _currentAnimState = newstate;

        switch(_currentAnimState)
        {
            case EilieAnimState.Idle:
                ResetAllAnimParameter();
                break;
            case EilieAnimState.Walk:
                Animator_Eilie.SetBool("IsWalk", true);
                break;

            default:
                ResetAllAnimParameter();
                break;

        }
    }

    private void ResetAllAnimParameter()
    {
        Animator_Eilie.SetBool("IsWalk", false);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _lastDirection = direction;
        }
        Animator_Eilie.SetFloat("DirX", _lastDirection.x);
        Animator_Eilie.SetFloat("DirY", _lastDirection.y);
    }
  
}
