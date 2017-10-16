using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskHandler : MonoBehaviour {

    public const string TaskCompleteNotification = "TaskHandler.TaskCompleteNotification";
    public const string TaskStartNotification = "TaskHandler.TaskStartNotification";

    private GraphContainer graphContainer;
    private Task currentTask;
    public bool workingOnTask;

	void Start () {
        graphContainer = GameObject.FindGameObjectWithTag("Graph").GetComponent<GraphContainer>();
        this.AddObserver(OnDestinationReached, NodeNavigation.DestinationReachedNotification, gameObject);
        this.AddObserver(OnDestinationChanged, NodeNavigation.DestinationChangedNotification, gameObject);
    }

    void Update()
    {
        if (currentTask != null && !workingOnTask && !currentTask.running)
        {
            currentTask.StartTask();
            workingOnTask = true;
            gameObject.PostNotification(TaskStartNotification, currentTask);
            this.AddObserver(OnTaskCompleted, Task.TaskCompleteNotification, currentTask.gameObject);
        }
    }

    void OnDestinationChanged(object sender, object args)
    {
        if (currentTask != null)
        {
            currentTask.StopTask();
        }
        FreeTask();
    }

    void OnDestinationReached(object sender, object args)
    {
        if (currentTask == null)
        {
            Node node = ((Node)args);
            DestinationNodeContainer destinationNodeContainer = (DestinationNodeContainer)graphContainer.GetNodeContainerForNode(node);

            Task task = destinationNodeContainer.GetNextTaskForObject(gameObject);
            if (task != null)
            {
                currentTask = task;
                currentTask.peopleInTask++;
                GetComponent<NodeNavigation>().enabled = false;
            }
        }
    }

    void OnTaskCompleted(object sender, object args)
    {
        gameObject.PostNotification(TaskCompleteNotification, currentTask);
        FreeTask();
    }

    void FreeTask()
    {
        if (currentTask != null)
        {
            currentTask.peopleInTask--;
        }
        RemoveObservers();
        currentTask = null;
        GetComponent<NodeNavigation>().enabled = true;
        workingOnTask = false;
    }

    private void RemoveObservers()
    {
        if (currentTask != null)
        {
            this.RemoveObserver(OnTaskCompleted, Task.TaskCompleteNotification, currentTask.gameObject);
        }
    }
}
