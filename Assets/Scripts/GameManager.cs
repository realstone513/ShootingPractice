using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Setting")]
    public GameObject player;
    public GameObject boss;
    public GameObject bossHPBar;
    public List<GameObject> enemyPrefabs;
    public GameObject readyText;
    private List<GameObject> liveEnemyAircrafts = new ();
    private bool isPlaying = false;

    [Header("Stage")]
    private List<Dictionary<int, WaveData>> stageDatas = new();
    private int currentStageIndex = 0;
    public List<Transform> spawnPositions = new ();

    [Header("Platform")]
    public GameObject[] backgrounds;
    public float backgroundSize;
    public float platformSpeed;

    [Header("Bullet")]
    [SerializeField]
    private List<GameObject> bulletTypes;
    public Dictionary<string, CustomObjectPool> bullets = new ();
    public int poolLimitCount = 20;
    public Transform bulletsTransform;

    [Header("Variables")]
    public Vector2 playerInitPos = new(0, -4);
    public Vector2 playerWaitPos = new(0, -7);
    public Vector2 bossInitPos = new(0, 3);
    public Vector2 bossWaitPos = new(0, 7);

    public struct WaveData
    {
        public string arrange;
        public float interval;

        public WaveData(string arrange, float interval)
        {
            this.arrange = arrange;
            this.interval = interval;
        }
    }

    public override void Awake()
    {
        base.Awake();
        int count = bulletTypes.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject spawnObj = new (bulletTypes[i].name);
            spawnObj.transform.parent = bulletsTransform;
            bullets[bulletTypes[i].name] = new (poolLimitCount, bulletTypes[i], spawnObj.transform);
        }
        bulletTypes.Clear();
        ReadAllStages();
    }

    private void ReadAllStages()
    {
        string stageStr = "stage";
        int stageFileCount = 0;
        string resourcesPath = $"{Application.dataPath}/Resources";
        StringBuilder filePath = new ();
        while (true)
        {
            filePath.Append($"{stageStr}{(stageFileCount < 10 ? "0" : "")}{stageFileCount}");
            if (File.Exists($"{resourcesPath}/{filePath}.csv"))
            {
                List<Dictionary<string, object>> data = CSVReader.Read(filePath.ToString());
                int stageCount = (int)data[0]["Index"];

                Dictionary<int, WaveData> currentStage = new();
                for (int i = 0; i <= stageCount; i++)
                {
                    string arrange = (string)data[i]["arrange"];
                    float interval = float.Parse(data[i]["interval"].ToString());
                    int index = (int)data[i]["Index"];
                    currentStage.Add(index, new WaveData(arrange, interval));
                }
                stageDatas.Add(currentStage);
            }
            else
                break;
            filePath.Clear();
            stageFileCount++;
        }
    }

    public void StartGame()
    {
        int count = liveEnemyAircrafts.Count;
        for (int i = 0; i < count; i++)
            Destroy(liveEnemyAircrafts[i]);
        liveEnemyAircrafts.Clear();
        boss.SetActive(false);
        bossHPBar.SetActive(false);
        boss.transform.position = bossWaitPos;
        player.SetActive(true);
        readyText.SetActive(false);
        StartCoroutine(CoStartGame());
    }

    private IEnumerator CoStartGame()
    {
        float arrangeSpeed = player.GetComponent<PlayerController>().speed + 1;
        while (player.transform.position.y < playerInitPos.y)
        {
            player.transform.position += (arrangeSpeed + 1) * Time.deltaTime * Vector3.up;
            yield return null;
        }
        isPlaying = true;
        StartCoroutine(CoStartStage());
    }

    private IEnumerator CoStartStage()
    {
        int waveCount = stageDatas[currentStageIndex].Count - 1;
        for (int i = waveCount; i > 0; i--)
        {
            WaveData waveData = stageDatas[currentStageIndex][i];
            int spawnPosCount = spawnPositions.Count;
            for (int j = 0; j < spawnPosCount; j++)
            {
                char enemy = waveData.arrange[j];
                switch (enemy)
                {
                    case 'A':
                        liveEnemyAircrafts.Add(Instantiate(enemyPrefabs[0], spawnPositions[j]));
                        break;

                    case 'B':
                        liveEnemyAircrafts.Add(Instantiate(enemyPrefabs[1], spawnPositions[j]));
                        break;

                    case 'C':
                        liveEnemyAircrafts.Add(Instantiate(enemyPrefabs[2], spawnPositions[j]));
                        break;

                    case 'X':
                    default:
                        break;
                }
            }
            Debug.Log($"wave {i}, arrange {waveData.arrange} interval {waveData.interval}");
            yield return new WaitForSeconds(waveData.interval);
        }

        Debug.Log("Boss");
        boss.SetActive(true);
        bossHPBar.SetActive(true);
    }

    public void EndGame()
    {
        player.SetActive(false);
        readyText.SetActive(true);
        player.transform.position = playerWaitPos;
        isPlaying = false;
    }

    public void DestroyAircraft(GameObject gameObject)
    {
        liveEnemyAircrafts.Remove(gameObject);
        Destroy(gameObject);
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

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    StartGame();
        //}
        if (Input.GetKeyDown(KeyCode.P))
        {
            EndGame();
        }
    }
}
