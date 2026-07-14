using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

    public void Reset()
    {
        //foreach (var movement in availableMovements)
        //{
        //    List<BaseCondition> conditions = movement.conditions;
        //    foreach (BaseCondition condition in conditions)
        //    {
        //        condition.ResetCondition();
        //    }
        //}
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
        foreach (ConditionalMovement movement in activeMovements)
        {
            movement.movement.FixedUpdateMovement();
        }
    }

    public void CheckForValidMovements()
    {
        if (!moveInterfaceAccess.canMove) return;

        List<ConditionalMovement> potentialExclusiveMovements = new List<ConditionalMovement>();

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
                if (movement.exclusive)
                {
                    potentialExclusiveMovements.Add(movement);
                }
                else 
                {
                    activeMovements.Add(movement);
                    availableMovements.Remove(movement);
                    movement.movement.StartMovement(entity);
                }

            }
            else if (anyNonRequiredPresent && !movement.allConditionsRequired && moveInterfaceAccess.canMove)
            {
                if (movement.exclusive)
                {
                    potentialExclusiveMovements.Add(movement);
                }
                else
                {
                    activeMovements.Add(movement);
                    availableMovements.Remove(movement);
                    movement.movement.StartMovement(entity);
                }
            }

        }

        //if there are exlcusive actions to choose from and there are no active exclusive actions, choose one to activate.
        if (potentialExclusiveMovements.Count <= 0) return;

        ActivateExclusiveMovement(potentialExclusiveMovements);
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

    private void ActivateExclusiveMovement(List<ConditionalMovement> potentialActiveMovements)
    {
        List<ConditionalMovement> lowestPriorityActiveMovements = new List<ConditionalMovement>();

        for (int i = 0; i < potentialActiveMovements.Count; i++)
        {
            ConditionalMovement movement = potentialActiveMovements[i];
            //fill list if empty
            if (lowestPriorityActiveMovements.Count <= 0)
            {
                lowestPriorityActiveMovements.Add(movement);
                continue;
            }
            //add to list if same priority as current lowest
            if (movement.priority == lowestPriorityActiveMovements[0].priority)
            {
                lowestPriorityActiveMovements.Add(movement);
            }
            //empty list and add action if its lowest priority so far
            else if (movement.priority < lowestPriorityActiveMovements[0].priority)
            {
                lowestPriorityActiveMovements.Clear();
                lowestPriorityActiveMovements.Add(movement);
            }
        }

        //if there is only one lowest priority movement, use it.
        ConditionalMovement chosenMovement = lowestPriorityActiveMovements[0];
        //if there are multiple movements, use one at random.
        if (lowestPriorityActiveMovements.Count > 1)
        {
            int randomIndex = UnityEngine.Random.Range(0, lowestPriorityActiveMovements.Count);
            chosenMovement = lowestPriorityActiveMovements[randomIndex];
        }

        ConditionalMovement currentExclusiveMovement = null;
        if (GetCurrentExclusiveMovment(out currentExclusiveMovement))
        {
            if (currentExclusiveMovement.priority <= chosenMovement.priority)
            {
                return;
            }
            else 
            {
                availableMovements.Add(currentExclusiveMovement);
                activeMovements.Remove(currentExclusiveMovement);
                currentExclusiveMovement.movement.EndMovement();
                currentExclusiveMovement.ResetConditionsAll();
            }
        }

        activeMovements.Add(chosenMovement);
        availableMovements.Remove(chosenMovement);
        chosenMovement.movement.StartMovement(entity);
    }

    private bool GetCurrentExclusiveMovment(out ConditionalMovement currentExclusive)
    {
        currentExclusive = null;
        for (int i = 0; i < activeMovements.Count; i++)
        {
            if (activeMovements[i].exclusive)
            { 
                currentExclusive = activeMovements[i];
                return true;
            }
        }

        return false;
    }

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


