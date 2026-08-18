using UnityEngine;

public class SpiderWebConnection
{
    public SpiderWebNode NodeA;
    public SpiderWebNode NodeB;
    public WebConnectionVisual visual;
    public SpiderWebConnection(SpiderWebNode nodeA, SpiderWebNode nodeB) {  NodeA = nodeA; NodeB = nodeB; }

    public SpiderWebConnection(SpiderWebNode nodeA, SpiderWebNode nodeB, WebConnectionVisual visual)
    { 
        NodeA = nodeA; NodeB = nodeB; 
        this.visual = visual;
        this.visual.SetConnectionRenderer(this);
    }
}
