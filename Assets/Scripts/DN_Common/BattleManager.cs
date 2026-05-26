using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("전투유닛 위치")]
    [SerializeField] private GameObject Prefab_BattlePlayer;
    [SerializeField] private Transform Root_BattlePlayer;
    [SerializeField] private GameObject Prefab_BattleMonster;
    [SerializeField] private Transform Root_BattleMonster;

    private List<UnitModel> _playerModels = new List<UnitModel>();
    private List<UnitModel> _enemyModels = new List<UnitModel>();

    public static BattleManager Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
    }

    public void StartBattle(List<string> playerList, List<string> monsterList)
    {
        foreach (string playerId in playerList)
        {
            var playerData = DaniTechGameDataManager.Instance.GetCharacterData(playerId);

            if(playerData != null)
            {
                var playerModel = new UnitModel(DaniTechGameObjectManager.Inst.GenerateInstanceId(), playerData);
                _playerModels.Add(playerModel);

                DaniTechGameObjectManager.Inst.RequestInitBattleUnit(playerModel.InstanceId, playerModel, Prefab_BattlePlayer, Root_BattlePlayer);

            }

        }

        foreach (string monsterId in monsterList)
        {
            var mobData = DaniTechGameDataManager.Instance.GetDNMonsterData(monsterId);

            if (mobData != null)
            {
                var mobModel = new UnitModel(DaniTechGameObjectManager.Inst.GenerateInstanceId(), mobData);
                _enemyModels.Add(mobModel);
            }
        }
    }

}
