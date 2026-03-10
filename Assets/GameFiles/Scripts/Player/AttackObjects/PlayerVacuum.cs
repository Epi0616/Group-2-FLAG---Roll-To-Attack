using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVacuum : MonoBehaviour
{
    [SerializeField] GameObject temporaryImpactField;
    private Color red = Color.red, blue = Color.blue;
    private Material material;
    private float timer = 2f, range;
    private bool detonated = false, activated = false, toggle = false;


    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (!activated) return;

        timer -= Time.deltaTime;

        if (timer <= 0 && !detonated)
        {
            OnVacuum();
            detonated = true;
        }
    }

    public void SetUp(float range, float timer)
    {
        this.range = range;
        this.timer = timer;
        activated = true;
        ShowRange();
        StartCoroutine(FlashRoutine());
    }

    private void OnVacuum()
    {
        List<EnemyStateController> enemies = GetEnemiesInRange();

        foreach (EnemyStateController enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.OnTakeKnockback(transform.position, -10);
                enemy.OnTakeDamage(20, Color.blue);
            }
        }
        DestroyMe();
    }

    private List<EnemyStateController> GetEnemiesInRange()
    {
        List<EnemyStateController> enemies = new();
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (var collider in colliders)
        {
            if (!collider.gameObject) { continue; }

            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(collider.gameObject.GetComponent<EnemyStateController>());
            }
        }

        return enemies;
    }

    private void DestroyMe()
    { 
        Destroy(gameObject);
    }

    private void ShowRange()
    {
        GameObject rangeDisplay = Instantiate(temporaryImpactField, transform.position, Quaternion.identity);
        rangeDisplay.GetComponent<TemporaryImpactField>().adjustObject(range, 0.25f, 0.15f, timer);
    }

    private IEnumerator FlashRoutine()
    {
        while (timer > 0)
        {
            toggle = !toggle;
            material.color = toggle ? red : blue;

            float t = 1 - Mathf.Clamp01(timer / 2f);
            float interval = Mathf.Lerp(0.5f, 0.05f, t);

            yield return new WaitForSeconds(interval);
        }
    }
}
