using UnityEngine;

public class EnemyAI : MonoBehaviour, IMainWeapon
{
    public Vector2 moveDirection = Vector2.down;
    public float moveSpeed = 3f;
    [SerializeField]
    private Weapon mainWeapon;
    [SerializeField]
    private Transform mainShootTransform;
    private float mainWeaponDelayTimer;

    public Weapon MainWeapon { get => mainWeapon; set => mainWeapon = value; }
    public Transform MainShootTransform { get => mainShootTransform; set => mainShootTransform = value; }
    public float MainWeaponDelayTimer { get => mainWeaponDelayTimer; set => mainWeaponDelayTimer = value; }

    protected GameManager gm;

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        if (mainWeapon != null)
            mainWeapon.Init();
        mainWeaponDelayTimer = mainWeapon.reloadDelay;
    }

    protected virtual void Update()
    {
        ShootMainWeapon();
        if (gameObject.CompareTag("Enemy"))
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && collision.name.Equals("Bottom"))
        {
            gm.DestroyEnemyAircraft(gameObject);
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
}