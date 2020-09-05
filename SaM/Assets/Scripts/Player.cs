using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

    #region Variables
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    #region Player Stats
    [SerializeField]
    public int maxHealth = 45;

    [SyncVar]
    public int currentHealth;
    #endregion

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;
    #endregion

    #region Setup
    public void Setup ()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
    }
    #endregion

    #region Damage
    [ClientRpc]
    public void RpcTakeDamage (int _amount)
    {
        if (isDead)
        return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    #endregion

    #region Dying
    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + " fucking died");

        StartCoroutine(Respawn());
    }
    #endregion

    #region Respawn
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " respawed");
    }
    #endregion

    #region Default Stats
    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }
    #endregion
}
