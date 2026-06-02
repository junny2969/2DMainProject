using System;
using System.Collections.Generic;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}


[System.Serializable]

public class BattleUnitDataBase : GameDataBase
{
    public string Name;
    public int MaxHp;
    public int MaxMp;
    public int Atk;
    public string PrefabKey;
    public string SpritePath;
    public List<int> SkillIdList;
    public string IconPath;
}
// C# 때와 약간 달라진 점
    // Syste.Text.Json대신 유니티 내장 JsonUtility를 사용
    // 따라서 프로퍼티말고 그냥 일반 public 멤버변수로 변경함
    // [System.Serializable]가 없다면 JsonUtility는 데이터를 무시

[System.Serializable]
public class DNCharacterData : BattleUnitDataBase
{
    public string BasicCostumeId;
    public string SkillList;
    public string UseWeaponId;

    // public int Id;
    //public string Name;
    //public int MaxHp;
    //public int MaxMp;
    //public int Atk;
    //public string PrefabKey;
    //public string SpritePath;
    //public List<int> SkillIdList;
}

[System.Serializable]
public class DNSkillData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    // public string MotionPath;
    public int Damage;
    public int CostMp;
    public string SkillType;
    public string AnimTrigger;
}

[System.Serializable]
public class DNWeaponData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
}

[System.Serializable] 
public class DNCostumeData : GameDataBase
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class DNItemData : GameDataBase
{
    public string Name;
    public string Description;
    public string ItemType;
    public string Grade;
    public string MaxStackCount;
    public string SellingPrice;
    public string IconPath;
    public string UseItemType;
    public List<string> UseItemParameterList; // 특수한 제약 조건이 있긴 하다
}

[System.Serializable]
public class DNDialogueGroupData : GameDataBase
{
    public List<string> DialogueIdList;
}

[System.Serializable]
public class DNDialogueData : GameDataBase
{
    public string CharacterDataId;
    public string Description;
    public string NextDialogueId;
    public List<string> SelectionNameList;
    public List<string> SelectionDialogueIdList;
    public string TexturePath;
    public string VoicePath;
}

[System.Serializable]
public class DNFieldObjectData : GameDataBase
{
    public string Name;
    public string Description;
    public string FieldObjectType;
    public List<int> DropCountRange;
    public string DropItemDataId;
    public string IconPath;
    public string PrefabPath;
}

[System.Serializable]
public class DNMonsterData : BattleUnitDataBase
{
    //public string Name;
    public string Description;
    //public int BaseHP;
    //public int BaseAtk;
    public float NormalAtkMultiple;
    public List<float> SkillAtkMultipleList;
    public string PrefabPath;
    public string FieldSpritePath;
    //public int MaxHp;
    //public int MaxMp;
}
[System.Serializable]

public class PotionData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;

}
[System.Serializable]

public class EqupmentData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
}