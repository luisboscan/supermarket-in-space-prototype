using System;
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

    public int currentCustomerAmount;
    private float timer;
    private float nextSpawnRate;
    private GameObject[] customerSpawnObjects;

    private static CustomerManager instance;

    void Start () {

        customerSpawnObjects = GameObject.FindGameObjectsWithTag("CustomerSpawn");
        nextSpawnRate = 2f;
        instance = this;
        GameState.Instance.SetDifficulty();
    }

    private ShoppingListItem[] BuildShoppingList()
    {

        List<ShoppingSectionType> shoppingSectionList = GetShoppingSectionList();
        ShoppingListItem[] shoppingList = new ShoppingListItem[GetNextShoppingListSize()];
        for (int i=0; i<shoppingList.Length; i++) {
            ShoppingListItem shoppingListItem = shoppingList[i];
            shoppingListItem.amount = GetNextItemAmount();
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

        if (GameState.Instance.state == GameState.State.OPEN)
        {
            timer += Time.deltaTime * GameState.Instance.globalSpeedModifier * GameState.Instance.shipSpeed;
            if (timer > nextSpawnRate)
            {
                SpawnCustomer();
            }
        } else
        {
            timer = 0;
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

        GameObject objectiveBubble = Instantiate(objectiveBubblePrefab, canvas.gameObject.transform.Find("BubbleObjectiveGroup"));
        objectiveBubble.SetActive(false);
        customer.GetComponent<Customer>().objectiveBubble = objectiveBubble.GetComponent<ObjectiveBubble>();
        return customer;
    }

    public void RemoveCustomer(GameObject gameObject)
    {
        Customer customer = gameObject.GetComponent<Customer>();
        GameState.Instance.AddRating(customer.mood);
        Destroy(gameObject.GetComponent<Customer>().objectiveBubble.gameObject);
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

    private int GetNextItemAmount()
    {
        return UnityEngine.Random.Range((int)dificultySettings[currentDificultyIndex].itemAmountRange.min,
            ((int)dificultySettings[currentDificultyIndex].itemAmountRange.max) + 1);
    }

    private float GetNextSpawnTime()
    {
        return UnityEngine.Random.Range(dificultySettings[currentDificultyIndex].spawnRateRange.min, 
            dificultySettings[currentDificultyIndex].spawnRateRange.max);
    }

    public static CustomerManager Instance
    {
        get { return instance; }
    }
}