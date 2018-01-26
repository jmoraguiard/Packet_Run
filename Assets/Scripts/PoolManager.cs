using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

	public GameObject PooledObject;
	public int Amount;

	private List<GameObject> _pooledObjects;

	void Awake()
	{
		_pooledObjects = new List<GameObject> ();

		GameObject parent = new GameObject("Pool of " + PooledObject.name);

		for (int i = 0; i < Amount; ++i)
		{
			GameObject go = Instantiate(PooledObject);
			go.SetActive(false);
			go.transform.parent = parent.transform;
			_pooledObjects.Add(go);
		}
	}

	public GameObject GetPooledObject () {
		for (int i = 0; i < _pooledObjects.Count; ++i) {
			if (!_pooledObjects [i].activeInHierarchy) {
				return _pooledObjects [i];
			}
		}
		GameObject parent = new GameObject("Pool of " + PooledObject.name);

		GameObject go = Instantiate(PooledObject);
		go.SetActive(false);
		go.transform.parent = parent.transform;
		_pooledObjects.Add(go);

		return go;
	}
}
