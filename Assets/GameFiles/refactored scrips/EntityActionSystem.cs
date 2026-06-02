using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public void StartAction();
    public void UpdateAction();
    public void InterruptAction();
    public void EndAction();
}

[Serializable]
public class BaseEntityAction : IAction
{
    public int variable = 1;
    public void StartAction()
    {
    }
    public void UpdateAction()
    {
    }
    public void InterruptAction()
    {
    }
    public void EndAction()
    {
    }   
}

[Serializable]
public class ConditionalAction
{
    [SerializeReference, SubclassSelector]
    public BaseEntityAction action;
    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    //public ConditionalAction() { }

    public ConditionalAction(BaseEntityAction action, List<BaseCondition> conditions)
    { 
        this.action = action;
        this.conditions = conditions;
    }
}

public interface IConditionalActionDescriptor
{ 
    public BaseEntityAction action { get; set; }
    public List<BaseCondition> conditions { get; set; }

    public ConditionalAction Create(List<BaseCondition> condiitons, BaseEntityAction actions);
}

public class ConditionalActionDescriptor : ScriptableObject
{
    public int variable = 1;

    [SerializeReference, SubclassSelector]
    public BaseEntityAction action;

    [SerializeReference, SubclassSelector]
    public List<BaseCondition> conditions;

    public ConditionalAction Create(List<BaseCondition> condiitons, BaseEntityAction actions)
    {
        return new ConditionalAction(action, conditions);
    }
}
