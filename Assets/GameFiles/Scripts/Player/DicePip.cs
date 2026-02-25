using System;
using UnityEngine;

public struct DicePip
{
    public int pipNumber;
    public int weight;
    public Func<PlayerBaseState> createState;

    public DicePip(int pipNumber, Func<PlayerBaseState> createState)
    {
        this.pipNumber = pipNumber;
        this.weight = 1;
        this.createState = createState;
    }
}

//public PlayerStateFactory stateFactory;

//public PlayerBaseState CreateState()
//{
//    return stateFactory.Create();
//}

//public 

