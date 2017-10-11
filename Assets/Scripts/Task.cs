using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public enum TaskType
    {
        GRABBING_ITEM,
        RESTOCKING_ITEM,
        PAYING,
        OTHER
    }

    public const string TaskCompleteNotification = "Tasks.TaskCompleteNotification";
    public const string TaskStartNotification = "Tasks.TaskStartNotification";
    public const string TaskStoppedNotification = "Tasks.TaskStoppedNotification";

    public int peopleInTask;
    public float timeToComplete = 5f;
    public int priority = 1;
    public string targetTag;
    public TaskUI taskUI;
    public TaskType taskType;
    public Vector3 taskUIOffset;

    private float counter;
    public bool running;

    private void Start()
    {
        taskUI.Init();
    }

    public bool IsTaskAssignable(GameObject gameObject)
    {
        return gameObject.tag == targetTag;
    }

    public bool CanStartTask()
    {
        return true;
    }

    void Update()
    {
        if (!running)
        {
            taskUI.SetPeopleInLineAmount(0);
            return;
        }

        taskUI.SetPeopleInLineAmount(peopleInTask-1);

        float timeModifier = 1;
        if (taskType == TaskType.GRABBING_ITEM)
        {
            Resource resource = gameObject.GetComponent<Resource>();
            timeModifier = 1 - (resource.currentAmount / resource.maxAmount);
        }

        float actualTimeToComplete = timeToComplete * timeModifier;

        counter += Time.deltaTime;
        taskUI.ProgressBar.SetValue(counter / actualTimeToComplete);
        if (counter >= actualTimeToComplete)
        {
            Debug.Log("Task Finished");
            OnTaskFinished();
            gameObject.PostNotification(TaskCompleteNotification, this);
        }
    }

    public void StartTask()
    {
        Debug.Log("Task Started");
        counter = 0;
        running = true;
        taskUI.ProgressBar.Reset();
        taskUI.gameObject.SetActive(true);
        taskUI.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + taskUIOffset);
        gameObject.PostNotification(TaskStartNotification, this);
    }

    public void StopTask()
    {
        bool interrupted = running;
        OnTaskFinished();
        if (interrupted)
        {
            gameObject.PostNotification(TaskStoppedNotification, this);
        }
    }

    private void OnTaskFinished()
    {
        taskUI.gameObject.SetActive(false);
        running = false;
    }
}