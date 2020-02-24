using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    [SerializeField] private GameObject block;
    [SerializeField] private GameObject gap;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject baseBlock;
    public static List<GameObject> poolList;
    private int poolLimit = 20;

    public Transform blocks;
    
    private int interval;

    private float blockStartPosX = -8.5f;
    private float blockStartPosY = -1.82f;

    // Start is called before the first frame update
    private void Start()
    {
        poolList = new List<GameObject>();
        AddBlocksToPool();
    }

    public void AddBlocksToPool()
    {
        interval = 0;
        for(int i = 0; i < poolLimit; i++)
        {
            GameObject temp = Instantiate(baseBlock);
            temp.tag = "Block";
            temp.transform.parent = blocks;
            temp.name = "BlockObject";
            temp.transform.position = new Vector3(blockStartPosX + 0.96f * i, blockStartPosY - 0.96f, 0);
            temp.layer = 8;
            poolList.Add(temp);
        }
    }

    private void Update()
    {
        if (poolList.Count < poolLimit)
        {
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        if (interval > 70)
        {
            int height = Random.Range(0, 4);
            int obstacleHeight = Random.Range(1, 3);
            int objectInt = Random.Range(0, 7);

            if (objectInt >= 3 || interval <= 80)
            {
                GameObject temp = Instantiate(block);
                temp.transform.parent = blocks;
                temp.name = "BlockObject";
                temp.transform.position = new Vector3(poolList[poolLimit - 2].transform.position.x + 0.91f, blockStartPosY + 0.96f * height, 0);
                temp.layer = 8;
                poolList.Add(temp);
            }
            else if (objectInt == 2)
            {
                GameObject temp = Instantiate(obstacle);
                temp.transform.parent = blocks;
                temp.name = "ObstacleObject";
                temp.transform.position = new Vector3(poolList[poolLimit - 2].transform.position.x + 0.91f, blockStartPosY + 0.96f * obstacleHeight, 0);
                temp.layer = 8;
                poolList.Add(temp);
            }
            else
            {
                GameObject temp = Instantiate(gap);
                temp.transform.parent = blocks;
                temp.name = "GapObject";
                temp.transform.position = new Vector3(poolList[poolLimit - 2].transform.position.x + 0.91f, blockStartPosY - 0.96f, 0);
                temp.layer = 8;
                poolList.Add(temp);
            }

            
        }
        else if (interval > 20)
        {
            int height = Random.Range(0, 2);
            int objectInt = Random.Range(0, 5);

            if (objectInt >= 2)
            {
                GameObject temp = Instantiate(block);
                temp.transform.parent = blocks;
                temp.name = "BlockObject";
                temp.transform.position = new Vector3(poolList[poolLimit - 2].transform.position.x + 0.91f, blockStartPosY + 0.96f * height, 0);
                temp.layer = 8;
                poolList.Add(temp);
            }
            else
            {
                GameObject temp = Instantiate(gap);
                temp.transform.parent = blocks;
                temp.name = "GapObject";
                temp.transform.position = new Vector3(poolList[poolLimit - 2].transform.position.x + 0.91f, blockStartPosY - 0.96f, 0);
                temp.layer = 8;
                poolList.Add(temp);
            }
        }
        else
        {
            GameObject temp = Instantiate(baseBlock);
            temp.transform.parent = blocks;
            temp.name = "BaseObject";
            temp.transform.position = new Vector3(poolList[poolLimit - 2].transform.position.x + 0.91f, blockStartPosY - 0.96f, 0);
            temp.layer = 8;
            poolList.Add(temp);
        }

        interval++;
    }

    public void ResetPool()
    {
        for (int i = 0; i < poolLimit; i++)
        {
            Destroy(poolList[i]);
        }
        poolList.Clear();

        AddBlocksToPool();
    }
}
