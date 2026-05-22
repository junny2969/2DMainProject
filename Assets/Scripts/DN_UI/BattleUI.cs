using UnityEngine;

public class BattleUI : DaniTechUIBase
{
    //[SerializeField] private DaniTechUIButton Btn_MyProfile;
    //[SerializeField] private DaniTechUIButton Btn_Atk;
    //[SerializeField] private DaniTechUIButton Btn_Jump;
    //[SerializeField] private DaniTechUIButton Btn_Harvest;
    //[SerializeField] private DaniTechUIButton Btn_Inventory;
    //[SerializeField] private DaniTechUIButton Btn_Test;
    //[SerializeField] private DaniTechUIButton Btn_GameBook;

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


        Btn_NomalAttack.BindOnClickButtonEvent(Onclick_UseNormalAttack);
        Btn_FirstAttack.BindOnClickButtonEvent(Onclick_UseFirstAttack);
        Btn_SecondAttack.BindOnClickButtonEvent(Onclick_UseSecondAttack);
        Btn_ThirdAttack.BindOnClickButtonEvent(Onclick_UseThirdAttack);
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

   

}
