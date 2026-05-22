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


    // 충돌 했을때 그 충돌한 대상의 ID를 부모에게 이르는 델리게이트
    // 구독부분과 발생 부분이 있다
    private event Action<int, int> _onSkillCollision;

    private void OnDisable()
    {
        _onSkillCollision = null;
    }

    public void InitSkillObject(int ownerInstanceId, bool isDirRight, Vector3 playerPos, int damage, string parentTag, Action<int, int> onSkillCollision) 
    {
        this.transform.position = playerPos;
        _moveDirection = isDirRight ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
        // SpriteRenderer_Effect.flipX = isDirRight;
        _damage = damage;
        _ownerInstanceId = ownerInstanceId;

        //콜백이라 그냥 1:1로 구독 +=도 가능
        _onSkillCollision = onSkillCollision;

        // 소환자의 Tag정보를 기입
        this.gameObject.tag = parentTag;
    }
    private void Update()
    {
        this.gameObject.transform.position += _moveDirection * ProjectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.collider);
    }

    private void CheckCollision(Collider2D collision)
    {
        bool isOwnerPlayer = (_ownerInstanceId == 0);

        if(collision.CompareTag("Player") == (isOwnerPlayer == false)) // Owner가 0이면 무조건 플레이어다
        {
            // 1번 방식 = 플레이어에게 직접 투사체가 데미지를 줘씀
            // 플레이어라면 직접 플레이어에게 투사체가 데미지를 부여
            //var player = DaniTechGameObjectManager.Inst.GetLocalPlayer();
            //player.TakeDamage(_damage);

            // 2번 방식 - 투사체가 직접 데미지를 주는게 아니라 부모에게 충돌체의 ID를 이름
            _onSkillCollision.Invoke(0, _damage); // 0? > LocalPlayer는 0번이니까 그냥 하드코딩

            // 스킬은 오브젝트 매니저를 통해서 만들어지지는 않았ㅇ으므로 직접 스스로 제거
            // 몬스터 > 오브젝트 매니저를 통해서 제거(UI매니저와 동일한 프로세스)
            // 스킬은 직접 스스로 제거

            Destroy(this.gameObject);
        }
    }
}
