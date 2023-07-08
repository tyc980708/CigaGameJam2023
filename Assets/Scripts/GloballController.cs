using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    // 注册Actor
    public Transform center;

    public GameObject enemy;
    public GameObject friend;

    public int enemyNum;
    public int friendNum;

    // 有效世界距离
    public float activeRange;

    // 保护距离
    public float protectionRange;


    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> activeFriends = new List<GameObject>();
    private int _frameCount;

    // Start is called before the first frame update
    public void Start()
    {   
        int i = enemyNum;

        while (i > 0)
        {
            activeEnemies.Add(ActorGenerator(enemy));
            i--;
        }

        int j = friendNum;

        while (j > 0)
        {
            activeFriends.Add(ActorGenerator(friend));
            j--;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        //低刷新率
        if (_frameCount == 30)
        {
            _frameCount = 0;
            //清理已经出距离的敌人
            for (int i = activeEnemies.Count - 1; i > 0; i--)
            {
                Debug.Log(Vector3.Distance(activeEnemies[i].transform.position, center.position));
                if (Vector3.Distance(activeEnemies[i].transform.position, center.position) > activeRange)
                {
                    Destroy(activeEnemies[i]);
                    activeEnemies.RemoveAt(i);
                }
            }
            //朋友数量少，不清GameObject了，尤其是万一要做八方来援
            for (int i = activeFriends.Count - 1; i > 0; i--)
            {
                if (Vector3.Distance(activeFriends[i].transform.position, center.position) > activeRange)
                {
                    activeFriends.RemoveAt(i);
                }
            }

            int enemiesToCreate = enemyNum - activeEnemies.Count;
            while (enemiesToCreate > 0)
            {
                activeEnemies.Add(ActorGenerator(enemy));
                enemiesToCreate--;
            }

            int friendsToCreate = friendNum - activeFriends.Count;
            while (friendsToCreate > 0)
            {
                activeFriends.Add(ActorGenerator(friend));
                friendsToCreate--;
            }
        }
        _frameCount++;
    }

    public GameObject ActorGenerator(GameObject obj)
    {
        Vector3 pos = RandPos();
        Vector3 rotation = new Vector3(0f, 0f, Random.Range(0f, 360f));
        return Object.Instantiate(obj, pos, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
    }

    private Vector3 RandPos()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        do
        {
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            float dist = Random.Range(protectionRange, activeRange);
            pos = center.position + dir * dist;
        }
        while (IsTooClose(pos));
        Debug.Log(pos);
        return pos;
    }

    private bool IsTooClose(Vector3 position)
    {
        // Check if the generated position is too close to any existing game objects
        Collider[] colliders = Physics.OverlapSphere(position, protectionRange);

        return colliders.Length > 0;
    }
}
