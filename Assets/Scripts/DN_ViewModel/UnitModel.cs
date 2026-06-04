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
    public event Action<int, int> OnMpChanged;
    public event Action OnDead;

    public UnitModel(int instanceId, BattleUnitDataBase data)
    {
        InstanceId = instanceId;
        DataId = data.Id;
        Data = data;
        CurrentHp = data.MaxHp;
        CurrentMp = data.MaxMp;
    }

    public void TakeDamage(int damage)
    {
        if (IsAlive == false) return;

        CurrentHp = CurrentHp - damage;
        if(CurrentHp <= 0)
        {

            CurrentHp = 0;
            OnDead?.Invoke();
        }

        OnHpChanged?.Invoke(CurrentHp, Data.MaxHp);
    }

    public void TakeMp(int consumeMp)
    {
        CurrentMp = CurrentMp - consumeMp;
        if (CurrentMp <= 0)
        {
            CurrentMp = 0;
        }

        OnMpChanged?.Invoke(CurrentMp, Data.MaxMp);
    }

    public void RestoreCurrentStat(int hp, int mp)
    {
        CurrentHp = hp;
        CurrentMp = mp;
    }

}
