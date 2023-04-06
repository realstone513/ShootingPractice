using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAI : EnemyAI
{
    protected int phase = 0;
    public List<Weapon> otherWeapons;

    protected override void OnEnable()

    {
        base.OnEnable();
        subWeaponDelayTimer = 0f;
        StartCoroutine(CoArrangeBoss());
    }

    protected override void Update()
    {
        base.Update();
        if (phase == 0 && HPRatio < 0.5f)
            ChangePhase(1);
    }

    protected virtual void ChangePhase(int nextPhase)
    {
        phase = nextPhase;
    }

    private IEnumerator CoArrangeBoss()
    {
        Vector2 initPos = GameManager.Instance.bossInitPos;
        while (gameObject.transform.position.y > initPos.y)
        {
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
            yield return null;
        }
    }
}