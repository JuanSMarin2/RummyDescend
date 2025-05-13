using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthManager : MonoBehaviour
{
    public static UIHealthManager Instance { get; private set; }

    [Header("Jugador")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    public int maxPlayerHealth = 50;

    [Header("Enemigo")]
    [SerializeField] private Image enemyBar;
    [SerializeField] private TextMeshProUGUI enemyText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateHealthUI(GameManager.Instance.playerHealth);
        if (GameData.Instance.lastEnemyData != null)
            UpdateEnemyUI(GameData.Instance.lastEnemyData.health);
    }

    private void Update()
    {
        UpdateHealthUI(GameManager.Instance.playerHealth);

        if (BattleManager.Instance != null)
        {
            var enemy = BattleManager.Instance.GetCurrentEnemy();
            if (enemy != null)
            {
                UpdateEnemyUI(enemy.health);
            }
        }

        GameManager.Instance.playerHealth = GameManager.Instance.playerHealth >= maxPlayerHealth ? maxPlayerHealth : GameManager.Instance.playerHealth;
    }

    public void UpdateHealthUI(int currentHealth)
    {
        float fill = Mathf.Clamp01((float)currentHealth / maxPlayerHealth);
        healthBar.fillAmount = fill;
        healthText.text = $"{currentHealth} / {maxPlayerHealth}";
    }

    public void UpdateEnemyUI(int currentHealth)
    {
        EnemyData enemy = GameData.Instance.lastEnemyData;
        if (enemy == null) return;


        if (!BattleManager.Instance.isEnemyDefeated)
        {
            float fill = Mathf.Clamp01((float)currentHealth / enemy.maxHealth);
            enemyBar.fillAmount = fill;
            enemyText.text = $"{currentHealth} / {enemy.maxHealth}";

        }

        else
        {
            enemyBar.fillAmount = 0;
            enemyText.text = $"Derrotado";
        }
    }
}
