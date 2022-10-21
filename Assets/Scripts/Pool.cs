using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private GameObject gameObject;
    private GameObject container;
    private int poolCapacity;
    private List<GameObject> poolObjects;

    public Pool(GameObject gameObject, GameObject container, int strartCapacity = 10)
    {
        this.gameObject = gameObject;
        this.container = container;
        this.poolCapacity = strartCapacity;

        CreatePool();
    }

    private void CreatePool()
    {
        poolObjects = new List<GameObject>();

        for (int i = 0; i < poolCapacity; i++)
        {
            CreateElement();
        }
    }

    private GameObject CreateElement(bool isActiveByDefault = false)
    {
        var createdObject = GameObject.Instantiate(gameObject, container.transform);
        createdObject.SetActive(isActiveByDefault);

        poolObjects.Add(createdObject);

        return createdObject;
    }

    public bool TryGetElement(out GameObject element)
    {
        foreach (var item in poolObjects)
        {
            if (!item.activeSelf)
            {
                element = item;
                item.SetActive(true);
                item.transform.SetParent(container.transform);
                return true;
            }
        }

        element = null;
        return false;
    }

    public GameObject GetFreeElement()
    {
        if (TryGetElement(out var element))
        {
            return element;
        }

        return CreateElement(true);
    }

    public GameObject GetFreeElement(Vector3 position)
    {
        var element = GetFreeElement();
        element.transform.position = position;

        return element;
    }

    public GameObject GetFreeElement(Vector3 position, Quaternion quaternion)
    {
        var element = GetFreeElement();
        element.transform.position = position;
        element.transform.rotation = quaternion;

        return element;
    }

    public void CleatPool()
    {
        foreach (var item in poolObjects)
        {
            item.SetActive(false);
        }
    }

    public List<GameObject> GetPool()
    {
        return poolObjects;
    }
}