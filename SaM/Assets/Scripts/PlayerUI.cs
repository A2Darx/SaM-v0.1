using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    private Player player;

    public void SetPlayer(Player _player)
    {
        player = _player;
    }
    
	void Start ()
	{
		PauseMenu.IsOn = false;
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseMenu();
            Debug.Log("Escape Key Pressed");
		}
	}

	public void TogglePauseMenu ()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
    }
}
