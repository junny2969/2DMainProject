using System.Collections.Generic;
using UnityEngine;

public class DaniTechGameManager : MonoBehaviour
{
    public static DaniTechGameManager Inst { get; set; }

    // 플레이어를 캐싱
    //  public DaniTech_2DPlayer LocalPlayer; > GameObjectManager 또는 GameManager로 역할 이전
    // 플레이 중에 저장되어야 하는 정보들이 있는 위치
    private DaniTechPlayerModel _playerModel = new DaniTechPlayerModel();

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        // LoadSaveData();
    }

    public void StartNewGame()
    {
        _playerModel = DaniTechNetworkManager.Inst.GetDefaultPlayerData();
        DaniTechNetworkManager.Inst.RequstSaveData(_playerModel);
    }

    public bool LoadAndStartGame()
    {
        DaniTechPlayerModel loaded = DaniTechNetworkManager.Inst.RequstLoadSaveData();
        if (loaded == null) return false;

        _playerModel = loaded;
        return true;
    }
    public void SaveData()
    {
        DaniTechNetworkManager.Inst.RequstSaveData(_playerModel);
    }

    public void SaveAndEndGame()
    {
        SaveData();
        Application.Quit();
    }

    private void LoadSaveData()
    {
        _playerModel = DaniTechNetworkManager.Inst.RequstLoadSaveData();
    }

    public void IncreasePlayerExp(int exp)
    {
        // 추후에 한곳에서 관리할 수 있게 익스텐션으로 빼도 된다
        _playerModel.PlayerTotalExp += exp;
    }

    public void AddItem(string itemDataId, int addItemCount)
    {
        // 저장할때 고유값 ID를 부여하기 위해 사용
        long uniqueId = DaniTechGameUtil.GenerateUniqueId();

        // TODO : 우선 쉽게 사용할 수 있도록 중복 처리는 빼두었다. 습득할때마다 아이템이 하나씩 추가되도록 해두고
        // 추후에 중복값은 StackCount가 다 찰때까지 누적해줄 수 있도록 로직을 추가하자
        var newItem = new DaniTechItemModel();
        newItem.ItemUniqueId = uniqueId;
        newItem.ItemDataId = itemDataId;
        newItem.ItemStackCount = addItemCount;

        _playerModel.ItemList.Add(newItem);

        if (itemDataId == "Item_Apple_1")
        {
            CheckGameClear(itemDataId);
        }
    }

    public List<DaniTechItemModel> GetPlayerItemList()
    {
        // _playerModel이 Private이므로 외부에서 ItemList를 받아올 수 있게 Get함수를 사용한다
        return _playerModel.ItemList;
    }

    private void CheckGameClear(string itemDataId)
    {

        int appleCount = 0;
        foreach (var item in _playerModel.ItemList)
        {
            if (item.ItemDataId == "Item_Apple_1")
            {
                appleCount += item.ItemStackCount;
            }
        }

        if (appleCount >= 10)
        {
            GameClear();
        }
    }

    private void GameClear()
    {
        Debug.Log("게임 클리어");
    }

    public DaniTech_2DPlayer GetLocalPlayer()
    {
        return DaniTechGameObjectManager.Inst.GetLocalPlayer();
    }

    public List<string> GetPlayerSkillList()
    {
        return _playerModel.OwnedSkillIdList;
    }

    public string GetPlayerCharacterDataId()
    {
        return _playerModel.PlayerCharacterDataId;
    }
    public void SetPlayerCurrentStat(int hp, int mp)
    {
        _playerModel.CurrentHp = hp;
        _playerModel.CurrentMp = mp;

    }
    public int GetPlayerCurrentHp()
    {
        return _playerModel.CurrentHp;
    }

    public int GetPlayerCurrentMp()
    {
        return _playerModel.CurrentMp;
    }

    public int GetPlayerCurLevel()
    {
        return _playerModel.CurLevel;
    }
    public int GetPlayerCurAtk()
    {
        return _playerModel.CurAtk;
    }
    public int GetPlayerCurDef()
    {
        return _playerModel.CurDef;
    }
    public int GetPlayerCurInt()
    {
        return _playerModel.CurInt;
    }
    public int GetPlayerCurDex()
    {
        return _playerModel.CurDex;
    }
    public int GetPlayerCurLuk()
    {
        return _playerModel.CurLuk;
    }
   

    public List<string> GetPlayerSkillListByType(string skillType)
    {
        var filteredList = new List<string>();
        foreach (string skillId in _playerModel.OwnedSkillIdList)
        {
            var skillData = DaniTechGameDataManager.Instance.GetSkill(skillId);
            if (skillData == null) continue;
            if (skillData.SkillType == skillType)
            {
                filteredList.Add(skillId);
            }
        }
        return filteredList;
    }

    [ContextMenu("SaveData 경로 열기")]
    private void OpenSaveDataPath()
    {
        string path = Application.persistentDataPath;
        Debug.Log($"저장경로 : {path}");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(path);
#endif

    }

}
