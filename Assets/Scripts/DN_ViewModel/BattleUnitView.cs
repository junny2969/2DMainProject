using UnityEngine;
using UnityEngine.UI;

public class BattleUnitView : MonoBehaviour
{
    [SerializeField] private Image Image_Character;
  
    [SerializeField] private Text Text_Name;
    
    
    private UnitModel _model;

    public void InitBattleUnit(UnitModel model)
    {
        Text_Name.text = model.Data.Name;
       

        _model = model;
    }


}
