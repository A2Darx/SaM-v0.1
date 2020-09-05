using UnityEngine;
using UnityEngine.UI;

public class Insanity : MonoBehaviour {

    #region Variables
    [SerializeField]
    private float insanityTimer = 0.0f;

    private bool insanityActive = false;
    private bool insanityReady = false;
    private bool insanityLoadingUp = false;
    private bool insanityLoadingDown = false;

    [SerializeField]
    private float loadingTimerUp = 3.0f;
    [SerializeField]
    private float loadingTimerDown = 3.0f;

    public GameObject Player;

    public float speed;
    public float maxHealth;
    public float nowHealth;

    public Text InsanityProgress;
    #endregion

    #region Setup
    void Start()
    {
        speed = Player.gameObject.GetComponent<PlayerController>().speed;
        maxHealth = Player.gameObject.GetComponent<Player>().maxHealth;
        nowHealth = Player.gameObject.GetComponent<Player>().currentHealth;
        InsanityProgress.text = "";
    }

    void Update()
    {
        InsanitySetup();
        InsanityTimer();
    }
    #endregion

    #region Activation/Deactivation
    public void InsanitySetup()
    {
        // Activates
        if (Input.GetButtonDown("Insanity") && insanityReady == true)
        {
            insanityLoadingUp = true;
            insanityReady = false;
            Debug.Log("Activating");
        }

        // Deactivates
        if (Input.GetButtonDown("Insanity") && insanityActive == true)
        {
            insanityLoadingDown = true;
            insanityActive = false;
            Debug.Log("Deactivating");
        }
    }
    #endregion

    #region Timers
    public void InsanityTimer()
    {
        #region Insanity Timer
        // If it isnt ready, count the timer up
        if (insanityReady == false && insanityActive == false)
        {
            insanityTimer += Time.deltaTime;
        }

        // If it is active, count the timer down
        if (insanityActive == true)
        {
            insanityTimer -= Time.deltaTime;
        }
        #endregion

        #region Insanity Timer states
        // If the timer is at its maximum, it is ready
        if (insanityTimer > 60)
        {
            insanityReady = true;
            Debug.Log("Ready");
        }

        // If the timer has depleted, it isnt ready
        if (insanityTimer < 0)
        {
            insanityActive = false;
            insanityReady = false;
            Debug.Log("Depleted");
            loadingTimerUp = 3.0f;
            loadingTimerDown = 3.0f;
            insanityTimer = 0.0f;
        }
        #endregion

        #region Loading Timer
        // Timer for activation
        if (insanityLoadingUp == true)
        {
            loadingTimerUp -= Time.deltaTime;
        }

        // Timer for deactivation
        if (insanityLoadingDown == true)
        {
            loadingTimerDown -= Time.deltaTime;
        }
        #endregion

        #region Loading Timer States
        // If up timer is finished, set as active
        if (loadingTimerUp < 0)
        {
            insanityLoadingUp = false;
            insanityActive = true;
            Debug.Log("Activated");
            loadingTimerUp = 3.0f;
        }

        // If down timer is finished, set as inactive
        if (loadingTimerDown < 0)
        {
            insanityLoadingDown = false;
            insanityActive = false;
            Debug.Log("Deactivated");
            loadingTimerDown = 3.0f;
        }
        #endregion
    }
    #endregion

    #region Insanity Stats
    public void InsanityStats()
    {
        if (insanityActive == true)
        {
            speed = speed * 1.25f;
            maxHealth = maxHealth * 1.2f;
            nowHealth = nowHealth * 1.2f;
        }

        if (insanityLoadingUp == true)
        {
            speed = speed / 1.5f;
        }

        if (insanityLoadingDown == true)
        {
            speed = speed / 1.5f;
        }
    }
    #endregion

    #region Text Notifications
    public void InsanityText ()
    {
        if (insanityReady == false)
        {
            InsanityProgress.text = "Insanity: Not Ready (" + insanityTimer + ")";
        }

        if (insanityReady == true && insanityActive == false)
        {
            InsanityProgress.text = "Insanity: Ready (Right Click to use)";
        }

        if (insanityActive == true)
        {
            InsanityProgress.text = "Insanity: Active (" + insanityTimer + ")";
        }
    }
    #endregion

}
