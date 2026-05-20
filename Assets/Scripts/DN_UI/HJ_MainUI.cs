using UnityEngine;

public class HJ_MainUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Btn_MyProfile;
    [SerializeField] private DaniTechUIButton Btn_Atk;
    [SerializeField] private DaniTechUIButton Btn_Jump;
    [SerializeField] private DaniTechUIButton Btn_Harvest;
    [SerializeField] private DaniTechUIButton Btn_Inventory;
    [SerializeField] private DaniTechUIButton Btn_Test;
    [SerializeField] private DaniTechUIButton Btn_GameBook;

    


    private void OnEnable()
    {
        Btn_MyProfile.BindOnClickButtonEvent(OnClick_OpenMyProfile);
        Btn_Atk.BindOnClickButtonEvent(OnClick_Atk);
        Btn_Jump.BindOnClickButtonEvent(OnClick_Jump);
        Btn_Harvest.BindOnClickButtonEvent(OnClick_Harvest);
        Btn_Inventory.BindOnClickButtonEvent(OnClick_OpenInventory);
        Btn_Test.BindOnClickButtonEvent(Onclick_Test);
        
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
        DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.InventoryUI);

    }

    public void Onclick_Test()
    {
        DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.InventoryUI);
        // DaniTechUIManager.Instance.OpenContentUI(DaniTechUIType.BattleUI);
    }


}
