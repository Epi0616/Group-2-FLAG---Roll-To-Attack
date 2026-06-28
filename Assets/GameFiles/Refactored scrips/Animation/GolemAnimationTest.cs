using System.Collections;
using UnityEngine;

public class GolemAnimationTest : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        StartCoroutine(LoopThroughAnimations());
    }

    private IEnumerator LoopThroughAnimations()
    {
        while (true)
        {
            animator.CrossFade("WakeUp", 0.5f);
            yield return new WaitForSeconds(5);
            animator.CrossFade("Waddle", 0.2f);
            yield return new WaitForSeconds(5);
            animator.CrossFade("Attack", 0.2f);
            //animator.Play("Attack");
            yield return new WaitForSeconds(5);
            animator.CrossFade("Death", 0.5f);
            yield return new WaitForSeconds(5);

        }
    }
}
