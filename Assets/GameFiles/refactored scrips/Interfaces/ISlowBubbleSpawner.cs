using UnityEngine;

public interface ISlowBubbleSpawner
{ 
    public GameObject slowBubblePrefab { get; set; }
    public EnhancedSlowingBubble currentBubbleInstance { get; set; }
}
    

