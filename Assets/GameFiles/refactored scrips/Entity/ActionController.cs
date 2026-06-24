using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionController
{
    private Entity entity;
    public List<ConditionalAction> availableActions;
    private List<ConditionalAction> activeActions;
    private IActionable actionable;

    public ActionController(Entity entity, List<ConditionalAction> actions)
    {
        this.entity = entity;
        availableActions = actions;
        activeActions = new List<ConditionalAction>();
        actionable = entity as IActionable;
    }

    public void Initialize()
    {
        foreach (var action in availableActions)
        {
            List<BaseCondition> conditions = action.conditions;
            foreach (BaseCondition condition in conditions)
            {
                condition.Initialize(entity);
            }
        }
    }

    public void Update()
    {
        CheckForValidActions();
        CheckForCompleteActions();

        foreach (ConditionalAction action in activeActions)
        {
            action.action.UpdateAction();
        }
    }
    public void FixedUpdate()
    {
        foreach (ConditionalAction action in activeActions)
        {
            action.action.FixedUpdateAction();
        }
    }

    public void CheckForValidActions()
    {
        List<ConditionalAction> potentialExclusiveActions = new List<ConditionalAction>();

        for (int i = availableActions.Count - 1; i >= 0; i--)
        {
            ConditionalAction action = availableActions[i];

            if (action.singleUse && action.triggered)
            { 
                availableActions.Remove(action);
                continue;
            }

            List<BaseCondition> conditions = action.conditions;
            bool allRequiredConditionsMet = false;
            bool anyNonRequiredPresent = false;
            int numRequired = 0;
            foreach (BaseCondition condition in conditions)
            {
                condition.ConditionUpdate();

                if (action.allConditionsRequired)
                {
                    numRequired++;
                    allRequiredConditionsMet = true;
                    if (!condition.IsConditionMet())
                    {
                        allRequiredConditionsMet = false;
                        break;
                    }
                }
                else
                {
                    if (condition.IsConditionMet())
                    {
                        anyNonRequiredPresent = true;
                    }
                }
            }          

            if (allRequiredConditionsMet && actionable.canAct)
            {
                if (action.exclusive)
                {
                    potentialExclusiveActions.Add(action);
                }
                else 
                {
                    activeActions.Add(action);
                    availableActions.Remove(action);
                    action.triggered = true;
                    action.action.StartAction(entity);
                }
            }
            else if (anyNonRequiredPresent && !action.allConditionsRequired && actionable.canAct)
            {
                if (action.exclusive)
                {
                    potentialExclusiveActions.Add(action);
                }
                else
                {
                    activeActions.Add(action);
                    availableActions.Remove(action);
                    action.triggered = true;
                    action.action.StartAction(entity);
                }
            }
        }

        //if there are exlcusive actions to choose from and there are no active exclusive actions, choose one to activate.
        if (potentialExclusiveActions.Count <= 0) return;
        foreach (ConditionalAction action in activeActions)
        {
            if (action.exclusive)
            {
                return;
            }
        }

        ActivateExclusiveAction(potentialExclusiveActions);
    }

    private void ActivateExclusiveAction(List<ConditionalAction> potentialActiveActions)
    {
        List<ConditionalAction> lowestPriorityActiveActions = new List<ConditionalAction>();

        for (int i = 0; i < potentialActiveActions.Count; i++)
        {
            ConditionalAction action = potentialActiveActions[i];
            //fill list if empty
            if (lowestPriorityActiveActions.Count <= 0)
            {
                lowestPriorityActiveActions.Add(action);
                continue;
            }
            //add to list if same priority as current lowest
            if (action.priority == lowestPriorityActiveActions[0].priority)
            {
                lowestPriorityActiveActions.Add(action);
            }
            //empty list and add action if its lowest priority so far
            else if (action.priority < lowestPriorityActiveActions[0].priority)
            { 
                lowestPriorityActiveActions.Clear();
                lowestPriorityActiveActions.Add(action);
            }
        }

        //if there is only one lowest priority action, use it.
        ConditionalAction chosenAction = lowestPriorityActiveActions[0];
        //if there are multiple actions, use one at random.
        if (lowestPriorityActiveActions.Count > 1)
        {
            int randomIndex = UnityEngine.Random.Range(0, lowestPriorityActiveActions.Count);
            chosenAction = lowestPriorityActiveActions[randomIndex];
        }

        activeActions.Add(chosenAction);
        availableActions.Remove(chosenAction);
        chosenAction.triggered = true;
        chosenAction.action.StartAction(entity);
    }


    public void CheckForCompleteActions()
    {
        for (int i = activeActions.Count - 1; i >= 0; i--)
        {
            ConditionalAction action = activeActions[i];

            if (action.action.isComplete)
            {
                activeActions.Remove(action);
                action.action.isComplete = false;
                action.ResetConditionsAll();
                availableActions.Add(action);
            }
        }
    }

    public bool CheckForMovementBlockersAction()
    {
        bool result = false;
        for (int i = activeActions.Count - 1; i >= 0; i--)
        {
            if (activeActions[i].action.preventsMovement) { result = true; }
        }
        return result;
    }

    public void InterruptAllActive()
    {
        foreach (ConditionalAction action in activeActions)
        {
            action.action.InterruptAction();
        }
    }
}
