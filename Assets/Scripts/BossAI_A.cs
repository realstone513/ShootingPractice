using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_A : BossAI
{
    protected List<Vector2> customDirections = new ();

    private void Start()
    {
        customDirections.Add(Vector2.down);
        customDirections.Add(new Vector2(-1, 1).normalized);
        customDirections.Add(new Vector2(1, 1).normalized);
    }

    protected override void ChangePhase(int nextPhase)
    {
        phase = nextPhase;
        mainWeapon = otherWeapons[0];
    }

    public override void ShootMainWeapon()
    {
        if (phase == 0)
            base.ShootMainWeapon();
        else if (phase == 1)
        {
            if (mainWeapon == null)
                return;

            mainWeaponDelayTimer += Time.deltaTime;
            if (mainWeaponDelayTimer > mainWeapon.reloadDelay)
            {
                mainWeaponDelayTimer = 0f;
                //if (mainWeapon.magazineSize == 1)
                //    MainSingleShot();
                //else
                StartCoroutine(CoMainOpenFire());
            }
        }
    }


    protected override IEnumerator CoMainOpenFire()
    {
        int count = mainWeapon.magazineSize;
        WaitForSeconds wfs = new(mainWeapon.fireRate);
        for (int i = 0; i < count; i++)
        {
            MainSingleShot(customDirections[i], true, false);
            yield return mainWeapon.fireRate == 0 ? null : wfs;
        }
    }

    //public override void ShootSubWeapon()
    //{
    //    base.ShootSubWeapon();
    //}
}