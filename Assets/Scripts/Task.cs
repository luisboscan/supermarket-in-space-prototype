using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public const string TaskCompleteNotification = "Tasks.TaskCompleteNotification";
    public const string TaskStartNotification = "Tasks.TaskStartNotification";
    public const string TaskStoppedNotification = "Tasks.TaskStoppedNotification";

    public float timeToComplete = 5f;
    public int priority = 1;
    public LayerMask targetMask;
    public ProgressBar progressBar;

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
        progressBar.SetValue(counter / timeToComplete);
        if (counter >= timeToComplete)
        {
            Debug.Log("Task Finished");
            OnTaskFinished();
            this.PostNotification(Task.TaskCompleteNotification);
        }
    }

    public void StartTask()
    {
        Debug.Log("Task Started");
        counter = 0;
        running = true;
        progressBar.gameObject.SetActive(true);
        progressBar.Reset();
        progressBar.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        this.PostNotification(Task.TaskStartNotification);
    }

    public void StopTask()
    {
        bool interrupted = running;
        OnTaskFinished();
        if (interrupted)
        {
            this.PostNotification(Task.TaskStoppedNotification);
        }
    }

    private void OnTaskFinished()
    {
        progressBar.gameObject.SetActive(false);
        running = false;
    }
}