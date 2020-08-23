using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    CharacterStatus status;

    public class AttackInfo
    {
        public int attackPower;     // 공격력
        public Transform attacker;  // 공격자
    }

    void Start()
    {
        status = transform.root.GetComponent<CharacterStatus>();
    }

    // 공격 정보를 가져온다
    AttackInfo GetAttackInfo()
    {
        AttackInfo attackInfo = new AttackInfo();
        // 공격력 계산
        attackInfo.attackPower = status.Power;

        // 공격력 강화 중
        if (status.powerBoost)
            attackInfo.attackPower += attackInfo.attackPower;

        attackInfo.attacker = transform.root;

        return attackInfo;
    }

    // 히트
    void OnTriggerEnter(Collider other)
    {
        // 공격 당한 상대의 Damage 메시지를 보낸다.
        other.SendMessage("Damage", GetAttackInfo());

        // 공격한 대상을 저장한다
        status.lastAttackTarget = other.transform.root.gameObject;
    }

    // 공격 판정을 유효로 한다.
    void OnAttack()
    {
        GetComponent<Collider>().enabled = true;
    }

    // 공격 판정을 무효로 한다
    void OnAttackTermination()
    {
        GetComponent<Collider>().enabled = false;
    }

}
