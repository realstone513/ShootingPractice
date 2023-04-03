using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public Transform mainWeaponPos;
    public Transform[] subWeaponPos;
    public List<GameObject> bullets;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(bullets[0], mainWeaponPos.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(bullets[1], subWeaponPos[0].position, Quaternion.identity);
            Instantiate(bullets[1], subWeaponPos[1].position, Quaternion.identity);
        }
    }
}