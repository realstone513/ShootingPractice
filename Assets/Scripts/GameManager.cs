using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Platform Setting")]
    public GameObject[] backgrounds;
    public float backgroundSize;
    public float platformSpeed;

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
