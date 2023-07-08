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

    public int enemyNum;

    // 有效世界距离
    public float activeRange;

    // 保护距离
    public float protectionRange;

    // Start is called before the first frame update
    public void Start()
    {   
        int i = enemyNum;

        while (i > 0)
        {
            ActorGenerator(enemy);
            i--;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void ActorGenerator(GameObject obj)
    {
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        float dist = Random.Range(protectionRange, activeRange);
        Vector3 pos = center.position + dir * dist;
        Vector3 rotation = new Vector3(0f, 0f, Random.Range(0f, 360f));
        

        Object.Instantiate(obj, pos, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
    }
}
