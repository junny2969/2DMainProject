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
    private UnitModel _targetModel;

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

        switch(newState)
        {
            case BattleState.MonsterTurn:
                ActionMonster();
                break;
        }
    }
   
    public void OnClick_SkillSlot(string skillId)
    {
        _selectedSkillId = skillId;
        DaniTechUIManager.Instance.OpenSkillConfirmPopup(skillId);
    }
    
    public BattleState GetCurState()
    {
        return curBattleState;
    }

    public void SaveTarget(UnitModel target)
    {
        _targetModel = target;
        ChangeBattleState(BattleState.PlayerAction);
        ActivePlayerSkill(_targetModel);
    }

    public void ActivePlayerSkill(UnitModel targetUnit)
    {
        var skillData = DaniTechGameDataManager.Instance.GetSkill(_selectedSkillId);
        targetUnit.TakeDmage(skillData.Damage);

        var caster = BattleManager.Inst.GetPlayerModel();
        caster.TakeMp(skillData.CostMp);

        ChangeBattleState(BattleState.MonsterTurn);

    }

    public void ActionMonster()
    {
        var attacker = BattleManager.Inst.GetEnemyModel();
        var damage = attacker.Data.Atk;
        var target = BattleManager.Inst.GetPlayerModel();

        target.TakeDmage(damage);
        ChangeBattleState(BattleState.PlayerTurn);
    }

    
}

