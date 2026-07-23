using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Nest : Entity, ICreatureSpawner, IActionable
{
    [SerializeField] private ActionController ActionController;
    public ActionController actionController { get; set; }
    [SerializeField] private bool CanAct;
    public bool canAct { get; set; }
    [SerializeField] private List<ConditionalAction> ConditionalActions;
    public List<ConditionalAction> conditionalActions { get; set; }
    public void CheckForCanAct()
    {
        canAct = !statusSystem.CheckForActionBlockersStatus();
        if (!canAct)
        {
            actionController.InterruptAllActive();
        }
    }
    


    [SerializeField] private GameObject CreaturePrefab;
    public GameObject creaturePrefab { get => CreaturePrefab; set => CreaturePrefab = value; }
}
