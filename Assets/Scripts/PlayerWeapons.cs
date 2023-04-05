using UnityEngine;

public class PlayerWeapons : MonoBehaviour, IMainWeapon, ISubWeapon
{
    [SerializeField]
    private Weapon mainWeapon;
    [SerializeField]
    private Transform mainShootTransform;
    private float mainWeaponDelayTimer;

    [SerializeField]
    private Weapon subWeapon;
    [SerializeField]
    private Transform[] subShootTransform;
    private float subWeaponDelayTimer;

    private GameManager gm;
    public float DPS { get => mainWeapon.damage / mainWeapon.reloadDelay + subWeapon.damage / subWeapon.reloadDelay * subShootTransform.Length; }

    public Weapon MainWeapon { get => mainWeapon; set => mainWeapon = value; }
    public Transform MainShootTransform { get => mainShootTransform; set => mainShootTransform = value; }
    public float MainWeaponDelayTimer { get => mainWeaponDelayTimer; set => mainWeaponDelayTimer = value; }
    public Weapon SubWeapon { get => subWeapon; set => subWeapon = value; }
    public Transform[] SubShootTransform { get => subShootTransform; set => subShootTransform = value; }
    public float SubWeaponDelayTimer { get => subWeaponDelayTimer; set => subWeaponDelayTimer = value; }

    private void Start()
    {
        gm = GameManager.Instance;
        if (mainWeapon != null)
            mainWeapon.Init();
        if (subWeapon != null)
            subWeapon.Init();
    }

    private void OnEnable()
    {
        mainWeaponDelayTimer = 0f;
        subWeaponDelayTimer = 0f;
    }

    private void Update()
    {
        ShootMainWeapon();
        ShootSubWeapon();
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log($"DPS : {DPS}");
        }
    }

    public void ShootMainWeapon()
    {
        if (mainWeapon == null)
            return;

        mainWeaponDelayTimer += Time.deltaTime;
        if (mainWeaponDelayTimer > mainWeapon.reloadDelay)
        {
            mainWeaponDelayTimer = 0f;
            string mainWeaponName = mainWeapon.bulletPrefab.name;
            GameObject bulletObj = gm.GetBullet(mainWeaponName);
            bulletObj.transform.position = mainShootTransform.position;
        }
    }

    public void ShootSubWeapon()
    {
        if (subWeapon == null)
            return;

        subWeaponDelayTimer += Time.deltaTime;
        if (subWeaponDelayTimer > subWeapon.reloadDelay)
        {
            subWeaponDelayTimer = 0f;
            int count = subShootTransform.Length;
            for (int i = 0; i < count; i++)
            {
                string subWeaponName = subWeapon.bulletPrefab.name;
                GameObject bulletObj = gm.GetBullet(subWeaponName);
                bulletObj.transform.position = subShootTransform[i].position;
            }
        }
    }
}