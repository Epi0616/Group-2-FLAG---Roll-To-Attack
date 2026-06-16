using System.Collections.Generic;
using UnityEngine;

public interface IConditionalActionDescriptor
{
    public IAction action { get; set; }
    public List<ICondition> conditions { get; set; }

    public ConditionalAction Create(List<ICondition> condiitons, IAction actions);
}
