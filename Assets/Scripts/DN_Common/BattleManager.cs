using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("메인카메라")]
    [SerializeField] private GameObject MainCameraObject;

    [Header("전투유닛 위치")]
    [SerializeField] private GameObject Prefab_BattlePlayer;
    [SerializeField] private Transform Root_BattlePlayer;
    [SerializeField] private GameObject Prefab_BattleMonster;
    [SerializeField] private Transform Root_BattleMonster;
    [SerializeField] private Transform Transform_BattleCenter;

    [SerializeField] private GameObject BattleCamera;

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
        //var playerRoot = battleUi.GetPlayerRoot();
        //var monsterRoot = battleUi.GetPMonsterRoot();

        foreach (string playerId in playerList)
        {
            var playerData = DaniTechGameDataManager.Instance.GetCharacterData(playerId);
            // Debug.LogWarning("playerId:" + playerId);

            if(playerData != null)
            {
                var playerModel = new UnitModel(DaniTechGameObjectManager.Inst.GenerateInstanceId(), playerData);
                playerModel.OnDead += OnPlayerDead;
                //Debug.LogWarning("playerData: " + (playerData == null ? "null" : playerData.Name));
                _playerModels.Add(playerModel);

                DaniTechGameObjectManager.Inst.RequestInitBattleUnit(playerModel.InstanceId, playerModel, Prefab_BattlePlayer, Root_BattlePlayer);
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

                DaniTechGameObjectManager.Inst.RequestInitBattleUnit(mobModel.InstanceId, mobModel, Prefab_BattleMonster, Root_BattleMonster);
                battleUi.SetMonsterUnit(mobModel);

            }
        }
        TurnManager.Inst.StartBattle();
    }

    public async UniTaskVoid EnterBattle(List<string> playerList, List<string> monsterList)
    {
        DaniTechUIManager.Instance.CloseUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);
        DaniTechGameObjectManager.Inst.HideLocalPlayer();

        
        var cameraFollow = MainCameraObject.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetFollowActive(false);
        }

        BattleCamera.SetActive(true);

        MainCameraObject.SetActive(false);

        

        DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.BattleUI);
        await StartBattle(playerList, monsterList);
    }

    public void EnterBattleFromField(string monsterDataId)
    {
        List<string> playerList = new List<string>();
        playerList.Add("character_ellie_01");

        List<string> monsterList = new List<string>();
        monsterList.Add(monsterDataId);

        EnterBattle(playerList, monsterList).Forget();
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

    

    private void OnPlayerDead()
    {
        OnPlayerDeadAsyck().Forget();
    }

    private async UniTask OnPlayerDeadAsyck()
    {
        DaniTechUIManager.Instance.OpenBattleResultPopup("패 배");
        await UniTask.Delay(TimeSpan.FromSeconds(1.5));
        RestoreFromBattle();
        DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.Lobby_UI);
        //DaniTechUIManager.Instance.OpenUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);


        // TODO 마을 귀환

    }

    private void OnMonsterDead()
    {
        OnMonsterDeadAsyck().Forget();
    }
    private async UniTask OnMonsterDeadAsyck()
    {
        DaniTechUIManager.Instance.OpenBattleResultPopup("승 리");
        await UniTask.Delay(TimeSpan.FromSeconds(1.5));

        RestoreFromBattle();
        DaniTechUIManager.Instance.OpenUI(DaniTechUIRootType.MainUI, DaniTechUIType.MainUI);

        //TODO 승리 다이얼로그 > 엔딩
    }

    public Vector3 GetBattleCenterPosition()
    {
        if(Transform_BattleCenter == null)
        {
            Debug.LogWarning("BattleCenter가 null");
            return Vector3.zero;
        }
        return Transform_BattleCenter.position;
    }

    public Transform GetBattlePlayerRoot()
    {
        return Root_BattlePlayer;
    }

    public Transform GetBattleMonsterRoot()
    {
        return Root_BattleMonster;
    }

    private void RestoreFromBattle()
    {
        BattleCamera.SetActive(false);
        MainCameraObject.SetActive(true);
        var cameraFollow = MainCameraObject.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetFollowActive(true);
        }

        _playerModels.Clear();
        _enemyModels.Clear();

        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.BattleUI);
        DaniTechGameObjectManager.Inst.ReSpawnLocalPlayer();
       
        // DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.Lobby_UI);
    }
}
