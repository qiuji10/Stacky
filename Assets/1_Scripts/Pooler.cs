using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubeType { xCube, zCube, bonusCube, dropCube }

public class Pooler : MonoBehaviour
{
    [System.Serializable]
	public class CubePoolInfo
	{
		public CubeType type;
		public int amount;
		public Transform cubePrefab;
		public Transform container;

		public List<Transform> cubePoolList = new List<Transform>();
	}

    [SerializeField] private List<CubePoolInfo> cubesPool;

    void Start()
	{
		for (int i = 0; i < cubesPool.Count; i++)
		{
			FillCubePool(cubesPool[i]);
		}
    }

    void FillCubePool(CubePoolInfo info)
	{ 
		for (int i = 0; i < info.amount; i++)
		{
			Transform newCube = Instantiate(info.cubePrefab, Vector3.zero, Quaternion.identity, info.container);
			newCube.gameObject.SetActive(false);
			info.cubePoolList.Add(newCube);
		}
	}

    public Transform GetFromPool(CubeType type)
	{
		CubePoolInfo pool = GetPoolByType(type);

		for (int i = 0; i < pool.cubePoolList.Count; i++)
		{
			if (!pool.cubePoolList[i].gameObject.activeInHierarchy)
			{
				return pool.cubePoolList[i];
			}
		}

		Transform newCube = Instantiate(pool.cubePrefab, pool.container);
		pool.cubePoolList.Add(newCube);
		return newCube;
	}

    CubePoolInfo GetPoolByType(CubeType poolType)
	{
		for (int i = 0; i < cubesPool.Count; i++)
		{
			if (poolType == cubesPool[i].type)
			{
				return cubesPool[i];
			}
		}
		
		return null;
	}
}
