using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUnitView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("유닛 정보")]
    [SerializeField] private Image Image_Character;
    [SerializeField] private Text Text_Name;

    [Header("클릭 버튼")]
    [SerializeField] private DaniTechUIButton Button_Target;

    private BattleState _curState;
    
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        var curState = TurnManager.Inst.GetCurState();
        if (curState == BattleState.ChoiceTarget)
        {
            Image_Character.color = Color.yellow;

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image_Character.color = Color.white;

    }

}
