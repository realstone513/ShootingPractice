using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Setting")]
    public GameObject player;
    public GameObject boss;
    public List<GameObject> bossPrefabs;
    public List<GameObject> enemyPrefabs;
    public GameObject bossHPBar;
    public TextMeshProUGUI centerText;
    public TextMeshProUGUI scoreText;
    private List<GameObject> liveEnemyAircrafts = new ();
    private int score;
    public bool isClear = false;

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
        bossHPBar.SetActive(false);
        if (boss != null)
        {
            Destroy(boss);
        }
        player.SetActive(true);
        player.transform.position = playerWaitPos;
        centerText.gameObject.SetActive(false);
        TranslateScore(0, !isClear); // 클리어 못하면 점수 초기화, 클리어면 초기화 X
        StartCoroutine(CoStartGame());
        isClear = false;
    }

    private IEnumerator CoStartGame()
    {
        float arrangeSpeed = player.GetComponent<PlayerController>().speed + 1;
        while (player.transform.position.y < playerInitPos.y)
        {
            player.transform.position += (arrangeSpeed + 1) * Time.deltaTime * Vector3.up;
            yield return null;
        }
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
            //Debug.Log($"wave {i}, arrange {waveData.arrange} interval {waveData.interval}");
            yield return new WaitForSeconds(waveData.interval);
        }

        WaveData bossWave = stageDatas[currentStageIndex][0];
        char bossID = bossWave.arrange[0];
        switch (bossID)
        {
            case 'A':
                boss = Instantiate(bossPrefabs[0], bossWaitPos, Quaternion.identity, spawnPositions[spawnPositions.Count / 2]); // Center
                break;
            default: 
                break;
        }
        boss.SetActive(true);
        HpBar bossHPbarComponent = bossHPBar.GetComponent<HpBar>();
        boss.GetComponent<Aircraft>().hpBar = bossHPbarComponent;
        bossHPbarComponent.SetFill(1f);
        bossHPBar.SetActive(true);
    }

    public void EndGame()
    {
        player.SetActive(false);
        StopAllCoroutines();
        centerText.gameObject.SetActive(true);
        centerText.text = "PRESS ANY KEY";
        player.transform.position = playerWaitPos;
    }

    public void ClearGame()
    {
        isClear = true;
        centerText.gameObject.SetActive(true);
        centerText.text = "CLEAR !!\nPRESS SPACE TO START";
        if (stageDatas.Count != currentStageIndex)
            currentStageIndex++;
    }

    public void TranslateScore(int score, bool setScore = false)
    {
        if (setScore)
            this.score = score;
        else
            this.score += score;
        scoreText.text = $"SCORE : {this.score}";
    }

    public void DestroyEnemyAircraft(GameObject gameObject, bool playerKill = false)
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
