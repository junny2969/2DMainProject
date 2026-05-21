using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMonster : MonsterBase
{
    [Header("몬스터 프리팹에서 미리 세팅할 데이터")]
    public float SkillCoolTime;
    public GameObject Prefab_ThisMonsterSkillObject;
    [SerializeField] private SpriteRenderer SpriteRenderer_Monster;



    [Header("데이터 확인용 임시")]
    public int _instanceId; // 게임에서 태어날때 부여된 고유번호 (중복불가) > 게임 오브젝트 매니저에서 찾기용
    public string _dataId; // 내가 누구인지 나중에 찾을수 있는 호출번호 (중복가능??) > 데이터 드리븐용 (아이디를 통해 부가데이터 찾기)


    // DNMonsterData _thisMonsterData;
    [Header("받아왔는데 전투에서 필요한 데이터")]
    private DNMonsterData _thisMonsterData;
    public int _baseHp;
    public int _baseAtk;
    public bool _isAlive = true;
    private bool _lookRight = true;

    private Vector3 _moveDirection;


    private void OnDisable()
    {
        _isAlive = false;
    }

    // 태어난 시점에서 어떤 정보를 저장해주자
    public void InitMonster(int instanceId, string dataId)
    {
        _instanceId = instanceId;
        _dataId = dataId;

        // 초기화 한 다음에 그 객체가 가지고 있는 데이터를 찾아와서 필요한 세팅을 해준다
        var monsterData = DaniTechGameDataManager.Instance.GetDNMonsterData(dataId);
        if (monsterData != null)
        {
            // 이 몬스터가 생성된 시점에서 자신의 엑셀에서 받아온 json을 거친 데이터를 캐싱해둔다(보관)
            _thisMonsterData = monsterData;
            _baseHp = _thisMonsterData.BaseHP;
            _baseAtk = _thisMonsterData.BaseAtk;
        }

        StartCoroutine(CheckAndUseSkill());
    }

    private int GetFinalNormalAktDamage(int baseAtk, float normalAtkMultiple)
    {
        return GetFinalSkillDamage(baseAtk, normalAtkMultiple);
    }

    private int GetFinalSkillDamage(int baseAtk, float skillMultiple)
    {
        return (int)(baseAtk * skillMultiple);
    }

    // 코루틴이 등장한다는건 => 유니테스크로 호환이 가능한다
    // 일정 시간마다 스킬을 사용할 예정
    // 스타트 코루틴은 이 몬스터가 생성된 시점에서 돌아도 됨
    IEnumerator CheckAndUseSkill()
    {
        while (_isAlive)
        {
            yield return new WaitForSeconds(SkillCoolTime);

            if (_isAlive == false)
            {
                break;
            }

            ChangeMonsterDirection();
            UseSkill();
        }
    }

    void ChangeMonsterDirection()
    {
        _lookRight = !_lookRight;
        _moveDirection = new Vector3(_lookRight ? 1 : -1, 0, 0);
        SetMeshDirectionByMoveDirection((int)_moveDirection.x);
    }
    void SetMeshDirectionByMoveDirection(int x)
    {
        // + 디테일을 살리기 위해 방향에 따라 캐릭터 리소스를 뒤집는다
        // 역시 중요한 로직은 아니다!
        SpriteRenderer_Monster.flipX = (x < 0);
    }

    private void UseSkill()
    {
        var gObj = Instantiate(Prefab_ThisMonsterSkillObject, DaniTechGameObjectManager.Inst.transform);
        if (gObj == null) return;

        var skillProjectileComponent = gObj.GetComponent<SkillProjectile>();
        if (skillProjectileComponent == null) return;


        // 확인필요 

        float skillMultiple = _thisMonsterData.SkillAtkMultipleList.Count > 0 ? _thisMonsterData.SkillAtkMultipleList[0] : 0;
        float finalSkillDamage = GetFinalSkillDamage(_baseAtk, skillMultiple);
        skillProjectileComponent.InitSkillObject(_instanceId, _lookRight, this.transform.position, 50);
    }
}
