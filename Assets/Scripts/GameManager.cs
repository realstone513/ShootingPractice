using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : Singleton<GameManager>
{
    [Header("Platform Setting")]
    public GameObject[] backgrounds;
    public float backgroundSize;
    public float platformSpeed;

    [Header("Bullets")]
    [SerializeField]
    private List<GameObject> bulletTypes;
    public Dictionary<string, IObjectPool<GameObject>> bullets = new ();
    private GameObject curBulletObject;

    public Transform bulletsTransform;

    public override void Awake()
    {
        base.Awake();
        int count = bulletTypes.Count;
        for (int i = 0; i < count; i++)
        {
            curBulletObject = bulletTypes[i];
            Bullet bullet = bulletTypes[i].GetComponent<Bullet>();
            string bulletPoolName = $"{curBulletObject.name} Pool";
            ObjectPool<GameObject> objPool = new (OnCreate, OnGet, OnRelease, OnPoolObjDestroy, maxSize: 20);
            bullets[curBulletObject.name] = objPool;
        }
    }

    public GameObject GetBullet(string name)
    {
        curBulletObject = bulletTypes.Find(x => x.name.Equals(name));
        return bullets[name].Get();
    }

    private GameObject OnCreate()
    {
        GameObject bulletObj = Instantiate(curBulletObject, bulletsTransform);
        bulletObj.GetComponent<Bullet>().SetPool(bullets[curBulletObject.name]);
        return bulletObj;
    }

    private void OnGet(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    private void OnRelease(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    private void OnPoolObjDestroy(GameObject bullet)
    {
        Destroy(bullet);
    }

    private void Update()
    {
        int length = backgrounds.Length;
        for (int i = 0; i < length; i++)
        {
            backgrounds[i].transform.Translate(platformSpeed * Time.deltaTime * Vector2.down);
            if (backgrounds[i].transform.position.y < -backgroundSize)
                backgrounds[i].transform.Translate(2 * backgroundSize * Vector3.up);
        }
    }
}
