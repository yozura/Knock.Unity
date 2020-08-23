using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleZombie : Zombie
{
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 2);

        if (_random == 0)        // idle
        {
            IDLE();
        }
        else if (_random == 1)   // walk;
        {
            Walk();
        }
    }

    private void IDLE()
    {
        currentTime = waitTime;
    }

    private void Walk()
    {
        isWalking = true;
        currentTime = walkTime;
        applySpeed = walkSpeed;
        zombieAni.SetBool("Walk", isWalking);
    }

    public override void Damaged(int _dmg, Vector3 _targetPos)
    {
        base.Damaged(_dmg, _targetPos);
    }
}
