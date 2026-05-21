using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkillProjectile : SkillBase
{
    [SerializeField] private SpriteRenderer SpriteRenderer_Effect;
    [SerializeField] private float ProjectileSpeed = 5.0f;

    private int _damage;
    private int _ownerInstanceId; // 나를 소환한 주인의 Id

    private Vector3 _moveDirection = new Vector3(1, 0, 0); // 사이드뷰 기준으로는 x가 -1,1 좌우 / 탑뷰,아이소매트릭이면 y연산 필요

    public void InitSkillObject(int ownerInstanceId, bool isDirRight, Vector3 playerPos, int damage)
    {
        this.transform.position = playerPos;
        _moveDirection = isDirRight ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
        // SpriteRenderer_Effect.flipX = isDirRight;
        _damage = damage;
        _ownerInstanceId = ownerInstanceId;
    }
    private void Update()
    {
        this.gameObject.transform.position += _moveDirection * ProjectileSpeed * Time.deltaTime;
    }

}
