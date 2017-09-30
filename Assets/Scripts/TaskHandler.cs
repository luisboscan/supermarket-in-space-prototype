using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskHandler : MonoBehaviour {

    private GraphContainer graphContainer;
    private Task currentTask;

	void Start () {
        graphContainer = GameObject.FindGameObjectWithTag("Graph").GetComponent<GraphContainer>();
        this.AddObserver(OnDestinationReached, NodeNavigation.DestinationReachedNotification);
	}
	
	void OnDestinationReached(object sender, object args)
    {
        Node node = ((Node)args);
        DestinationNodeContainer destinationNodeContainer = (DestinationNodeContainer) graphContainer.GetNodeContainerForNode(node);

        Task task = destinationNodeContainer.GetNextTaskForObject(gameObject);
        if (task != null)
        {
            task.AddObserver(OnTaskCompleted, Task.TaskCompleteNotification);
            task.StartTask();
            GetComponent<NodeNavigation>().enabled = false;
            currentTask = task;
        }
    }

    void OnTaskCompleted(object sender, object args)
    {
        currentTask.RemoveObserver(OnTaskCompleted, Task.TaskCompleteNotification);
        currentTask = null;
        GetComponent<NodeNavigation>().enabled = true;
    }
}
