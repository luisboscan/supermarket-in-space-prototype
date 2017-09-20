using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour {

    public int maxCustomerAmount = 10;
    public float minSpawnRate = 5;
    public float maxSpawnRate = 8;
    public GameObject customerPrefab;

    private int currentCustomerAmount;
    private float timer;
    private float nextSpawnRate;
    private GameObject[] customerSpawnObjects;

    void Start () {

        customerSpawnObjects = GameObject.FindGameObjectsWithTag("CustomerSpawn");
        nextSpawnRate = Random.Range(minSpawnRate, maxSpawnRate);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > nextSpawnRate)
        {

        }
        nextSpawnRate = Random.Range(minSpawnRate, maxSpawnRate);
    }

    public GameObject SpawnCustomer () {
		
        if (currentCustomerAmount >= maxCustomerAmount)
        {
            return null;
        }
        return Instantiate(customerPrefab, GetNextSpawnPoint(), Quaternion.identity);
    }

    private Vector3 GetNextSpawnPoint()
    {
        int index = Random.Range(0, customerSpawnObjects.Length);
        return customerSpawnObjects[index].transform.position;
    }
}
