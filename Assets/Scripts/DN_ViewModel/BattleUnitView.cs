using UnityEngine;
using UnityEngine.UI;

public class BattleUnitView : MonoBehaviour
{
    [Header("유닛 정보")]
    [SerializeField] private Image Image_Character;
    [SerializeField] private Text Text_Name;

    [Header("클릭 버튼")]
    [SerializeField] private DaniTechUIButton Button_Target;
    
    
    private UnitModel _model;

    private void OnEnable()
    {
        if (Button_Target == null) return;
        Button_Target.BindOnClickButtonEvent(OnClick_Target);
    }
    public void InitBattleUnit(UnitModel model)
    {
        Text_Name.text = model.Data.Name;
       

        _model = model;
    }

    private void OnClick_Target()
    {
        var curState = TurnManager.Inst.GetCurState();
        if(curState == BattleState.ChoiceTarget)
        {
            TurnManager.Inst.SaveTarget(_model);
        }
    }

}
