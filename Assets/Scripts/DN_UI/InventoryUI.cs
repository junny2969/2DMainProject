using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum EInventoryCategory
{
    None = 0,
    SkillCategory,
    PotionCategory,
    EqupmentCategory,
    WeaponCategory,
    QuestItemCategory
}

public class CalculatedStat
{
    public int Atk;
    public int Def;
    public int Int;
    public int Dex;
    public int Luk;
}
public class InventoryUI : DaniTechUIBase
{
    [Header("동적 생성할 프리팹")]
    [SerializeField] private GameObject Prefab_Slot;

    [Header("장착한 아이템 정보")]
    [SerializeField] private Image Image_CurWeapon;
    [SerializeField] private Text Text_CurWeapon;
    [SerializeField] private Image Image_CurEqupment;
    [SerializeField] private Text Text_CurEqupment;


    [Header("버튼리스트")]
    [SerializeField] private DaniTechUIButton Button_CloseInventory;
    [SerializeField] private DaniTechUIButton Button_OpenSkill;
    [SerializeField] private DaniTechUIButton Button_OpenPotion;
    [SerializeField] private DaniTechUIButton Button_OpenEqupment;
    [SerializeField] private DaniTechUIButton Button_Weapon;
    [SerializeField] private DaniTechUIButton Button_UseItem;

    [Header("상단 스탯")]
    [SerializeField] private Image Image_PlayerIcon;
    [SerializeField] private Text Text_PlayerName_Top;
    [SerializeField] private Text Text_Level_Stat;
    [SerializeField] private Text Text_Hp_Stat;
    [SerializeField] private Text Text_Mp_Stat;
    [SerializeField] private Text Text_Exp_Stat;

    [SerializeField] private Text Text_Atk_Stat;
    [SerializeField] private Text Text_Def_Stat;
    [SerializeField] private Text Text_Int_Stat;
    [SerializeField] private Text Text_Dex_Stat;
    [SerializeField] private Text Text_Luk_Stat;

    [Header("장착시 스탯")]
    [SerializeField] private Text Text_Atk_AfterStat;
    [SerializeField] private Text Text_Def_AfterStat;
    [SerializeField] private Text Text_Int_AfterStat;
    [SerializeField] private Text Text_Dex_AfterStat;
    [SerializeField] private Text Text_Luk_AfterStat;

    [Header("캐릭터 정보")]
    [SerializeField] private Text Text_PlayerName;
    [SerializeField] private Slider Slider_Hp;
    [SerializeField] private Text Text_PlayerHp;
    [SerializeField] private Slider Slider_Mp;
    [SerializeField] private Text Text_PlayerMp;
    [SerializeField] private Slider Slider_Exp;



    [Header("슬롯 리스트 영역")]
    [SerializeField] private Transform Transform_SlotRoot;

    [Header("아이템 설명")]
    [SerializeField] private Image Image_ItemIcon;
    [SerializeField] private Text Text_ItemName;
    [SerializeField] private Text Text_Description;

    private string _selectedDataId;
    private EInventoryCategory _selectedCategory;
    // private string previewWeapon;
    // private string previewArmor;



    // [Header("부가정보")] >> 추후 레이아웃을 껏다켰다 할때 사용
    // [SerializeField] private GameObject Layout_SubInfoSkill;
    private Dictionary<string, InventorySlotUI> _slotList = new Dictionary<string, InventorySlotUI>();


    private void OnEnable()

    {
        OnClick_OpenPotion(); // 이 UI가 열릴때 스스로 기본적으로 아이템 도감안에 있는 모든 데이터를 불러온다.

        Button_CloseInventory.BindOnClickButtonEvent(OnClick_CloseInventory);
        Button_OpenSkill.BindOnClickButtonEvent(OnClick_OpenSkill);
        Button_OpenPotion.BindOnClickButtonEvent(OnClick_OpenPotion);
        Button_OpenEqupment.BindOnClickButtonEvent(OnClick_OpenEqupment);
        Button_Weapon.BindOnClickButtonEvent(OnClick_Weapon);
        Button_UseItem.BindOnClickButtonEvent(OnClick_UseItem);

        InitCharacterStatus();
        RefreshEquipmentSlot();
        // Button_UseItem.gameObject.SetActive(false);
    }

    private void OnClick_CloseInventory()
    {
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.InventoryUI);
    }

    private void OnClick_UseItem()
    {
        Debug.LogWarning($"UseItem 클릭. category:{_selectedCategory}, dataId : {_selectedDataId}");
        switch(_selectedCategory)
        {
            case EInventoryCategory.WeaponCategory:
                DaniTechGameManager.Inst.SetEquippedWeaponId(_selectedDataId);
                RefreshEquipmentSlot();
                InitCharacterStatus();
                break;
            case EInventoryCategory.EqupmentCategory:
                DaniTechGameManager.Inst.SetEquippedArmorId(_selectedDataId);
                RefreshEquipmentSlot();
                InitCharacterStatus();

                break;
            case EInventoryCategory.PotionCategory:
                // TODO 포션사용구현
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
       
    }

    private void OnClick_OpenSkill()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.SkillCategory);
    }

    private void OnClick_OpenPotion()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.PotionCategory);

    }

    private void OnClick_OpenEqupment()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.EqupmentCategory);

    }

    private void OnClick_Weapon()
    {
        SetInventoryLayoutByCategory(EInventoryCategory.WeaponCategory);

    }
    

    private void RequestSelectUseItm()
    {

    }

    private void RemoveItemSlot()
    {
        // 저장 정보에서 먼저 아이템이 제거된 후에 슬롯이 제거되어야 한다
    }

    private void SetInventoryLayoutByCategory(EInventoryCategory category)
    {
        DestroyAndClearSlotList();
        RefreshUseItemButton(category);
        switch (category)
        {
            case EInventoryCategory.SkillCategory:
                ReadSkillListAndCreateSlot();
                break;
            case EInventoryCategory.PotionCategory:
                ReadPotionListAndCreateSlot();
                break;
            case EInventoryCategory.EqupmentCategory:
                ReadEqupmentListAndCreateSlot();
                break;
            case EInventoryCategory.WeaponCategory:
                ReadWeaponListAndCreateSlot();
                break;
            default:
                break;
        }
    }

    private void RefreshUseItemButton(EInventoryCategory category)
    {
        switch (category)
        {
            case EInventoryCategory.SkillCategory:
                Button_UseItem.gameObject.SetActive(false);
                break;
            case EInventoryCategory.PotionCategory:
                Button_UseItem.gameObject.SetActive(true);
                Button_UseItem.ChangeButtonText("사용");
                break;
            case EInventoryCategory.EqupmentCategory:
                Button_UseItem.gameObject.SetActive(true);
                Button_UseItem.ChangeButtonText("장착");
                break;
            case EInventoryCategory.WeaponCategory:
                Button_UseItem.gameObject.SetActive(true);
                Button_UseItem.ChangeButtonText("장착");
                break;
            default:
                Button_UseItem.gameObject.SetActive(false);
                break;


        }
    }

    private void DestroyAndClearSlotList()
    {
        if(_slotList.Count > 0)
        {
            foreach(var slotKv in _slotList)
            {
                var slot = slotKv.Value;
                DestroyImmediate(slot.gameObject);
            }

            _slotList.Clear();
        }
    }
    private void ReadSkillListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.SkillDataList;
        foreach (var dataKv in dataList)
        {
            DNSkillData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.SkillCategory);
        }
        SelectFirstSlot();
    }

    private void ReadPotionListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.PotionDataList;
        foreach (var dataKv in dataList)
        {
            PotionData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.PotionCategory);
        }

        SelectFirstSlot();
    }

    private void ReadEqupmentListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.EqupmentDataList;
        foreach (var dataKv in dataList)
        {
            EqupmentData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.EqupmentCategory);
        }

        SelectFirstSlot();
    }

    private void ReadWeaponListAndCreateSlot()
    {
        var dataList = DaniTechGameDataManager.Instance.WeaponDataList;
        foreach (var dataKv in dataList)
        {
            DNWeaponData data = dataKv.Value;
            if (data == null) return;
            CreateInventorySlot(data.Id, EInventoryCategory.WeaponCategory);
        }

        SelectFirstSlot();
    }

    private void SelectFirstSlot()
    {
        if(_slotList.Count > 0)
        {
            foreach (var slotKv in _slotList)
            {
                var slot = slotKv.Value;
                slot.OnClick_InventorySlot();
            }
        }
    }


    private void CreateInventorySlot(string dataId, EInventoryCategory curCategory)
    {
        var gObj = Instantiate(Prefab_Slot, Transform_SlotRoot);
        if (gObj == null) return;

        var slotComponent = gObj.GetComponent<InventorySlotUI>();
        if (slotComponent == null) return;

        // 동적 생성된 자식슬롯(게임오브젝트)안에 있는 컴포넌트도 잘 가져왔다.
        slotComponent.InitSlot(dataId, curCategory, OnClickChildSlotSelected);
        _slotList.Add(dataId, slotComponent);
    }

    private void SetDetailInforUI(string dataName, string dataDescription = "", string iconPath = "")
    {
       
        Text_ItemName.text = dataName;
        Text_Description.text = dataDescription;

        if (string.IsNullOrEmpty(iconPath) == false)
        {
            DaniTechGameUtil.LoadAndSetSpriteImage(Image_ItemIcon, iconPath).Forget();
        }

        Image_ItemIcon.gameObject.SetActive(string.IsNullOrEmpty(iconPath) == false);

    }
    private void OnClickChildSlotSelected(string slotDataId, EInventoryCategory selectedCatogory)
    {
        _selectedDataId = slotDataId;
        _selectedCategory = selectedCatogory;
        RefreshStatPreview(slotDataId, selectedCatogory);


        if (selectedCatogory == EInventoryCategory.SkillCategory)
        {
            var currentSelectedData = DaniTechGameDataManager.Instance.GetSkill(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);
        }

        else if(selectedCatogory == EInventoryCategory.PotionCategory)
        {
            var currentSelectedData = DaniTechGameDataManager.Instance.GetPotionData(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);
        }

        else if(selectedCatogory == EInventoryCategory.EqupmentCategory)
        {
            var currentSelectedData = DaniTechGameDataManager.Instance.GetEqupmentData(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);
        }

        else if(selectedCatogory == EInventoryCategory.WeaponCategory)
        {
            var currentSelectedData = DaniTechGameDataManager.Instance.GetWeaponData(slotDataId);
            if (currentSelectedData == null) return;

            SetDetailInforUI(currentSelectedData.Name, currentSelectedData.Description, currentSelectedData.IconPath);
        }

        foreach (var slotKv in _slotList)
        {
            var slot = slotKv.Value;
            var dataId = slot.GetSlotDataId();
            slot.SetSelectedUI(slotDataId == dataId);
        }
    }

    private void InitCharacterStatus()
    {

        string characterDataId = DaniTechGameManager.Inst.GetPlayerCharacterDataId();
        var charData = DaniTechGameDataManager.Instance.GetCharacterData(characterDataId);
        if (charData == null) return;

        int curLevel = DaniTechGameManager.Inst.GetPlayerCurLevel();
        int curAtk = DaniTechGameManager.Inst.GetPlayerCurAtk();
        int curDef = DaniTechGameManager.Inst.GetPlayerCurDef();
        int curInt = DaniTechGameManager.Inst.GetPlayerCurInt();
        int curDex = DaniTechGameManager.Inst.GetPlayerCurDex();
        int curLuk = DaniTechGameManager.Inst.GetPlayerCurLuk();

        int currentHp = DaniTechGameManager.Inst.GetPlayerCurrentHp();
        int currentMp = DaniTechGameManager.Inst.GetPlayerCurrentMp();

        int atkBonus = 0;
        int intBonus = 0;
        int defBonus = 0;
        int dexBonus = 0;
        int lukBonus = 0;

        string weaponId = DaniTechGameManager.Inst.GetEquippedWeaponId();
        if(string.IsNullOrEmpty(weaponId) == false)
        {
            var weaponData = DaniTechGameDataManager.Instance.GetWeaponData(weaponId);
            if(weaponData != null)
            {
                atkBonus += weaponData.AtkBonus;
                intBonus += weaponData.IntBonus;
            }
        }

        string armorId = DaniTechGameManager.Inst.GetEquippedArmorId();
        if(string.IsNullOrEmpty(armorId) == false)
        {
            var armorData = DaniTechGameDataManager.Instance.GetEqupmentData(armorId);
            if(armorData != null)
            {
                defBonus += armorData.DefBonus;
                dexBonus += armorData.DexBonus;
                lukBonus += armorData.LukBonus;
            }
        }

        if(Image_PlayerIcon != null && string.IsNullOrEmpty(charData.IconPath) == false)
        {
            DaniTechGameUtil.LoadAndSetSpriteImage(Image_PlayerIcon, charData.IconPath).Forget();
        }

        Text_PlayerName_Top.text = charData.Name;
        Text_Level_Stat.text = $"{curLevel}";
        Text_Hp_Stat.text = $"{currentHp} / {charData.MaxHp}";
        Text_Mp_Stat.text = $"{currentMp} / {charData.MaxMp}";
        Text_Atk_Stat.text = $"{curAtk + atkBonus}";
        Text_Def_Stat.text = $"{curDef + defBonus}";
        Text_Int_Stat.text = $"{curInt + intBonus}";
        Text_Dex_Stat.text = $"{curDex + dexBonus}";
        Text_Luk_Stat.text = $"{curLuk + lukBonus}";

        Text_PlayerName.text = charData.Name;
        Text_PlayerHp.text = $"{currentHp} / {charData.MaxHp}";
        Text_PlayerMp.text = $"{currentMp} / {charData.MaxMp}";

        Slider_Hp.value = (float)currentHp / charData.MaxHp;
        Slider_Mp.value = (float)currentMp / charData.MaxMp;

    }

    private void RefreshEquipmentSlot()
    {
        string weaponId = DaniTechGameManager.Inst.GetEquippedWeaponId();
        if(string.IsNullOrEmpty(weaponId) == false)
        {
            var weaponData = DaniTechGameDataManager.Instance.GetWeaponData(weaponId);
            if(weaponData != null)
            {
                Text_CurWeapon.text = weaponData.Name;
                DaniTechGameUtil.LoadAndSetSpriteImage(Image_CurWeapon, weaponData.IconPath).Forget();
                Image_CurWeapon.gameObject.SetActive(true);
            }
        }
        else
        {
            Text_CurWeapon.text = "";
            Image_CurWeapon.gameObject.SetActive (false);
        }

        string armorId = DaniTechGameManager.Inst.GetEquippedArmorId();
        if(string.IsNullOrEmpty (armorId) == false)
        {
            var armorData = DaniTechGameDataManager.Instance.GetEqupmentData(armorId);
            if(armorData != null)
            {
                Text_CurEqupment.text = armorData.Name;
                DaniTechGameUtil.LoadAndSetSpriteImage(Image_CurEqupment, armorData.IconPath).Forget();
                Image_CurEqupment.gameObject.SetActive(true);
            }

        }
        else
        {
            Text_CurEqupment.text = "";
            Image_CurEqupment.gameObject.SetActive(false);
        }
    }

    private CalculatedStat CalculateStats(string weaponId, string armorId)
    {
        var stat = new CalculatedStat();

        stat.Atk = DaniTechGameManager.Inst.GetPlayerCurAtk();
        stat.Def = DaniTechGameManager.Inst.GetPlayerCurDef();
        stat.Int = DaniTechGameManager.Inst.GetPlayerCurInt();
        stat.Dex = DaniTechGameManager.Inst.GetPlayerCurDex();
        stat.Luk = DaniTechGameManager.Inst.GetPlayerCurLuk();

        if (string.IsNullOrEmpty(weaponId) == false)
        {
            var weaponData = DaniTechGameDataManager.Instance.GetWeaponData(weaponId);
            if (weaponData != null)
            {
                stat.Atk += weaponData.AtkBonus;
                stat.Int += weaponData.IntBonus;
            }
        }

        if(string.IsNullOrEmpty(armorId) == false)
        {
            var armorData = DaniTechGameDataManager.Instance.GetEqupmentData(armorId);
            if (armorData != null)
            {
                stat.Def += armorData.DefBonus;
                stat.Dex += armorData.DexBonus;
                stat.Luk += armorData.LukBonus;
            }
        }

        return stat;
    }
    
    private void RefreshStatPreview(string selectedDataid, EInventoryCategory category)
    {
        string curWeapon = DaniTechGameManager.Inst.GetEquippedWeaponId();
        string curArmor = DaniTechGameManager.Inst.GetEquippedArmorId();

        CalculatedStat current = CalculateStats(curWeapon, curArmor);

        string previewWeapon = curWeapon;
        string previewArmor = curArmor;

        if(category == EInventoryCategory.WeaponCategory)
        {
            previewWeapon = selectedDataid;
        }

        else if(category == EInventoryCategory.EqupmentCategory)
        {
            previewArmor = selectedDataid;
        }

        CalculatedStat preview = CalculateStats(previewWeapon, previewArmor);

        SetStatPreviewText(Text_Atk_Stat, Text_Atk_AfterStat, current.Atk, preview.Atk);
        SetStatPreviewText(Text_Int_Stat, Text_Int_AfterStat, current.Int, preview.Int);
        SetStatPreviewText(Text_Def_Stat, Text_Def_AfterStat, current.Def, preview.Def);
        SetStatPreviewText(Text_Dex_Stat, Text_Dex_AfterStat, current.Dex, preview.Dex);
        SetStatPreviewText(Text_Luk_Stat, Text_Luk_AfterStat, current.Luk, preview.Luk);

        //Text_Atk_Stat.text = $"{current.Atk}";
        //Text_Atk_AfterStat.text = $">      {preview.Atk}";
        //Text_Int_Stat.text = $"{current.Int}";
        //Text_Int_AfterStat.text = $">      {preview.Int}";
        //Text_Def_Stat.text = $"{current.Def}";
        //Text_Def_AfterStat.text = $">       {preview.Def}";
        //Text_Dex_Stat.text = $"{current.Dex}";
        //Text_Dex_AfterStat.text = $">       {preview.Dex}";
        //Text_Luk_Stat.text = $"{current.Luk}";
        //Text_Luk_AfterStat.text = $">       {preview.Luk}";
    }


    private void SetStatPreviewText(Text currentText, Text afterText, int current, int preview)
    {
        currentText.text = $"{current}";
        if (preview > current)
        {
            // afterText.gameObject.SetActive(true);
            afterText.text = $">      {preview}";
            afterText.color = Color.green;
        }
        else if (preview < current)
        {
            // afterText.gameObject.SetActive(true);
            afterText.text = $">      {preview}";
            afterText.color = Color.red;
        }
        else
        {
            afterText.text = "";
        }
    }
}