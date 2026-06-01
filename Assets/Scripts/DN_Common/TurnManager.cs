using Cysharp.Threading.Tasks;
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
                ActionMonsterAsync().Forget();
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
        ActivePlayerSkill(_targetModel).Forget();
    }

    private async UniTaskVoid ActivePlayerSkill(UnitModel targetUnit)
    {
        var skillData = DaniTechGameDataManager.Instance.GetSkill(_selectedSkillId);
        if (skillData == null) return;

        var playerModel = BattleManager.Inst.GetPlayerModel();
        var playerView = DaniTechGameObjectManager.Inst.GetBattleUnitView(playerModel.InstanceId);
        if(playerView != null)
        {
            await playerView.PlayAttackAction(GetCenterPosition());
        }


        targetUnit.TakeDamage(skillData.Damage);
        playerModel.TakeMp(skillData.CostMp);

        ChangeBattleState(BattleState.MonsterTurn);
    }

    private async UniTaskVoid ActionMonsterAsync()
    {
        var enemyModel = BattleManager.Inst.GetEnemyModel();

        var monsterView = DaniTechGameObjectManager.Inst.GetBattleUnitView(enemyModel.InstanceId);
        if(monsterView != null)
        {
            await monsterView.PlayAttackAction(GetCenterPosition());
        }

        var playerModel = BattleManager.Inst.GetPlayerModel();
        playerModel.TakeDamage(enemyModel.Data.Atk);

        ChangeBattleState(BattleState.PlayerTurn);
    }

   

    private Vector3 GetCenterPosition()
    {
        return BattleManager.Inst.GetBattleCenterPosition();
    }

    
}

