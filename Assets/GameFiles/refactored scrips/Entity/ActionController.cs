using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class ActionController : MonoBehaviour
{
    public GameObject obj;
    private Entity entity;
    private List<IAction> actions;

    public void Initialize(Entity entity, List<IAction> actions)
    {
        this.entity = entity;
        this.actions = actions;
    }

    public void UpdateActions()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].UpdateAction();
            //perform various checks to see if an action needs to be interrupted/ended
        }
    }
    public void InterruptAction()
    {

    }
    public void EndAction()
    {

    }
}
