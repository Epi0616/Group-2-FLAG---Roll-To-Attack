using UnityEngine;
using System.Collections.Generic;

public class MovementController
{
    private Entity entity;
    private IMoveable moveInterfaceAccess;
    public List<ConditionalMovement> availableMovements;
    private List<ConditionalMovement> activeMovements;

    public MovementController(Entity entity, List<ConditionalMovement> startMovements)
    {
        this.entity = entity;
        moveInterfaceAccess = entity as IMoveable;
        availableMovements = startMovements;
        activeMovements = new List<ConditionalMovement>();
    }

    public void Initialize()
    {
        foreach (var movement in availableMovements)
        {
            List<BaseCondition> conditions = movement.conditions;
            foreach (BaseCondition condition in conditions)
            {
                condition.Initialize(entity);
            }
        }
    }

    public void Update()
    {
        CheckForValidMovements();
        CheckForInvalidMovements();

        foreach (ConditionalMovement movement in activeMovements)
        {
            movement.movement.UpdateMovement();
        }

    }
    public void FixedUpdate()
    {

    }

    public void CheckForValidMovements()
    {
        for (int i = availableMovements.Count - 1; i >= 0; i--)
        {
            ConditionalMovement movement = availableMovements[i];
            List<BaseCondition> conditions = movement.conditions;
            bool allRequiredConditionsMet = false;
            bool anyNonRequiredPresent = false;
            foreach (BaseCondition condition in conditions)
            {
                condition.ConditionUpdate();

                if (movement.allConditionsRequired)
                {
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

            if (allRequiredConditionsMet && moveInterfaceAccess.canMove)
            {
                activeMovements.Add(movement);
                availableMovements.Remove(movement);
                movement.movement.StartMovement(entity);
            }
            else if (anyNonRequiredPresent && !movement.allConditionsRequired && moveInterfaceAccess.canMove)
            {
                activeMovements.Add(movement);
                availableMovements.Remove(movement);
                movement.movement.StartMovement(entity);
            }

        }
    }

    // Old Version

    //for (int i = availableMovements.Count - 1; i >= 0; i--)
    //    {
    //        ConditionalMovement movement = availableMovements[i];
    //        List<BaseCondition> conditions = movement.conditions;
    //        bool allReleventConditionsMet = true;
    //        foreach (BaseCondition condition in conditions)
    //        {
    //            condition.ConditionUpdate();

    //            if (!condition.IsConditionMet() && condition.isRequired)
    //            {
    //                allReleventConditionsMet = false;
    //            }
    //        }

    //        if (allReleventConditionsMet && moveInterfaceAccess.canMove)
    //        {
    //            activeMovements.Add(movement);
    //            availableMovements.Remove(movement);
    //            movement.movement.StartMovement(entity);
    //      }
    //}


    public void CheckForInvalidMovements()
    {
        for (int i = activeMovements.Count - 1; i >= 0; i--)
        {
            ConditionalMovement movement = activeMovements[i];
            List<BaseCondition> conditions = movement.conditions;
            bool allRequiredConditionsMet = false;
            bool anyNonRequiredPresent = false;
            foreach (BaseCondition condition in conditions)
            {
                condition.ConditionUpdate();

                allRequiredConditionsMet = true;

                if (movement.allConditionsRequired)
                {

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

            if (!allRequiredConditionsMet && movement.allConditionsRequired)
            {
                availableMovements.Add(movement);
                activeMovements.Remove(movement);
                movement.movement.EndMovement();
                movement.ResetConditionsAll();
            }
            else if (!anyNonRequiredPresent && !movement.allConditionsRequired)
            {
                availableMovements.Add(movement);
                activeMovements.Remove(movement);
                movement.movement.EndMovement();
                movement.ResetConditionsAll();
            }
        }
    }
}

    // Old Version

//    for (int i = activeMovements.Count - 1; i >= 0; i--)
//    {
//    ConditionalMovement movement = activeMovements[i];
//    List<BaseCondition> conditions = movement.conditions;
//    bool allReleventConditionsMet = true;

//    foreach (BaseCondition condition in conditions)
//    {
//        condition.ConditionUpdate();

//        if (!condition.IsConditionMet() && condition.isRequired)
//        {
//            allReleventConditionsMet = false;
//        }
//    }

//    if (!allReleventConditionsMet)
//    {
//        availableMovements.Add(movement);
//        activeMovements.Remove(movement);
//        movement.movement.EndMovement();
//    }
//    }


