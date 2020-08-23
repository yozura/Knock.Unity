using UnityEngine;

public class FollowZombie : Zombie
{

    protected void Hit(Vector3 _targetPos)
    {
        direction = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;
        currentTime = runTime;
        applySpeed = runSpeed;
        isWalking = false;
        isRunning = false;
    }

    protected void Run()
    {
        isRunning = true;
        currentTime = runTime;
        applySpeed = runSpeed;
        Debug.Log("Run");
    }

    public override void Damaged(int _dmg, Vector3 _targetPos)
    {
        base.Damaged(_dmg, _targetPos);
        if (!isDead)
            Hit(_targetPos);
    }
}