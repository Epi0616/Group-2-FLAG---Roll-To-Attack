using System.Collections;
using UnityEngine;

public class PlayerBodySystem : EntityBodySystem
{
    public ParticleSystem chargeCompleteEffect;
    public ParticleSystem chargingEffect;

    private float iTimer = 0f;

    private void OnEnable()
    {
        PlayerHealthSystem.ShowIFrames += DisplayIFrames;
    }

    private void OnDisable()
    {
        PlayerHealthSystem.ShowIFrames -= DisplayIFrames;
    }

    public override void InitialiseSystem(Entity entity)
    {
        base.InitialiseSystem(entity);
        chargeCompleteEffect.Stop();
        chargingEffect.Stop();
    }

    public override void ResetSystem()
    {
        base.ResetSystem();
    }

    public void DisplayChargingEffect()
    {
        if (chargingEffect.isPlaying) { return; }
        chargingEffect.Play();
    }

    public void DisplayChargeCompleteEffect()
    {
        if (chargeCompleteEffect.isPlaying) { return; }
        chargeCompleteEffect.Play();
    }

    public void ResetChargingEffects()
    {
        if (chargingEffect == null || chargeCompleteEffect == null) { return; }
        if (chargingEffect.isPlaying)
        {
            chargingEffect.Stop();
        }
        if (chargeCompleteEffect.isPlaying)
        {
            chargeCompleteEffect.Stop();
        }
    }

    private void DisplayIFrames(float iTime)
    {
        iTimer = iTime;
        StartCoroutine(CountDownITime());
        StartCoroutine(IFrameFlashRoutine());
    }

    private IEnumerator CountDownITime()
    {
        while (iTimer > 0)
        { 
            iTimer -= Time.deltaTime;

            yield return null;
        }

        iTimer = 0;
    }

    private IEnumerator IFrameFlashRoutine()
    {
        //Debug.Log("starting corotuine");
        bool toggle = false;
        while (iTimer > 0)
        {
            toggle = !toggle;
            body.SetActive(toggle);

            float t = 1 - Mathf.Clamp01(iTimer / 2f);
            float interval = Mathf.Lerp(0.25f, 0.05f, t);

            yield return new WaitForSeconds(interval);
        }
        body.SetActive(true);
    }
}
