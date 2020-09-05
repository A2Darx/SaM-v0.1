using UnityEngine;
using UnityEngine.Networking;

public class PlayerAttack : NetworkBehaviour {

    #region Variables
    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
    #endregion

    #region No Camera Error
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerAttack: No camera referenced");
            this.enabled = false;
        }
    }
    #endregion

    #region Attack
    void Update()
    {
        if (PauseMenu.IsOn)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    [Client]
    void Attack()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask) )
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerHit(_hit.collider.name, weapon.damage);
            }
        }

    }
    #endregion

    #region Hit Detection
    [Command]
    void CmdPlayerHit (string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been hit");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
    #endregion

}
