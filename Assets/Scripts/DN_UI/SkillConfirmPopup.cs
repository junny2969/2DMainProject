using UnityEngine;
using UnityEngine.UI;

public class SkillConfirmPopup : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_Confirm;
    [SerializeField] private DaniTechUIButton Button_Cancel;
    [SerializeField] private Text Text_SkillNameConfirm;

    public void OnEnable()
    {
        Button_Confirm.BindOnClickButtonEvent(OnClick_Confirm);
        Button_Cancel.BindOnClickButtonEvent(OnClick_Cancel);
    }

    private void OnClick_Confirm()
    {
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.SkillConfirmPopup);
        TurnManager.Inst.ChangeBattleState(BattleState.ChoiceTarget);
    }

    private void OnClick_Cancel()
    {
        DaniTechUIManager.Instance.ClosePopupUI(DaniTechUIType.SkillConfirmPopup);

    }

    public void PrintConfirmText(string skillName)
    {
        Text_SkillNameConfirm.text = $"{skillName} 스킬을 사용하겠습니까?";

    }
}
