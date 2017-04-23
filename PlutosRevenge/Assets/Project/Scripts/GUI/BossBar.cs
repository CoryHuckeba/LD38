using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BossBar : Singleton<BossBar> {

    // Cached components
    public Text bossName;
    public Text bossDetails;
    public Animator animator;
    public Image fillBar;

    private EnemyPlanet enemy;
    private float targetFill;
    private float actionfill;

    // flag to prevent early triggering
    private bool active = false;

    // ANimation constants
    private const string ANIM_INTRO = "intro";
    private const string ANIM_OUTRO = "outro";

    private void Start()
    {
        Invoke("SetActive", 1f);
    }

    public void SetInformation(EnemyPlanet enemy)
    {
        if (active)
        {
            this.enemy = enemy;
            this.bossName.text = enemy.name;
            this.bossDetails.text = enemy.details;

            enemy.damaged += OnDamaged;

            animator.Play(ANIM_INTRO);
        }
    }

    public void ClearInfo()
    {
        animator.Play(ANIM_OUTRO);
    }

    private void OnDamaged()
    {
        targetFill = enemy.currentHealth / enemy.maxHealth;
    }

    public void TweenFillBar()
    {
        iTween.ValueTo(
            gameObject,
            iTween.Hash(
                "from", 0,
                "to", (enemy.currentHealth / enemy.maxHealth),
                "time", 0.65f,
                "onupdate", "OnTween",
                "easetype", iTween.EaseType.easeOutQuad
            )
        );
    }

    private void OnTween(float value)
    {
        fillBar.fillAmount = value;
    }

    public void ResetFill()
    {
        fillBar.fillAmount = 0f;
    }

    private void SetActive()
    {
        active = true;
    }
}
