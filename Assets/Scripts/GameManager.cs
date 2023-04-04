using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Settings")]
    public GameObject player;
    public GameObject boss;
    public bool isPlaying = false;
    public GameObject readyText;

    [Header("Platform Setting")]
    public GameObject[] backgrounds;
    public float backgroundSize;
    public float platformSpeed;

    [Header("Bullet Object Pool")]
    [SerializeField]
    private List<GameObject> bulletTypes;
    public Dictionary<string, CustomObjectPool> bullets = new ();
    public int poolLimitCount = 20;

    public Transform bulletsTransform;

    public override void Awake()
    {
        base.Awake();
        int count = bulletTypes.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject spawnObj = new (bulletTypes[i].name);
            Transform spawnTransform = Instantiate(spawnObj, bulletsTransform).transform;
            bullets[bulletTypes[i].name] = new (poolLimitCount, bulletTypes[i], spawnTransform);
        }
        bulletTypes.Clear();
    }

    public void StartGame()
    {
        player.SetActive(true);
        readyText.SetActive(false);
        StartCoroutine(CoStartGame());
    }

    private IEnumerator CoStartGame()
    {
        while (player.transform.position.y < -4)
        {
            player.transform.position += 3 * Time.deltaTime * Vector3.up;
            yield return null;
        }
        isPlaying = true;
    }

    public void EndGame()
    {
        player.SetActive(false);
        readyText.SetActive(true);
        player.transform.position = new Vector3(0, -7f, 0);
        isPlaying = false;
    }

    public GameObject GetBullet(string name)
    {
        return bullets[name].GetObject();
    }

    public void ReleaseBullet(GameObject bullet)
    {
        bullets[bullet.name].ReleaseObject(bullet);
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            EndGame();
        }
    }
}
