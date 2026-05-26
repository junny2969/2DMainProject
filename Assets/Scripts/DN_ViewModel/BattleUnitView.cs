using UnityEngine;
using UnityEngine.UI;

public class BattleUnitView : MonoBehaviour
{
    [SerializeField] private Image Image_Character;
    [SerializeField] private Slider Slider_Hp;
    [SerializeField] private Slider Slider_Mp;
    [SerializeField] private Text Text_Name;
    
    
    private UnitModel _model;

    public void InitBattleUnit(UnitModel model)
    {
        Text_Name.text = model.Data.Name;
        Slider_Hp.value = (model.CurrentHp / (float)model.Data.MaxHp);
        Slider_Mp.value = (model.CurrentMp / (float)model.Data.MaxMp);

        _model = model;
    }


}
