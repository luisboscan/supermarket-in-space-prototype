using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public const string TaskCompleteNotification = "Tasks.TaskCompleteNotification";
    public const string TaskStartNotification = "Tasks.TaskStartNotification";

    public float timeToComplete = 5f;
    public float taskDelay = 5f;
    public int priority = 1;
    public LayerMask targetMask;

    private float counter;
    private bool running;

    public bool IsTaskAssignable(GameObject gameObject)
    {
        return Util.IsObjectInLayerMask(targetMask, gameObject);
    }

    void Update()
    {
        if (!running)
        {
            return;
        }

        counter += Time.deltaTime;
        Debug.Log(counter);
        if (counter >= timeToComplete)
        {
            Debug.Log("Task Finished");
            running = false;
            this.PostNotification(Task.TaskCompleteNotification);
        }
    }

    public void StartTask()
    {
        Debug.Log("Task Started");
        counter = 0;
        running = true;
        this.PostNotification(Task.TaskStartNotification);
    }
}