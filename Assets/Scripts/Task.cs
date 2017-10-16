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

    private float counter;
    private float timeModifier = 1;
    private Vector3 uiOffset = new Vector3(0, 0, 2);
    public bool running;

    private void Start()
    {
        Init();
    }

    protected void Init()
    {
        taskUI.Init();
    }

    public bool IsTaskAssignable(GameObject gameObject)
    {
        return gameObject.tag == targetTag;
    }

    public virtual bool CanStartTask(GameObject gameObject)
    {
        return true;
    }

    protected virtual void Update()
    {
        if (!running)
        {
            if (taskUI.gameObject.tag != "PlayerTaskUI")
                taskUI.SetPeopleInLineAmount(0);
            return;
        }

        if (taskUI.gameObject.tag != "PlayerTaskUI")
            taskUI.SetPeopleInLineAmount(peopleInTask);

        float actualTimeToComplete = timeToComplete * timeModifier;

        float mod = Time.deltaTime;
        if (taskUI.gameObject.tag != "PlayerTaskUI")
            mod *= GameState.Instance.globalSpeedModifier;
        counter += mod;
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
        if (taskUI.gameObject.tag != "PlayerTaskUI")
            taskUI.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + uiOffset);
        gameObject.PostNotification(TaskStartNotification, this);

        if (taskType == TaskType.GRABBING_ITEM || taskType == TaskType.RESTOCKING_ITEM)
        {
            Resource resource = gameObject.GetComponent<Resource>();
            timeModifier = Mathf.Lerp(1, 0.2f, (resource.currentAmount / resource.maxAmount));
        }
        else
        {
            timeModifier = 1;
        }
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