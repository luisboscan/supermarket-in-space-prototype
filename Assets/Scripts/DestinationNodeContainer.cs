using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationNodeContainer : NodeContainer {

    private Task[] tasks;
    private Task currentTask;

    void Start()
    {
        tasks = GetComponents<Task>();
        Array.Sort(tasks, delegate (Task x, Task y) { return x.priority.CompareTo(y.priority); });

        foreach (Task task in tasks)
        {
            task.AddObserver(OnTaskStart, Task.TaskStartNotification);
            task.AddObserver(OnTaskComplete, Task.TaskCompleteNotification);
            task.AddObserver(OnTaskComplete, Task.TaskStoppedNotification);
        }
    }

    void OnTaskStart(object sender, object args)
    {
        currentTask = (Task) sender;
    }

    void OnTaskComplete(object sender, object args)
    {
        currentTask = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    public bool IsTaskActive()
    {
        return currentTask != null;
    }

    public Task GetNextTaskForObject(GameObject gameObject)
    {
        foreach (Task task in tasks)
        {
            if (task.isActiveAndEnabled && task.IsTaskAssignable(gameObject))
            {
                return task;
            }
        }
        return null;
    }
}