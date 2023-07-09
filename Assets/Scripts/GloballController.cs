using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    // 注册Actor
    public Transform center;
    public float avatarLevel = 1f;

    public GameObject enemy;
    public GameObject friend;

    public int enemyNum;
    public int friendNum;

    // 有效世界距离
    public float activeRange;

    // 保护距离
    public float protectionRange;

    public float evo3Range;

    [Header("演职人员")]
    public GameObject cast;


    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> activeFriends = new List<GameObject>();
    private List<GameObject> activeEvo3s = new List<GameObject>();
    private int _frameCount;

    // Start is called before the first frame update
    public void Start()
    {   
        int i = enemyNum;

        while (i > 0)
        {
            activeEnemies.Add(EnemyGenerator(enemy));
            i--;
        }

        int j = friendNum;

        while (j > 0)
        {
            activeFriends.Add(FriendGenerator(friend));
            j--;
        }

        activeEvo3s.Add(Evo3Generator(friend));
    }

    // Update is called once per frame
    public void Update()
    {
        //低刷新率
        if (_frameCount == 30)
        {
            _frameCount = 0;
            center = GameObject.Find("Avatar").transform;
            //清理已经出距离的敌人
            for (int i = activeEnemies.Count - 1; i > 0; i--)
            {
                if (activeEnemies[i] == null)
                {
                    activeEnemies.RemoveAt(i);
                    continue;
                }
                if (Vector3.Distance(activeEnemies[i].transform.position, center.position) > activeRange)
                {
                    Destroy(activeEnemies[i]);
                    activeEnemies.RemoveAt(i);
                }
            }
            //朋友数量少，不清GameObject了，尤其是万一要做八方来援
            for (int i = activeFriends.Count - 1; i > 0; i--)
            {
                if (activeFriends[i] == null)
                {
                    activeFriends.RemoveAt(i);
                    continue;
                }
                if (Vector3.Distance(activeFriends[i].transform.position, center.position) > activeRange)
                {
                    activeFriends.RemoveAt(i);
                    continue;
                }
                //需要额外加一个是否帮助过判断，帮助过了就移出列表腾格子
                //再增加一个是否和现在等级相同，尽量生成新等级的
                var friend = activeFriends[i].GetComponent<Friend>();
                if ((friend != null && friend.isHelped) || (friend.evoLevel != avatarLevel && avatarLevel < 3))
                    activeFriends.RemoveAt(i);
            }

            var needNewEvo3 = true;
            foreach (var evo3 in activeEvo3s)
            {
                if (Vector3.Distance(evo3.transform.position, center.position) < evo3Range)
                    needNewEvo3 = false;
            }
            if (needNewEvo3)
                activeEvo3s.Add(Evo3Generator(friend));

            int enemiesToCreate = enemyNum - activeEnemies.Count;
            while (enemiesToCreate > 0)
            {
                activeEnemies.Add(EnemyGenerator(enemy));
                enemiesToCreate--;
            }

            int friendsToCreate = friendNum - activeFriends.Count;
            while (friendsToCreate > 0)
            {
                activeFriends.Add(FriendGenerator(friend));
                friendsToCreate--;
            }
        }
        _frameCount++;
        if (avatarLevel == 3)
            cast.SetActive(true);
    }

    public GameObject EnemyGenerator(GameObject obj)
    {
        Vector3 pos = RandPos(protectionRange, activeRange, center.position);
        Vector3 rotation = new Vector3(0f, 0f, Random.Range(0f, 360f));
        return Object.Instantiate(obj, pos, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
    }
    public GameObject FriendGenerator(GameObject obj)
    {
        Vector3 pos = RandPos(protectionRange, activeRange, center.position);
        Vector3 rotation = new Vector3(0f, 0f, Random.Range(0f, 360f));
        var go = Object.Instantiate(obj, pos, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
        go.GetComponent<Friend>().evoLevel = RandEvoLevel();
        go.GetComponent<Friend>().isHelped = false;
        return go;
    }
    public GameObject Evo3Generator(GameObject obj)
    {
        Vector3 pos = RandPos(protectionRange, activeRange, center.position);
        Vector3 rotation = new Vector3(0f, 0f, Random.Range(0f, 360f));
        var go = Object.Instantiate(obj, pos, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
        go.GetComponent<Friend>().evoLevel = 3f;
        go.GetComponent<Friend>().isHelped = false;

        for (int i = 0; i <= 4; i++)
        {
            bool generate = Random.Range(1, 2) != 1;
            if (generate)
            {
                Vector3 subpos = RandPos(3f, 10f, pos);
                Vector3 subrot = new Vector3(0f, 0f, Random.Range(0f, 360f));
                AccompanyGenerator(friend, subpos, subrot, go.GetComponent<BaseActor>());
            }
        }

        return go;
    }
    public GameObject AccompanyGenerator(GameObject obj, Vector3 pos, Vector3 rotation, BaseActor focus)
    {
        var go = Object.Instantiate(obj, pos, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
        go.GetComponent<Friend>().evoLevel = RandEvoLevel();
        go.GetComponent<Friend>().isHelped = false;
        go.GetComponent<Friend>().focusedTarget = focus;
        return go;
    }

    private float RandEvoLevel()
    {
        switch (avatarLevel)
        {
            case 1f:
                var rd1 = Random.Range(1f, 5f);
                if (rd1 <= 4f) return 1f;
                else return 2f;
            case 2f:
                var rd2 = Random.Range(1f, 5f);
                if (rd2 <= 4f) return 2f;
                else return 1f;
            case 3f:
                var rd3 = Random.Range(1, 3);
                if (rd3 == 1) return 1f;
                else return 2f;
        }
        return 1f;
    }
    private Vector3 RandPos(float protectionRange, float activeRange, Vector3 center)
    {
        Vector3 pos = new Vector3(0, 0, 0);
        do
        {
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
            float dist = Random.Range(protectionRange, activeRange);
            pos = center + dir * dist;
        }
        while (false);
        return pos;
    }

    private bool IsTooClose(Vector3 position)
    {
        // Check if the generated position is too close to any existing game objects
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, protectionRange);

        return colliders.Length > 0;
    }
}
