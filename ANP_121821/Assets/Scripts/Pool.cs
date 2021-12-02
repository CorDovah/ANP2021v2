using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    List<GameObject> pool;
    GameObject prefab;

    public void Starting(GameObject _prefab, int _size)
    {
        pool = new List<GameObject>();
        prefab = _prefab;
        for (int i = 0; i < _size; i++)
        {
            GameObject go = Instatiating(_prefab.transform.position, _prefab.transform.rotation);
            go.SetActive(false);
        }
    }

    GameObject Instatiating(Vector3 _pos, Quaternion _rot)
    {
        GameObject go = GameObject.Instantiate(prefab, _pos, _rot);
        go.name = go.name + "_" + pool.Count;
        pool.Add(go);
        return go;
    }

    public GameObject Spawn(Vector3 _pos, Quaternion _rot)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeSelf == false)
            {
                pool[i].transform.position = _pos;
                pool[i].transform.rotation = _rot;
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        return Instatiating(_pos, _rot);
    }
}
