﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour {

    public int currentDificultyIndex = 0;
    public DificultySetting[] dificultySettings;
    public GameObject customerPrefab;
    public GameObject objectiveBubblePrefab;
    public Material[] materials;
    public Canvas canvas;

    private int currentCustomerAmount;
    private float timer;
    private float nextSpawnRate;
    private GameObject[] customerSpawnObjects;

    void Start () {

        customerSpawnObjects = GameObject.FindGameObjectsWithTag("CustomerSpawn");
        nextSpawnRate = GetNextSpawnTime();
    }

    private ShoppingListItem[] BuildShoppingList()
    {

        List<ShoppingSectionType> shoppingSectionList = GetShoppingSectionList();
        ShoppingListItem[] shoppingList = new ShoppingListItem[GetNextShoppingListSize()];
        for (int i=0; i<shoppingList.Length; i++) {
            ShoppingListItem shoppingListItem = shoppingList[i];
            shoppingListItem.amount = UnityEngine.Random.Range(1, 4);
            int shoppingSectionIndex = UnityEngine.Random.Range(0, shoppingSectionList.Count);
            ShoppingSectionType shoppingSection = shoppingSectionList[shoppingSectionIndex];
            shoppingListItem.shoppingSection = shoppingSection;
            shoppingSectionList.RemoveAt(shoppingSectionIndex);
            shoppingList[i] = shoppingListItem;
        }
        return shoppingList;
    }

    private List<ShoppingSectionType> GetShoppingSectionList()
    {
        Array shoppingSectionArray = Enum.GetValues(typeof(ShoppingSectionType));
        List<ShoppingSectionType> shoppingSections = new List<ShoppingSectionType>();
        for (int i = 0; i < shoppingSectionArray.Length; i++)
        {
            shoppingSections.Add((ShoppingSectionType)shoppingSectionArray.GetValue(i));
        }
        return shoppingSections;
    }

    void Update()
    {
        if (currentCustomerAmount >= dificultySettings[currentDificultyIndex].maxCustomerAmount)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > nextSpawnRate)
        {
            SpawnCustomer();
        }
    }

    public GameObject SpawnCustomer ()
    {
        currentCustomerAmount++;
        timer = 0;
        nextSpawnRate = GetNextSpawnTime();
        GameObject customer = Instantiate(customerPrefab, GetNextSpawnPoint(), Quaternion.identity);
        customer.GetComponent<NodeNavigation>().speed = GetNextSpeed();
        customer.GetComponent<Customer>().shoppingListItems = BuildShoppingList();
        customer.GetComponent<Renderer>().material = materials[UnityEngine.Random.Range(0, materials.Length)];

        GameObject objectiveBubble = Instantiate(objectiveBubblePrefab, canvas.gameObject.transform);
        objectiveBubble.SetActive(false);
        customer.GetComponent<Customer>().objectiveBubble = objectiveBubble.GetComponent<ObjectiveBubble>();
        return customer;
    }

    public void RemoveCustomer(GameObject gameObject)
    {
        Destroy(gameObject);
        currentCustomerAmount--;
    }

    private Vector3 GetNextSpawnPoint()
    {
        int index = UnityEngine.Random.Range(0, customerSpawnObjects.Length);
        return customerSpawnObjects[index].transform.position;
    }

    private float GetNextSpeed()
    {
        return UnityEngine.Random.Range(dificultySettings[currentDificultyIndex].speedRange.min,
            (dificultySettings[currentDificultyIndex].speedRange.max));
    }

    private int GetNextShoppingListSize()
    {
        return UnityEngine.Random.Range((int)dificultySettings[currentDificultyIndex].itemListRange.min,
            ((int)dificultySettings[currentDificultyIndex].itemListRange.max) + 1);
    }

    private float GetNextSpawnTime()
    {
        return UnityEngine.Random.Range(dificultySettings[currentDificultyIndex].spawnRateRange.min, 
            dificultySettings[currentDificultyIndex].spawnRateRange.max);
    }
}