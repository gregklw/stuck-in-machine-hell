using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private BusEventBinding<ObjectiveCompleteEventWrapper> _objectiveCompleteBinding;
    private List<IObjective> _objectives;
    private IObjective _objective;
    private void Awake()
    {
        //consider using script execution orders in order to prevent search on excessive game objects
        _objectives = UnityUtils.FindInterfacesInScene<IObjective>(gameObject.scene);
        Debug.Log($"Objective Count: {_objectives.Count}");
        _objectiveCompleteBinding = new BusEventBinding<ObjectiveCompleteEventWrapper>(AreAllObjectivesComplete);
    }

    private void OnEnable()
    {
        EventBus<ObjectiveCompleteEventWrapper>.Register(_objectiveCompleteBinding);
    }

    private void OnDisable()
    {
        EventBus<ObjectiveCompleteEventWrapper>.Deregister(_objectiveCompleteBinding);
    }

    private void AreAllObjectivesComplete(ObjectiveCompleteEventWrapper objectiveCompleteEvent)
    {
        bool assertCompleted = true;
        _objectives.ForEach(x => assertCompleted = x.IsComplete);
        Debug.Log($"Objectives complete: {assertCompleted}");
        if (assertCompleted)
        {
            EventBus<LevelProgressEventWrapper>.Raise(new LevelProgressEventWrapper());
            //Destroy(gameObject);
        }
    }
}
