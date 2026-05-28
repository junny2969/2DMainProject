using System;
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

    public event Action<BattleState> OnStateChanged;

    private string _selectedSkillId;

    public static TurnManager Inst {  get; private set; }

    private void Awake()
    {
        Inst = this;
    }

    public void StartBattle()
    {
        ChangeBattleState(BattleState.PlayerTurn);

    }

    public void ChangeBattleState(BattleState newState)
    {
        curBattleState = newState;
        OnStateChanged?.Invoke(curBattleState);
    }
   
    public void OnClick_SkillSlot(string skillId)
    {
        _selectedSkillId = skillId;
        ChangeBattleState(BattleState.ChoiceTarget);
    }
    
}

