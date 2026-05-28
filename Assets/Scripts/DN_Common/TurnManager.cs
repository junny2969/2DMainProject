using UnityEngine;

public enum BattleState
{
    None = 0,
    PlayerTurn,
    ChoiceAction,
    ChoiceTarget,
    PlayerAction,
    MonsterTurn

}
public class TurnManager : MonoBehaviour
{
    private BattleState curBattleState;
    public static TurnManager Inst {  get; private set; }
    private void Awake()
    {
        Inst = this;
    }

    public void StartBattle()
    {
        curBattleState = BattleState.PlayerTurn;
    }
}

