using UnityEngine;
using UnityEngine.UI;

public class BattleUI : DaniTechUIBase
{
    [Header("동적 생성 위치")]
    [SerializeField] private Transform Root_player;
    [SerializeField] private Transform Root_monster;

    [Header("하단 UI 정보")]
    [SerializeField] private Slider Slider_PlayerHp;
    [SerializeField] private Slider Slider_PlayerMp;
    [SerializeField] private Slider Slider_MonsterHp;
    [SerializeField] private Slider Slider_MonsterMp;

    [Header("스킬 버튼")]
    [SerializeField] private DaniTechUIButton Btn_NomalAttack;
    [SerializeField] private DaniTechUIButton Btn_FirstAttack;
    [SerializeField] private DaniTechUIButton Btn_SecondAttack;
    [SerializeField] private DaniTechUIButton Btn_ThirdAttack;

    

    private void OnEnable()
    {
        //Btn_MyProfile.BindOnClickButtonEvent(OnClick_OpenMyProfile);
        //Btn_Atk.BindOnClickButtonEvent(OnClick_Atk);
        //Btn_Jump.BindOnClickButtonEvent(OnClick_Jump);
        //Btn_Harvest.BindOnClickButtonEvent(OnClick_Harvest);
        //Btn_Inventory.BindOnClickButtonEvent(OnClick_OpenInventory);
        //Btn_Test.BindOnClickButtonEvent(Onclick_Test);


        //Btn_NomalAttack.BindOnClickButtonEvent(Onclick_UseNormalAttack);
        //Btn_FirstAttack.BindOnClickButtonEvent(Onclick_UseFirstAttack);
        //Btn_SecondAttack.BindOnClickButtonEvent(Onclick_UseSecondAttack);
        //Btn_ThirdAttack.BindOnClickButtonEvent(Onclick_UseThirdAttack);
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
    private void Update()
    {

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

    public void SetPlayerUnit(UnitModel _unitModel)
    {
        var currentHp = _unitModel.CurrentHp;
        Slider_PlayerHp.value = currentHp / (float)_unitModel.Data.MaxHp;
        var currentMp = _unitModel.CurrentMp;
        Slider_PlayerMp.value = currentMp / (float)_unitModel.Data.MaxMp;

        _unitModel.OnHpChanged += OnPlayerHpChanged;
        _unitModel.OnMpChanged += OnPlayerMpChanged;
    }

    private void OnPlayerHpChanged(int currentHp, int maxHp)
    {
        Slider_PlayerHp.value = currentHp / (float)maxHp;
    }

    private void OnPlayerMpChanged(int currentMp, int maxMp)
    {
        Slider_PlayerMp.value = currentMp / (float)maxMp;
    }
    public void SetMonsterUnit(UnitModel _unitModel)
    {
        var currentHp = _unitModel.CurrentHp;
        Slider_MonsterHp.value = currentHp / (float)_unitModel.Data.MaxHp;
        var currentMp = _unitModel.CurrentMp;
        Slider_MonsterMp.value = currentMp / (float)_unitModel.Data.MaxMp;

        _unitModel.OnHpChanged += OnMonsterHpChanged;
        _unitModel.OnMpChanged += OnMonsterMpChanged;

    }

    private void OnMonsterHpChanged(int currentHp, int maxHp)
    {
        Slider_MonsterHp.value = currentHp / (float)maxHp;
    }

    private void OnMonsterMpChanged(int currentMp, int maxMp)
    {
        Slider_MonsterMp.value = currentMp / (float)maxMp;

    }


}
