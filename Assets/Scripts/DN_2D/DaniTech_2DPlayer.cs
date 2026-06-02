using System;
using System.Collections;
using UnityEngine;

// +) 어떤 컴포넌트가 필수로 필요하다는 것을 강제할 수 있다
[RequireComponent(typeof(Rigidbody2D))]
public class DaniTech_2DPlayer : MonoBehaviour
{
    [Header("플레이어 ID")]
    [SerializeField] private string _characterDataId;
    [SerializeField] private GameObject Caution_Root;

    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 8f;
    // [SerializeField] private float _jumpForce = 12f;

    [Header("지면 체크 설정")]
    [SerializeField] private Transform _groundCheck;    // 발 밑에 배치할 빈 오브젝트
    [SerializeField] private float _checkRadius = 0.5f; // 체크 범위
    [SerializeField] private LayerMask _groundLayer;    // 지면으로 인식할 레이어 (Platforms 등)

    [Header("애니메이터")]
    [SerializeField] private Eilie_AnimatorController AnimatorController_Entity;

    [Header("스킬")]
    [SerializeField] private Collider2D Collider_PlayerNormalAttack;
    [SerializeField] private GameObject Prefab_SkillProfectile;
    [SerializeField] private Transform Transform_SkillProfectileRoot;

    [Header("전투 관련 정보")]
    [SerializeField] private int _maxHp = 1000;

    [SerializeField] private int _playerHp = 1000;
    [SerializeField] private int _playerBaseAtk = 100;

    


    // 우선 직접 들고 있다가 추후에 UI매니저한테 요청하도록 개선해볼 것
    [SerializeField] private DaniTech_ScoreUI _scoreUI;

    private Rigidbody2D _rigidBody;
    private bool _isGrounded;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _lookRight = true;
    private bool _isSkillUsing = false;

    

    // 추후에는 이런 데이터가 저장될 수 있도록 UI에 있는 것보다 한곳으로 모여지는게 좋다
    private int _currentScore;

    // 스킬 관련 ==========================================================================
    //플레이어가 바라보고 있는 방향
    private Vector2 _lookDirection = Vector2.right;
    public enum ViewType { sideView, TopDown, Isometric}
    public ViewType _currentView;

    private event Action<int, int> _onHpChanged;
    private event Action<int, int> _onMpChanged;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        // 2D 캐릭터가 물리 충돌 시 회전해서 넘어지는 것 방지
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Collider_PlayerNormalAttack.gameObject.SetActive(false);

        _playerHp = 1000;
        _maxHp = 1000;
    }

    private void Start()
    {
       // 나 스스롤를 등록 > 씬에 있는 그 2D플레이어가 등록됨
        DaniTechGameObjectManager.Inst.RegisterLocalPlayer(this);
        // DaniTechUIManager.Instance.AddHudSlot(-1, this.gameObject.transform);
    }

    void Update()
    {
        // 1. 입력 받기 (Update에서 수행)
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        // 2. 점프 입력
        //if (Input.GetKeyDown(KeyCode.X) && _isGrounded)
        //{
        //    //Debug.LogWarning("점프 입력받음");
        //    //Debug.LogWarning(AnimatorController_Entity  == null ? "animatorController가 null!" : "animatorController 정상" );

        //    Jump();
        //    return;
        //}

        // 3. 캐릭터 방향 전환 (Flip)
        if (_horizontalInput > 0 && !_lookRight)
        {
            Flip();
        }
        else if (_horizontalInput < 0 && _lookRight) 
        { 
            Flip(); 
        }

        // 이동을 한다라는 판정만 우선 해봅시다
        bool isMoving = ((_horizontalInput != 0) || (_verticalInput != 0));
        ChangePlayerState(isMoving ? EilieAnimState.Walk : EilieAnimState.Idle);

        AnimatorController_Entity.SetMoveDirection(new Vector2(_horizontalInput, _verticalInput));

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Atk();
        //}

        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //    UseNormalAttack();
        //}

    }

    private void ChangePlayerState(EilieAnimState newState)
    {
        // 이런 곳에 UI나 플레이어의 별도 처리를 넣어줄 수도 있다


        // 우선 애니메이션만 바꿔 봅시다
        AnimatorController_Entity.SetState(newState);
    }

    void FixedUpdate()
    {
        // 4. 지면 체크 (물리 연산 전 수행)
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayer);

        // 5. 좌우 이동 처리
        Move();
    }

    void Move()
    {
        // Y축 속도는 유지하면서 X축 속도만 변경 (관성 유지)
        _rigidBody.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _verticalInput * _moveSpeed);
    }

    //void Jump()
    //{
    //    ChangePlayerState(EilieAnimState.Jump);
    //    // 순간적인 힘을 위로 가함
    //    _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, _jumpForce);
    //}
    //void Atk()
    //{
    //    ChangePlayerState(EilieAnimState.Atk);

    //}
    void Flip()
    {
        _lookRight = !_lookRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // 에디터 뷰에서 지면 체크 범위를 시각적으로 확인
    private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _checkRadius);
        }
    }

    // 6) 적 충돌 시 처리를 해보자
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // 6-1) 플레이어의 > 콜리전에 충돌한 객체가 어떤 Tag인지 1차 검사한다.
    //        // 지면 같은 오브젝트와 점프시 충돌이 계속 오므로 이렇게 태그로 먼저 비교하는게 좋다
    //        // 중단점을 찍어보면서 확인 추천
    //    if (collision.gameObject.CompareTag("Enemy") == false)
    //    {
    //        return;
    //    }

    //    // 6-2) 충돌한 몬스터의 정보를 받아오려고 시도해보자
    //    var enemyComponent = collision.gameObject.GetComponent<DaniTech_2DEnemy>();
    //    if (enemyComponent == null)
    //    {
    //        Debug.Log($"충돌한 적 객체에서 컴포넌트를 찾을 수 없습니다 : {gameObject.name}");
    //        return;
    //    }

    //    // 6-3) 충돌된 오브젝트를 플레이어가 직접 제거하는게 아니라, Id로 게임오브젝트매니저한테 삭제를 요청한다
    //    DaniTechGameObjectManager.Inst.RequestDestroyEntityObject(enemyComponent.EntityInstancId);

    //    // 6-4) 피그미를 잡으면 스코어를 올려주자!
    //    AddGameScore();
    //}

    private void AddGameScore()
    {
        // 7) 여기서 맥락 -> UI를 갱신해주기 위해 과연 플레이어가 이렇게 UI를 직접
            // 알고 있는게 좋은걸까?

        _currentScore++;
        if(_scoreUI != null)
        {
            _scoreUI.AddGameScore(_currentScore);

        }

        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //if(collision.CompareTag("Fire"))
    //    //{
    //    //    ChangePlayerState(EilieAnimState.Hit);
    //    //}

    //    //if(collision.CompareTag("Apple"))
    //    //{
    //    //    Debug.LogWarning("사과를 1개 획득했습니다");
    //    //    CollectApple(collision.gameObject);
    //    //}
    //}

    private void CollectApple(GameObject apple)
    {
        // DaniTechGameObjectManager.Inst.RequestDestroyEntityObject(apple.GetComponent<DaniTech_2DFieldObject>().EntityInstanceId);

        var fieldObject = apple.GetComponent<DaniTech_2DFieldObject>();
        if (fieldObject == null) return;

        DaniTechGameObjectManager.Inst.RequestDestroyFieldObject(fieldObject.FieldObjectInstanceId);
        
        AddGameScore();
    }

    private void GameClear()
    {
        Debug.LogWarning("게임 클리어");
        // Todo 로그 대신 클리어 화면 UIManager에 요청해보기
    }

    //public bool CheckSkillUseable(bool isShowMsg = true)
    //{
    //    //원하는 스킬의 사용가능 조건 추가 Ex) 후딜레이
    //    if (_isSkillUsing == true)
    //    {
    //        if (isShowMsg == true)
    //        {
    //            DaniTechUIManager.Instance.OpenSimplePopup("스킬이 이미 사용중입니다.");

    //        }
    //        return false; 
    //    }

    //    return true;
    //}

    //public void UseNormalAttack()
    //{
    //    if(CheckSkillUseable(isShowMsg:false) == false) return;
        
    //    Collider_PlayerNormalAttack.gameObject.SetActive(true);
    //    StartCoroutine(CoStartNormalAttack());

    //    // ChangePlayerState(); 플레이어의 공격모션 변경
    //}

    //public void UseFirstlSkill()
    //{
    //    if (CheckSkillUseable() == false) return;

    //}

    //public void UseSecondSkill()
    //{
    //    if (CheckSkillUseable() == false) return;

    //}

    //public void UseThirdSkill()
    //{
    //    if (CheckSkillUseable() == false) return;
    //    CreateProjectileSkillObject();
    //}

    //private void CreateProjectileSkillObject()
    //{
    //    var gObj = Instantiate(Prefab_SkillProfectile);
    //    if (gObj == null) return;

    //    var skillProjectileComponent = gObj.GetComponent<SkillProjectile>();
    //    if(skillProjectileComponent == null) return;

    //    skillProjectileComponent.InitSkillObject(0, _lookRight, this.transform.position, 50, tag, OnSkillCollision);
    //}

    IEnumerator CoStartNormalAttack()
    {
        yield return new WaitForSeconds(1.0f);
        Collider_PlayerNormalAttack.gameObject.SetActive(false);

    }

    // 플레이어의 전투와 관련된 부분은 추후 다른곳으로 빠질수 있음
    // 데이터의 와리가리 하는 부분은 > 세이브 > GameManager
    // 인스턴스 데이터가 플레이어 코드안에 있는게 아니라 저장이 가능하도록 GameManager에 플레이어 인스턴스 데이터로 이동해야함
    // PlayerViewModel
    public void TakeDamage(int damage)
    {
       
        _playerHp -= damage;

        if (_playerHp <= 0)
        {
            // 죽음처리
            PlayerDie();
        }
    }

    private void OnSkillCollision(int collidedObjectInstanceId, int damage)
    {

    }

    public void BindOnStatChangedEvent(Action<int, int> hpChangeCallback, Action<int, int> mpChangeCallback)
    {
        _onHpChanged += hpChangeCallback;
        _onMpChanged += mpChangeCallback;
    }

    public void ResetBindStatChangedEvent()
    {
        _onHpChanged = null;
        _onMpChanged = null; 
    }

    private void InvokeStatChangedEvent()
    {
        _onHpChanged?.Invoke(_playerHp, _maxHp);
        // _onMpChanged?.Invoke(_playerMp);
    }

    // 이건 언제 왜 생긴거지?
    //private void DaniTech_2DPlayer__onMpChanged(int obj)
    //{
    //    throw new NotImplementedException();
    //}

    public void PlayerDie()
    {
        // bool _isAlive = false;
    }

    public void EnemyReset()
    {
        
    }

    public string GetCharacterDataId()
    {
        return _characterDataId;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == false) return;
        Caution_Root.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Caution_Root.gameObject.SetActive(false);

    }

}
