using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public Transform mainWeaponPos;
    public Transform[] subWeaponPos;
    [SerializeField]
    private List<GameObject> bullets;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            string mainWeaponName = bullets[0].name;
            GameObject bulletObj = gm.GetBullet(mainWeaponName);
            bulletObj.transform.position = mainWeaponPos.position;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            int count = subWeaponPos.Length;
            for (int i = 0; i < count; i++)
            {
                string subWeaponName = bullets[1].name;
                GameObject bulletObj = gm.GetBullet(subWeaponName);
                bulletObj.transform.position = subWeaponPos[i].position;
            }
        }
    }
}