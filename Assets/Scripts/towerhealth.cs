using UnityEngine;
using UnityEngine.UI;

public class towerhealth : MonoBehaviour
{
    public int MaxhealthTower = 170;
    public int Towerhealth;
    public Image HealthbarTower;

    void Start()
    {
        Towerhealth = MaxhealthTower;
    }

    private void Update()
    {
        
        int clampedHealth = Mathf.Clamp(Towerhealth, 0, MaxhealthTower);
        HealthbarTower.fillAmount = Mathf.Clamp((float)clampedHealth / MaxhealthTower, 0, 1);
    }

    public void TakeDamage(int damage)
    {
        Towerhealth -= damage;
        if (Towerhealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}