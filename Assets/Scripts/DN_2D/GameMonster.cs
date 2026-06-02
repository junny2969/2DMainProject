using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMonster : MonsterBase
{
    [Header("몬스터 프리팹에서 미리 세팅할 데이터")]
    [SerializeField] private SpriteRenderer SpriteRenderer_Monster;
    [SerializeField] private GameObject Caution_Root;


    [Header("데이터 확인용 임시")]
    public int _instanceId; // 게임에서 태어날때 부여된 고유번호 (중복불가) > 게임 오브젝트 매니저에서 찾기용
    public string _dataId; // 내가 누구인지 나중에 찾을수 있는 호출번호 (중복가능??) > 데이터 드리븐용 (아이디를 통해 부가데이터 찾기)
    public bool _isAlive;

    public int GetInstanceId() { return _instanceId; }
    public string GetDataId() { return _dataId; }
    public bool GetIsAlive() { return _isAlive; }


    public async void InitMonster(int instanceId, string dataId)
    {
        _instanceId = instanceId;
        _dataId = dataId;
        _isAlive = true;

        if(Caution_Root != null)
        {
            Caution_Root.SetActive(false);
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isAlive == false)
        {
            return;
        }

        if(collision.gameObject.CompareTag("player") == false)
        {
            return;
        }

        //TODO 다이얼로그 띄우기, 응답후 전투 진입
        BattleManager.Inst.EnterBattleFromField(_dataId);


        //if (collision.gameObject.CompareTag("Player") == false) return;

        //var player = collision.gameObject.GetComponent<DaniTech_2DPlayer>();
        //if (player == null) return;

        //var playerList = new List<string>();
        //playerList.Add(player.GetCharacterDataId());

        //var monsterList = new List<string>();
        //monsterList.Add(_dataId);

        //BattleManager.Inst.EnterBattle(playerList, monsterList).Forget();
    }

    
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;
        if (Caution_Root != null)
        {
            Caution_Root.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;
        if (Caution_Root != null)
        {
            Caution_Root.SetActive(false);
        }

    }
}
