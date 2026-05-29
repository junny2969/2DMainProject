using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum SkillPopupCategory
{
    None = 0,
    NormalSkill,
    SpeacialSkill,
    UseItem
}
public class BattleActionPopup : DaniTechUIBase
{
    [Header("동적 생성할 프리팹")]
    [SerializeField] private GameObject Prefab_Slot;

    [Header("상단 버튼")]
    [SerializeField] private DaniTechUIButton Btn_NormalSkill;
    [SerializeField] private DaniTechUIButton Btn_SpeacialSkill;
    [SerializeField] private DaniTechUIButton Btn_UseItem;

    [Header("하단 버튼")]
    [SerializeField] private Transform Root_SkillList;
    [SerializeField] private DaniTechUIButton Btn_SkillSlot;

    private UnitModel _unitModel;

    private void OnEnable()
    {
        var playerInfo = BattleManager.Inst.GetPlayerModel();
        _unitModel = playerInfo;
        // Debug.LogWarning("playerInfo :" + (_unitModel == null ? "null" : _unitModel.Data.Name));
        Btn_NormalSkill.BindOnClickButtonEvent(OnClick_NormalSkill);
        Btn_SpeacialSkill.BindOnClickButtonEvent(onClick_SpeacialSkill);
        Btn_UseItem.BindOnClickButtonEvent(onClick_UseItem);

        if (_unitModel == null) return;
        OnClick_NormalSkill();
    }

    private void OnDisable()
    {
        _unitModel = null;
    }

    private void OnClick_NormalSkill()
    {
        ClearNormalSkillList();
        OnClick_NormalSkillAsync().Forget();
    }

    private async UniTask OnClick_NormalSkillAsync()
    {
        await GetPlayerSkillList();
    }

    private void onClick_SpeacialSkill()
    {

    }

    private void onClick_UseItem()
    {
    }

    private void ClearNormalSkillList()
    {
        foreach (Transform child in Root_SkillList)
        {
            Destroy(child.gameObject);
        }
    }
    public async UniTask GetPlayerSkillList()
    {
        if (_unitModel == null)
        {
            Debug.LogWarning("_unitModel이 null");
            return;
        }
        var characterData = _unitModel.Data as DNCharacterData;
        if (characterData != null)
        {
            var skillList = characterData.SkillList;
            var playerSkill = skillList.Split(",");

            foreach(var skill in playerSkill)
            {
                var usableSkill = skill.Trim();
                var skillData = DaniTechGameDataManager.Instance.GetSkill(usableSkill);
                if (skillData == null) return;

                await CreateSkillSlot(skillData.Id, SkillPopupCategory.NormalSkill);
            }
        }
    }

    private async UniTask CreateSkillSlot(string dataId, SkillPopupCategory curCategory)
    {
        var gObj = Instantiate(Prefab_Slot, Root_SkillList);
        if (gObj == null) return;

        var getComponent = gObj.GetComponent<DaniTechUIButton>();
        var skillData = DaniTechGameDataManager.Instance.GetSkill(dataId);

        if(getComponent == null) return;
        getComponent.ChangeButtonText(skillData.Name);
        
        await getComponent.ChangeButtonImage(skillData.IconPath);

        void OnClickSkill()
        {
            TurnManager.Inst.OnClick_SkillSlot(dataId);
        }

        getComponent.BindOnClickButtonEvent(OnClickSkill);
    }
}
