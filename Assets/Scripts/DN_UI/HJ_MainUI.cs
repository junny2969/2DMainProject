using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HJ_MainUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Btn_MyProfile;
    [SerializeField] private DaniTechUIButton Btn_Left;
    [SerializeField] private DaniTechUIButton Btn_Down;
    [SerializeField] private DaniTechUIButton Btn_Right;
    [SerializeField] private DaniTechUIButton Btn_Up;

    [SerializeField] private DaniTechUIButton Btn_Inventory;
    [SerializeField] private DaniTechUIButton Btn_Test;

    [Header("프로필 텍스트")]
    [SerializeField] private Text Text_PlayerName;
    [SerializeField] private Text Text_PlayerInfo;

    [Header("기능테스트용")]
    [SerializeField] private List<string> playerList;
    [SerializeField] private List<string> monsterList;



    private void OnEnable()
    {
        Btn_MyProfile.BindOnClickButtonEvent(OnClick_OpenMyProfile);
        Btn_Left.BindOnClickButtonEvent(OnClick_Left);
        Btn_Down.BindOnClickButtonEvent(OnClick_Down);
        Btn_Right.BindOnClickButtonEvent(OnClick_Right);
        Btn_Up.BindOnClickButtonEvent(OnClick_Up);

        Btn_Inventory.BindOnClickButtonEvent(OnClick_OpenInventory);
        Btn_Test.BindOnClickButtonEvent(OnClick_Test);

        RefreshPlayerProfile();
        
    }

    private void Update()
    {
        
    }

    public void RefreshPlayerProfile()
    {
        var localPlayer = DaniTechGameObjectManager.Inst.GetLocalPlayer();
        if(localPlayer == null)
        {
            return;
        }

        string characterDataId = localPlayer.GetCharacterDataId();
        var characterData = DaniTechGameDataManager.Instance.GetCharacterData(characterDataId);
        if (characterData == null)
        {
            return;
        }

        Text_PlayerName.text = characterData.Name;
        Text_PlayerInfo.text = characterData.Description;
    }

    public void OnClick_OpenMyProfile()
    {
        
    }

    public void OnClick_Left()
    {
    }

    public void OnClick_Down() 
    {
    }

    public void OnClick_Right()
    {
    }

    public void OnClick_Up()
    {

    }
    public void OnClick_OpenInventory()
    {
        DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.InventoryUI);

    }

    public void OnClick_Test()
    {
        BattleManager.Inst.EnterBattle(playerList, monsterList).Forget();
    }

   

}
