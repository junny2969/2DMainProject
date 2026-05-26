using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel
{
    public int InstanceId { get; private set; }
    public string DataId { get; private set; }
    public BattleUnitDataBase Data { get; private set; }

    public int CurrentHp { get; private set; }
    public int CurrentMp { get; private set; }

    public bool IsAlive
    {
        get { return CurrentHp > 0; }
    }

    public event Action<int, int> OnHpChanged;
    public event Action OnDead;

    public UnitModel(int instanceId, BattleUnitDataBase data)
    {
        InstanceId = instanceId;
        DataId = data.Id;
        Data = data;
        CurrentHp = data.MaxHp;
        CurrentMp = data.MaxMp;
    }

    public void TakeDmage(int damage)
    {
        
    }
}
