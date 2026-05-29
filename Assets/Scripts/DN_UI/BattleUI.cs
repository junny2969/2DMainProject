using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : DaniTechUIBase
{
    [Header("동적 생성 위치")]
    [SerializeField] private Transform Root_player;
    [SerializeField] private Transform Root_monster;

    [Header("하단 UI 정보")]

    [SerializeField] private DaniTechUIButton Btn_Player;

    [SerializeField] private Slider Slider_PlayerHp;
    [SerializeField] private Slider Slider_PlayerMp;
    [SerializeField] private Slider Slider_MonsterHp;
    [SerializeField] private Slider Slider_MonsterMp;

    [SerializeField] private Image Image_ArrowPlayer;
    [SerializeField] private Image Image_ArrowMonster;

    [SerializeField] private Image Image_PlayerFace;

    [SerializeField] private Text Text_PlayerName;
    [SerializeField] private Text Text_MonsterName;

    [SerializeField] private Text Text_PlayerHp;
    [SerializeField] private Text Text_PlayerMp;
    [SerializeField] private Text Text_MonsterHp;
    [SerializeField] private Text Text_MonsterMp;


    [Header("스킬 버튼")]
    [SerializeField] private DaniTechUIButton Btn_NomalAttack;
    [SerializeField] private DaniTechUIButton Btn_FirstAttack;
    [SerializeField] private DaniTechUIButton Btn_SecondAttack;
    [SerializeField] private DaniTechUIButton Btn_ThirdAttack;




    private void OnEnable()
    {
        TurnManager.Inst.OnStateChanged += OnBattleStateChanged;

        Btn_Player.BindOnClickButtonEvent(OnClick_PlayerIcon);
        


    }

    private void OnBattleStateChanged(BattleState curBattleState)
    {
        Image_ArrowPlayer.gameObject.SetActive(false);

        switch (curBattleState)
        {

            case BattleState.PlayerTurn:
                Image_ArrowPlayer.gameObject.SetActive(true);

                break;

            case BattleState.ChoiceAction:
                DaniTechUIManager.Instance.OpenPopupUI(DaniTechUIType.BattleActionPopup);
                break;

            case BattleState.ChoiceTarget:
                DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.BattleActionPopup);
                break;

            case BattleState.PlayerAction:
                break;

            case BattleState.MonsterTurn:
                break;

        }
    }

    public void OnClick_PlayerIcon()
    {
        TurnManager.Inst.ChangeBattleState(BattleState.ChoiceAction);
    }

    public void Onclick_UseNormalAttack()
    {
        var localPlayer = DaniTechGameManager.Inst.GetLocalPlayer();
        localPlayer.UseNormalAttack();
    }

    public void Onclick_UseFirstAttack()
    {
        var localPlayer = DaniTechGameManager.Inst.GetLocalPlayer();
        localPlayer.UseFirstlSkill();
    }

    public void Onclick_UseSecondAttack()
    {
        var localPlayer = DaniTechGameManager.Inst.GetLocalPlayer();
        localPlayer.UseSecondSkill();

    }

    public void Onclick_UseThirdAttack()
    {
        var localPlayer = DaniTechGameManager.Inst.GetLocalPlayer();
        localPlayer.UseThirdSkill();

    }
 

    public void OnClick_OpenMyProfile()
    {

    }

    public void OnClick_Atk()
    {
    }

    public void OnClick_Jump()
    {
    }

    public void OnClick_Harvest()
    {
    }

    public void OnClick_OpenInventory()
    {

    }

    public void Onclick_Test()
    {
        DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.InventoryUI);
    }

    public Transform GetPlayerRoot()
    {
        return Root_player;
    }
    public Transform GetPMonsterRoot()
    {
        return Root_monster;
    }

    public async UniTask SetPlayerUnit(UnitModel _unitModel)
    {
        var playerIcon = await DaniTechResourceManager.Inst.LoadSprite(_unitModel.Data.IconPath);
        Image_PlayerFace.sprite = playerIcon;
        Text_PlayerName.text = _unitModel.Data.Name;

        var currentHp = _unitModel.CurrentHp;
        Slider_PlayerHp.value = currentHp / (float)_unitModel.Data.MaxHp;
        var currentMp = _unitModel.CurrentMp;
        Slider_PlayerMp.value = currentMp / (float)_unitModel.Data.MaxMp;

        Text_PlayerHp.text = ("H.P  :  " + _unitModel.CurrentHp + "  /  " + _unitModel.Data.MaxHp);
        Text_PlayerMp.text = ("M.P  :  " + _unitModel.CurrentMp + "  /  " + _unitModel.Data.MaxMp);
      


        _unitModel.OnHpChanged += OnPlayerHpChanged;
        _unitModel.OnMpChanged += OnPlayerMpChanged;
    }

    private void OnPlayerHpChanged(int currentHp, int maxHp)
    {
        Text_PlayerHp.text = ("H.P  :  " + currentHp + "  /  " + maxHp);
        Slider_PlayerHp.value = currentHp / (float)maxHp;
    }

    private void OnPlayerMpChanged(int currentMp, int maxMp)
    {
        Text_PlayerMp.text = ("M.P  :  " + currentMp + "  /  " + maxMp);

        Slider_PlayerMp.value = currentMp / (float)maxMp;
    }
    public void SetMonsterUnit(UnitModel _unitModel)
    {
        Text_MonsterName.text = _unitModel.Data.Name;

        var currentHp = _unitModel.CurrentHp;
        Slider_MonsterHp.value = currentHp / (float)_unitModel.Data.MaxHp;
        var currentMp = _unitModel.CurrentMp;
        Slider_MonsterMp.value = currentMp / (float)_unitModel.Data.MaxMp;

        Text_MonsterHp.text = ("H.P  :  " + _unitModel.CurrentHp + "  /  " + _unitModel.Data.MaxHp);
        Text_MonsterMp.text = ("M.P  :  " + _unitModel.CurrentMp + "  /  " + _unitModel.Data.MaxMp);

        _unitModel.OnHpChanged += OnMonsterHpChanged;
        _unitModel.OnMpChanged += OnMonsterMpChanged;

    }

    private void OnMonsterHpChanged(int currentHp, int maxHp)
    {
        Text_MonsterHp.text = ("H.P  :  " + currentHp + "  /  " + maxHp);

        Slider_MonsterHp.value = currentHp / (float)maxHp;
    }

    private void OnMonsterMpChanged(int currentMp, int maxMp)
    {
        Text_MonsterMp.text = ("M.P  :  " + currentMp + "  /  " + maxMp);

        Slider_MonsterMp.value = currentMp / (float)maxMp;

    }


}
