using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class DaniTechGameObjectManager : MonoBehaviour
{
    // 생성할 몬스터의 프리팹
    [SerializeField] private GameObject Prefab_Enemy;
    [SerializeField] private Transform Root_Enemy;

    [SerializeField] private GameObject Prefab_DamageText;
    [SerializeField] private GameObject Prefab_FieldMonster;


    [Header("전투 유닛 정보")]
    [SerializeField] private GameObject Prefab_BattlePlayer;
    [SerializeField] private GameObject Prefab_BattleMonster;

    [SerializeField] private Transform Root_BattlePlayer;
    [SerializeField] private Transform Root_BattleMonster;




    public static DaniTechGameObjectManager Inst { get; set; }

    // 생성된 오브젝트의 키가 됨
    private int _objectInstanceKeyGenerator = 10;

    // 생성된 오브젝트의 생명을 보관
    private Dictionary<int, GameObject> _createdGameObjectContainer = new Dictionary<int, GameObject>();
    private Dictionary<int, DaniTech_2DFieldObject> _fieldObjectContainer = new Dictionary<int, DaniTech_2DFieldObject>();
    private Dictionary<int, GameMonster> _monsterObjectContainer = new Dictionary<int, GameMonster>();

    // 게임 오브젝트 매니저가 살아있는 동안 이 플레이어를 보관(캐싱)해둔다
    private DaniTech_2DPlayer _localPlayer;

    private void Awake()
    {
        Inst = this;
    }

    // 등록과 가져오기
    public void RegisterLocalPlayer(DaniTech_2DPlayer localPlayer)
    {
        _localPlayer = localPlayer;
    }

    public DaniTech_2DPlayer GetLocalPlayer()
    {
        // 없다면 로그를 미리 찍어줄수도 있고 Find
        // 우리가 배웠던 원시적인 Get함수 > 원시적이지만 유용함 // 로그찍기 등
        if( _localPlayer == null )
        {
            Debug.LogError("등록된 플레이어가 없는데 참조하려고 시도하고 있음");
            return null;
        }
        return _localPlayer;
    }
    public void RequestSpawnEnemy()
    {
        if(Prefab_Enemy == null)
        {
            Debug.LogWarning("프리팹이 등록되지 않은 오브젝트 입니다.");
            return;
        }

        var gObj = Instantiate(Prefab_Enemy, Root_Enemy);
        if(gObj == null)
        {
            Debug.LogWarning("생성에 실패한 게임 오브젝트 입니다.");
            return;
        }

        // 1-1 생성에 성공했다면, 미리 Key를 발급한다.
        _objectInstanceKeyGenerator++;

        // 1-2 Dictionary에 추가하기 전에 미리 키 검사한다
        if (_createdGameObjectContainer.ContainsKey(_objectInstanceKeyGenerator) == true)
        {
            Debug.LogWarning("이미 동일한 키가 발급된 게임 오브젝트가 존재합니다");
            return;
        }

        // 1-3 동적생성(실체화)된 오브젝트를 게임 오브젝트 매니저의 자료구조(Dictionary)에 보관하자!
        _createdGameObjectContainer.Add(_objectInstanceKeyGenerator, gObj);
        InitGeneratedEntityObject(_objectInstanceKeyGenerator, gObj);

        Debug.Log($"키: {_objectInstanceKeyGenerator}의 객체 {gObj.name}이 호출되었습니다.");
    }

    private void InitGeneratedEntityObject(int generatedId, GameObject gObj)
    {
        // 4-1 지금은 Enemy지만, 나중에 IGameEntity 같은 인터페이스로 개선하면 더 좋다
        DaniTech_2DEnemy gameEntity = gObj.GetComponent<DaniTech_2DEnemy>();
        if(gameEntity == null)
        {
            Debug.LogWarning($"생성된 {gObj.name}의 InstanceId를 대입할 수 있는 컴포넌트를 가져올 수 없습니다!");
            return;
        }

        // 4-2 생성된 객체에 정보를 부여한다!
        gameEntity.InitEnemyInfo(generatedId);
    }


    public GameObject GetEntityObjectCanBeNull(int instanceId)
    {
        if(_createdGameObjectContainer.ContainsKey(instanceId) == false)
        {
            Debug.LogWarning($"{instanceId}는 존재하지 않습니다.");
            return null;
        }

        // 2-1 실체화하면서 등록된 게임 오브젝트가 있다면 반환
        return _createdGameObjectContainer[instanceId];
    } 

    public void RequestDestroyEntityObject(int instanceId)
    {
        var gObj = GetEntityObjectCanBeNull(instanceId);
        if(gObj == null)
        {
            return;
        }

        // 3-1 요청된 객체를 제거함
        _createdGameObjectContainer.Remove(instanceId);
        Destroy(gObj);
    }

    //[몬스터 오브젝트] ====================================================================================================
    public void CreateMonsterObject(string monsterDataId, Transform spawnSpot)
    {
        // 만드려는 몬스터 정보 받아오기
        var monsterData = DaniTechGameDataManager.Instance.GetDNMonsterData(monsterDataId);
        if (monsterData == null) return;

        if(Prefab_FieldMonster == null)
        {
            Debug.LogWarning("Prefab_FieldMonster 등록안됨");
            return;
        }

        // 비동기라 조금 어려우므로 일단 따라치기
        var createdObj = Instantiate(Prefab_FieldMonster, Root_Enemy);
        createdObj.transform.position = spawnSpot.position; // 위치를 스폰스팟의 위치로 조정

        AddMonsterObjectOnCreate(createdObj, monsterDataId);
    }

    private void AddMonsterObjectOnCreate(GameObject createdObject, string monsterDataId)
    {
        _objectInstanceKeyGenerator++;
        int generatedInstanceId = _objectInstanceKeyGenerator;

        // 생성된 애는 게임오브젝트이기 때문에 MonsterBase <- GameMonster로 상속구조 되어있음
        var monsterComponent = createdObject.GetComponent<GameMonster>();
        if (monsterComponent == null) return;

        // 생성이 되었고 컴포넌트도 잘 가져왔다면 보관을 해야한다
        _monsterObjectContainer.Add(generatedInstanceId, monsterComponent);

        // UI든 필드 오브젝트든 몬스터든 만들어지는 시점에서 Init(생성자처럼 정보를 세팅해주는 함수는 거의 자주 보게된다
        monsterComponent.InitMonster(generatedInstanceId, monsterDataId).Forget();
    }




    //[필드 오브젝트] ====================================================================================================

    public async UniTaskVoid CreateFieldObject(string fieldObjectDataId, Transform spawnSpot)
    {
        var fieldObject = DaniTechGameDataManager.Instance.GetDNFieldObjectData(fieldObjectDataId);
        if (fieldObject != null)
        {
            var createdObj = await DaniTechResourceManager.Inst.InstantiateAsync(fieldObject.PrefabPath, Root_Enemy, true);
            createdObj.transform.position = spawnSpot.position;
            AddFieldObjectOnCreate(createdObj, fieldObjectDataId);
        }
    }

    private void AddFieldObjectOnCreate(GameObject createdObject, string fieldObjectDataId)
    {
        _objectInstanceKeyGenerator++;
        var generatedInstanceId = _objectInstanceKeyGenerator;
        var fieldObject = createdObject.GetComponent<DaniTech_2DFieldObject>();

        if(fieldObject != null)
        {
            _fieldObjectContainer.Add(generatedInstanceId, fieldObject);
            fieldObject.InitFieldObjectInfoOnCreated(generatedInstanceId, fieldObjectDataId);
        }
    }

    public void RequestDestroyFieldObject(int instanceId)
    {
        var fieldObjectComponent = GetFieldObjectByInstanceId(instanceId);
        if (fieldObjectComponent == null)
        {
            return;
        }

        // 요청된 필드 오브젝트를 제거함
        _fieldObjectContainer.Remove(instanceId);
        Destroy(fieldObjectComponent.gameObject);
    }

    public DaniTech_2DFieldObject GetFieldObjectByInstanceId(int fieldObjectInstanceId)
    {
        if(_fieldObjectContainer.ContainsKey(fieldObjectInstanceId) == false)
        {
            Debug.LogError($"{fieldObjectInstanceId} 찾으려는 필드 오브젝트가 유효하지 않습니다");
            return null;
        }

        return _fieldObjectContainer[fieldObjectInstanceId];
    } 

    public int GenerateInstanceId()
    {
        return ++_objectInstanceKeyGenerator;
    }
    
    public void RequestInitBattleUnit(int instanceId, UnitModel unitModel, GameObject prefabUnit, Transform spwanRoot)
    {
        var unit = Instantiate(prefabUnit, spwanRoot);
        var battleUnit = unit.GetComponent<BattleUnitView>();

        if (battleUnit != null)
        {
            battleUnit.InitBattleUnit(unitModel);
            _createdGameObjectContainer.Add(instanceId, unit);
        }
    }

    public BattleUnitView GetBattleUnitView(int instanceId)
    {
        if(_createdGameObjectContainer.ContainsKey(instanceId) == false)
        {
            Debug.LogWarning($"{instanceId} 에 해당하는 유닛 없음");
            return null;
        }

        var gObj = _createdGameObjectContainer[instanceId];
        return gObj.GetComponent<BattleUnitView>();
    }

    public void HideLocalPlayer()
    {
        _localPlayer.gameObject.SetActive(false);
    }

    public void ReSpawnLocalPlayer()
    {
        _localPlayer.gameObject.SetActive(true);
    }

    public void SpawnDamageText(int damage, Vector3 position)
    {
        if(Prefab_DamageText == null)
        {
            Debug.LogWarning("Prefab_Damage가 등록되지 않았습니다");
            return;
        }

        Vector3 spawnPosition = position + new Vector3(0f, 500f, 0f);

        var gObj = Instantiate(Prefab_DamageText);
        var damageText = gObj.GetComponent<DamageTextController>();
        if(damageText != null)
        {
            damageText.PlayDamageText(damage, spawnPosition).Forget();
        }

    }
}
