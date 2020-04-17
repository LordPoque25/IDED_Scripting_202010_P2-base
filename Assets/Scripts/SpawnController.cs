using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnObjects;
    [SerializeField]
    private int maxpoolHigh;
    [SerializeField]
    private int maxpoolMid;
    [SerializeField]
    private int maxpoolLow;
    private GameObject[] PoolItems;

    private int actualobject = 0;


    [SerializeField]
    private float spawnRate = 1f;

    [SerializeField]
    private float firstSpawnDelay = 0f;

    [SerializeField]
    private Player player;

    private Vector3 spawnPoint;

    private bool IsThereAtLeastOneObjectToSpawn
    {
        get
        {
            bool result = false;

            for (int i = 0; i < spawnObjects.Length; i++)
            {
                result = spawnObjects[i] != null;

                if (result)
                {
                    break;
                }
            }

            return result;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        FillPool();
        StartCoroutine(SpawnEnemies());
        /*
        if (spawnObjects.Length > 0 && IsThereAtLeastOneObjectToSpawn)
        {
            InvokeRepeating("SpawnObject", firstSpawnDelay, spawnRate);

            if (player != null)
            {
                Player.OnPlayerDied += StopSpawning;
            }
        }*/
    }

    private void SpawnObject()
    {
        int randomitem = Random.Range(0, PoolItems.Length - 1);
        if (PoolItems[randomitem].activeInHierarchy == false)
        {
            PoolItems[randomitem].SetActive(true);
            Rigidbody rbitem = PoolItems[randomitem].GetComponent<Rigidbody>();
            rbitem.velocity = Vector3.zero;
            spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0F, 1F), 1F, transform.position.z));
            PoolItems[randomitem].transform.position = spawnPoint;
            PoolItems[randomitem].transform.rotation = Quaternion.identity;
        }
    }
    /*
    private void StopSpawning()
    {
        CancelInvoke();
    }
    */
    public void FillPool()
    {
        PoolItems = new GameObject[maxpoolHigh + maxpoolLow + maxpoolMid];
        for (int i = 0; i < maxpoolHigh; i++)
        {
            GameObject clone = Instantiate(spawnObjects[0]);
            PoolItems[i] = clone;
            PoolItems[i].SetActive(false);
        }
        for (int j = 0; j < maxpoolLow; j++)
        {
            GameObject clone2 = Instantiate(spawnObjects[1]);
            PoolItems[maxpoolHigh + j] = clone2;
            PoolItems[maxpoolHigh + j].SetActive(false);
        }
        for (int k = 0; k < maxpoolMid; k++)
        {
            GameObject clone3 = Instantiate(spawnObjects[2]);
            PoolItems[maxpoolLow + maxpoolHigh + k] = clone3;
            PoolItems[maxpoolLow + maxpoolHigh + k].SetActive(false);
        }
    }
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            if (player.dead == false)
            {
                SpawnObject();
            }
        }
    }
}