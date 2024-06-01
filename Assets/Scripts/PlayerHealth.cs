using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float MaxPlayerHealth = 25;
    public float PlayerHP;
    public DeathUI deathUIController;
    public DeathUI deathUIController2;
    public Image Healthbar;
    private Animator animator;
    public bool isAlive = true;

    private float lastDamageTime;
    public float healInterval = 0.1f; // Интервал времени для лечения в секундах
    public float healAmount = 0.2f; // Количество здоровья, которое восстанавливается

    void Start()
    {
        PlayerHP = MaxPlayerHealth;
        animator = GetComponent<Animator>();
        lastDamageTime = Time.time;
        StartCoroutine(AutoHeal());
    }

    private void Update()
    {
        
        float clampedHealth = Mathf.Clamp(PlayerHP, 0, MaxPlayerHealth);
        Healthbar.fillAmount = Mathf.Clamp((float)clampedHealth / MaxPlayerHealth, 0, 1);
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        PlayerHP -= damage;
        lastDamageTime = Time.time; // Сбрасываем время последнего получения урона

        if (PlayerHP <= 0)
        {
            isAlive = false;
            animator.SetTrigger("NoHealth");
            deathUIController.ShowDeathUI();
            deathUIController2.ShowDeathUI();
        }
    }

    public void Heal(float amount)
    {
        PlayerHP += amount;
        if (PlayerHP > MaxPlayerHealth)
        {
            PlayerHP = MaxPlayerHealth;
        }
    }

    private IEnumerator AutoHeal()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(healInterval);

            // Проверяем, прошло ли healInterval времени с последнего получения урона
            if (Time.time - lastDamageTime >= healInterval)
            {
                Heal(healAmount);
            }
        }
    }
}
