using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Button _btnMaxHealth;
    [SerializeField] private Button _btnHealthRegeneration;
    [SerializeField] private Button _btnAttackPower;
    [SerializeField] private Button _btnAttackSpeed;
    [SerializeField] private Button _btnCriticalChance;

    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _btnMaxHealthText;
    [SerializeField] private TextMeshProUGUI _btnHealthRegenerationText;
    [SerializeField] private TextMeshProUGUI _btnAttackPowerText;
    [SerializeField] private TextMeshProUGUI _btnAttackSpeedText;
    [SerializeField] private TextMeshProUGUI _btnCriticalChanceText;

    [SerializeField] private TextMeshProUGUI _maxHealthText;
    [SerializeField] private TextMeshProUGUI _healthRegenerationText;
    [SerializeField] private TextMeshProUGUI _attackPowerText;
    [SerializeField] private TextMeshProUGUI _attackSpeedText;
    [SerializeField] private TextMeshProUGUI _criticalChanceText;

    [SerializeField] private GameObject _failedField;
    [SerializeField] private Button _btnRestart;
    [SerializeField] private string _nameStartScene = "MainScene";

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _delayCheck = 0.2f;

    private HealthProperty _healthProperty;
    private RegenerationProperty _regenerationProperty;
    private AttackPowerProperty _attackPowerProperty;
    private AttackSpeedProperty _attackSpeedProperty;
    private CriticalChanceProperty _criticalChanceProperty;

    private void Start()
    {
        GameInfo.IsGameOver = false;
        _healthProperty = _gameManager.GetMaxHealthProperty();
        _regenerationProperty = _gameManager.GetRegenerationProperty();
        _attackPowerProperty = _gameManager.GetAttackPowerProperty();
        _attackSpeedProperty = _gameManager.GetAttackSpeedProperty();
        _criticalChanceProperty = _gameManager.GetCriticalChanceProperty();

        _btnMaxHealth.onClick.AddListener(UpgradeMaxHealth);
        _btnHealthRegeneration.onClick.AddListener(UpgradeRegeneration);
        _btnAttackPower.onClick.AddListener(UpgradeAttackPower);
        _btnAttackSpeed.onClick.AddListener(UpgradeAttackHealth);
        _btnCriticalChance.onClick.AddListener(UpgradeCriticalChance);


        _btnRestart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(_nameStartScene);
        });

        _gameManager.OnLevelChange += (level) => { _levelText.text = $"Level: {level}"; };
        _levelText.text = $"Level: 0";

        StartCoroutine(CheckUpdateProperty());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator CheckUpdateProperty()
    {
        while (true)
        {
            if (GameInfo.IsGameOver)
            {
                _failedField.SetActive(true);
                yield break;
            }
            int coins = _gameManager.GetPlayerCoin();
            _coinsText.text = $"Coins: {coins}";

            //Health Property
            _btnMaxHealth.interactable = coins >= _healthProperty.Price;
            _btnMaxHealthText.text = $"Upgrade {_healthProperty.Price}C";
            _maxHealthText.text = $"Max Health: {_healthProperty}";
            //Regeneration Property
            _btnHealthRegeneration.interactable = coins >= _regenerationProperty.Price;
            _btnHealthRegenerationText.text = $"Upgrade {_regenerationProperty.Price}C";
            _healthRegenerationText.text = $"Regeneraion: {_regenerationProperty}";
            //Attack power Property
            _btnAttackPower.interactable = coins >= _attackPowerProperty.Price;
            _btnAttackPowerText.text = $"Upgrade {_attackPowerProperty.Price}C";
            _attackPowerText.text = $"Attack Power: {_attackPowerProperty}";
            //Attack spped Property
            _btnAttackSpeed.interactable = coins >= _attackSpeedProperty.Price;
            _btnAttackSpeedText.text = $"Upgrade {_attackSpeedProperty.Price}C";
            _attackSpeedText.text = $"Attack Speed: {_attackSpeedProperty}";
            //CricicalChance Property
            _btnCriticalChance.interactable = coins >= _criticalChanceProperty.Price;
            _btnCriticalChanceText.text = $"Upgrade {_criticalChanceProperty.Price}C";
            _criticalChanceText.text = $"Cricical Chance: {_criticalChanceProperty}";

            yield return new WaitForSeconds(_delayCheck);
        }
    }

    public void UpgradeMaxHealth()
    {
        _gameManager.RemoveCoin(_healthProperty.Price);
        _healthProperty.Upgrade();
        _gameManager.UpdateMaxPlayerHealth();
    }

    public void UpgradeRegeneration()
    {
        _gameManager.RemoveCoin(_regenerationProperty.Price);
        _regenerationProperty.Upgrade();
    }

    public void UpgradeAttackPower()
    {
        _gameManager.RemoveCoin(_attackPowerProperty.Price);
        _attackPowerProperty.Upgrade();
    }

    public void UpgradeAttackHealth()
    {
        _gameManager.RemoveCoin(_attackSpeedProperty.Price);
        _attackSpeedProperty.Upgrade();
    }

    public void UpgradeCriticalChance()
    {
        _gameManager.RemoveCoin(_criticalChanceProperty.Price);
        _criticalChanceProperty.Upgrade();
    }
}
