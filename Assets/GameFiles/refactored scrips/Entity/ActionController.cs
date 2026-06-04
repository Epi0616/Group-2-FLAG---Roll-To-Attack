using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering;

public class ActionController
{
    private Entity entity;
    private List<ConditionalAction> availableActions;
    private List<ConditionalAction> activeActions;

    public ActionController(Entity entity, List<ConditionalAction> actions)
    {
        this.entity = entity;
        availableActions = actions;
        activeActions = new List<ConditionalAction>();
    }

    public void Initialize()
    {
        foreach (var action in availableActions)
        {
            List<ICondition> conditions = action.conditions;
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
        for (int i = availableActions.Count - 1; i >= 0; i--)
        {
            ConditionalAction action = availableActions[i];
            List<ICondition> conditions = action.conditions;
            bool allReleventConditionsMet = true;
            foreach (BaseCondition condition in conditions)
            {
                condition.ConditionUpdate();

                if (!condition.IsConditionMet() && condition.isRequired)
                {
                    allReleventConditionsMet = false;
                }
            }

            if (allReleventConditionsMet)
            {
                activeActions.Add(action);
                availableActions.Remove(action);
                action.action.StartAction(entity);
            }
        }
    }

    public void CheckForCompleteActions()
    {
        for (int i = activeActions.Count - 1; i >= 0; i--)
        {
            ConditionalAction action = activeActions[i];

            if (action.action.isComplete)
            {
                activeActions.Remove(action);
                availableActions.Add(action);
            }
        }
    }
}
