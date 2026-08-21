using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    public GameObject milestoneObj;
    public Image background, healthBar, armourBar;
    private BaseBossEnemy boss;
    private List<MilestoneInfo> currentMilestones = new List<MilestoneInfo>();

    private void OnEnable()
    {
        BaseBossEnemy.BossEnable += HandleBossActive;
        BaseBossEnemy.BossDisable += HandleBossInactive;
    }

    private void OnDisable()
    {
        BaseBossEnemy.BossEnable -= HandleBossActive;
        BaseBossEnemy.BossDisable -= HandleBossInactive;
    }

    private void HandleBossActive(BaseBossEnemy bossEnemy)
    {
        boss = bossEnemy;
        ToggleVisibilty(true);
        bossEnemy.SetMilestones += HandleSetMilestones;
        bossEnemy.UpdateHealth += HandleHealthUpdate;
        bossEnemy.UpdateShields += HandleShieldUpdate;
    }

    private void HandleBossInactive(BaseBossEnemy bossEnemy)
    {
        ToggleVisibilty(false);
        bossEnemy.SetMilestones -= HandleSetMilestones;
        bossEnemy.UpdateHealth -= HandleHealthUpdate;
        bossEnemy.UpdateShields -= HandleShieldUpdate;
        boss = null;
    }

    private void HandleSetMilestones(List<MilestoneDescriptor> milestones)
    {
        currentMilestones.Clear();

        float total = healthBar.rectTransform.rect.width;
        float start = -(total / 2);

        Vector2 healthBarPosition = healthBar.rectTransform.anchoredPosition;

        for (int i = 0; i < milestones.Count; i++)
        {
            MilestoneInfo currentMilestone = milestones[i].Create();
            GameObject newMilestoneObj = Instantiate(currentMilestone.milestoneObj, transform.position, Quaternion.identity, transform);
            Milestone milestone = newMilestoneObj.GetComponent<Milestone>();
            RectTransform milestoneRect = newMilestoneObj.GetComponent<RectTransform>();
            Vector2 temp = healthBarPosition;
            temp.x = start + total * currentMilestone.healthMilestone;
            temp.y -= 8;
            milestoneRect.anchoredPosition = temp;

            currentMilestones.Add(currentMilestone);
            currentMilestones[i].milestone = milestone;
        }
    }

    private void HandleHealthUpdate()
    {
        float healthPercentage = (float)boss.healthSystem.currentHealth / (float)boss.healthSystem.maxHealth;
        healthBar.fillAmount = healthPercentage;
        UpdateMilestones(healthPercentage);
    }

    private void HandleShieldUpdate()
    { 
        armourBar.fillAmount = (float)boss.currentShieldStacks / (float)boss.initialShieldStacks;
    }

    private void UpdateMilestones(float currentHealthPercentage)
    {
        for (int i = currentMilestones.Count - 1; i >= 0; i--)
        {
            MilestoneInfo milestone = currentMilestones[i];
            if (milestone.healthMilestone >= currentHealthPercentage)
            {
                milestone.ActivateMilestone();
                currentMilestones.Remove(milestone);
            }
        }
    }

    private void ToggleVisibilty(bool visible)
    {
        healthBar.gameObject.SetActive(visible);
        armourBar.gameObject.SetActive(visible);
        background.gameObject.SetActive(visible);
    }
}
