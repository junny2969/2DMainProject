using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("전투유닛 위치")]
    [SerializeField] private GameObject Prefab_BattlePlayer;
    // [SerializeField] private Transform Root_BattlePlayer;
    [SerializeField] private GameObject Prefab_BattleMonster;
    // [SerializeField] private Transform Root_BattleMonster;

    private List<UnitModel> _playerModels = new List<UnitModel>();
    private List<UnitModel> _enemyModels = new List<UnitModel>();

    public static BattleManager Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
    }

   

    public async UniTask StartBattle(List<string> playerList, List<string> monsterList)
    {
        var battleUi = DaniTechUIManager.Instance.GetOpenedUI(DaniTechUIRootType.ContentUI, DaniTechUIType.BattleUI) as BattleUI;
        var playerRoot = battleUi.GetPlayerRoot();
        var monsterRoot = battleUi.GetPMonsterRoot();

        foreach (string playerId in playerList)
        {
            var playerData = DaniTechGameDataManager.Instance.GetCharacterData(playerId);
            Debug.LogWarning("playerId:" + playerId);

            if(playerData != null)
            {
                var playerModel = new UnitModel(DaniTechGameObjectManager.Inst.GenerateInstanceId(), playerData);
                playerModel.OnDead += OnPlayerDead;
                //Debug.LogWarning("playerData: " + (playerData == null ? "null" : playerData.Name));
                _playerModels.Add(playerModel);

                DaniTechGameObjectManager.Inst.RequestInitBattleUnit(playerModel.InstanceId, playerModel, Prefab_BattlePlayer, playerRoot);
                await battleUi.SetPlayerUnit(playerModel);

            }
        }

        foreach (string monsterId in monsterList)
        {
            var mobData = DaniTechGameDataManager.Instance.GetDNMonsterData(monsterId);

            if (mobData != null)
            {
                var mobModel = new UnitModel(DaniTechGameObjectManager.Inst.GenerateInstanceId(), mobData);
                mobModel.OnDead += OnMonsterDead;
                _enemyModels.Add(mobModel);

                DaniTechGameObjectManager.Inst.RequestInitBattleUnit(mobModel.InstanceId, mobModel, Prefab_BattleMonster, monsterRoot);
                battleUi.SetMonsterUnit(mobModel);

            }
        }
        TurnManager.Inst.StartBattle();
    }

    public UnitModel GetPlayerModel()
    {
        if (_playerModels.Count == 0) return null;
        return _playerModels[0];
    }

    public UnitModel GetEnemyModel()
    {
        if (_enemyModels.Count == 0) return null;
        return _enemyModels[0]; // TODO 몬스터 늘어날시 개선 필요
    }

    private async UniTaskVoid OnPlayerDeadAsyck()
    {
        DaniTechUIManager.Instance.OpenBattleResultPopup("패 배");
        await UniTask.Delay(TimeSpan.FromSeconds(1.5));
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.BattleUI);
        // TODO 마을 귀환
        
    }

    private void OnPlayerDead()
    {
        OnPlayerDeadAsyck().Forget();
    }
    private async UniTaskVoid OnMonsterDeadAsyck()
    {
        DaniTechUIManager.Instance.OpenBattleResultPopup("승 리");
        await UniTask.Delay(TimeSpan.FromSeconds(1.5));
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.BattleUI);

    }

    private void OnMonsterDead()
    {
        OnMonsterDeadAsyck().Forget();
    }

}
