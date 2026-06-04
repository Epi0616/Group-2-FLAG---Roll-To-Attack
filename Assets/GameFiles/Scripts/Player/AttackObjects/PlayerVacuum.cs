using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVacuum : MonoBehaviour
{
    [SerializeField] GameObject temporaryImpactField;
    [SerializeField] private LayerMask enemyLayer;
    public AudioClip[] mineSpawned;
    public AudioClip[] mineDetonated;
    private float timer = 2f, range;
    private bool detonated = false; //, toggle = false;

    public void SetUp(float range, float timer)
    {
        detonated = false;
        this.range = range;
        this.timer = timer;
        ShowRange();
        AudioManager.instance.PlayRandomSoundClip(mineSpawned, new Vector3(0, 0, 0), 1.0f);
        StartCoroutine(CountDown());
    }

    private void OnVacuum()
    {
        

        List<EnemyStateController> enemies = GetEnemiesInRange();

        foreach (EnemyStateController enemy in enemies)
        {
            if (enemy != null)
            {
                //enemy.OnTakeKnockback(transform.position, -10);
                enemy.OnRecieveEffect(new ActiveStatusEffect(new VacuumDisplacementEffect(transform.position, -17f),
                new List<BaseCondition> { new oldGroundedCondition(true, enemy), new DurationCondition(true, 0.75f) }));
                enemy.OnTakeDamage(20, Color.blue);
            }
        }

        DestroyMe();
    }

    private List<EnemyStateController> GetEnemiesInRange()
    {
        List<EnemyStateController> enemies = new();
        Collider[] colliders = new Collider[100];
        int collisions = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, enemyLayer);

        for (int i = 0; i < collisions; i++)
        {
            if (!colliders[i].gameObject) { continue; }

            if (colliders[i].gameObject.CompareTag("Enemy"))
            {
                enemies.Add(colliders[i].GetComponent<EnemyStateController>());
            }
        }

        return enemies;
    }

    private void DestroyMe()
    { 
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    private void ShowRange()
    {
        //GameObject rangeDisplay = Instantiate(temporaryImpactField, transform.position, Quaternion.identity);
        GameObject rangeDisplay = ObjectPoolManager.SpawnObject(temporaryImpactField, transform.position, Quaternion.identity);

        rangeDisplay.GetComponent<TemporaryImpactField>().adjustObject(range, 0.25f, 0.15f, timer);
    }

    private IEnumerator CountDown()
    {
        bool hasPlayedSFX = false;
        while (timer > 0 && !detonated)
        {
            timer -= Time.deltaTime;
            if (timer < 0.055f && !hasPlayedSFX)
            {
                AudioManager.instance.PlayRandomSoundClip(mineDetonated, new Vector3(0, 0, 0), 1f);
                hasPlayedSFX = true;
            }

            yield return null;
        }
        
        OnVacuum();
        detonated = true;
    }

    //private IEnumerator FlashRoutine()
    //{
    //    while (timer > 0)
    //    {
    //        toggle = !toggle;
    //        material.color = toggle ? red : blue;

    //        float t = 1 - Mathf.Clamp01(timer / 2f);
    //        float interval = Mathf.Lerp(0.5f, 0.05f, t);

    //        yield return new WaitForSeconds(interval);
    //    }
    //}
}
