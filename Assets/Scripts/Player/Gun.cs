﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;          // 총의 이름
    public float range;             // 총의 사정거리
    public float accuracy;          // 정확도
    public float fireRate;          // 연사속도
    public float reloadTime;        // 재장전 속도

    public int damage;              // 총의 데미지

    public int reloadBulletCount;   // 총알의 재장전 개수
    public int currentBulletCount;  // 현재 남은 총알의 개수
    public int maxBulletCount;      // 최대 소유 가능 총알 개수
    public int carryBulletCount;    // 현재 소유 하고 있는 총알 개수

    public float retroActionForce;  // 반동 세기
    public float retroActionFineSightForce; // 정조준 시의 반동 세기

    public Vector3 fineSightOriginPos;  // 정조준시 위치

    public Animator anim;               // 총기 애니메이션
    public ParticleSystem muzzleFlash;  // 총구 화염 파티클

    public AudioClip fire_Sound;        // 격발시 소음
    public AudioClip reload_Sound;      // 재장전 시 효과음
}
