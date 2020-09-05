using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private uint roomSize = 16;

    private string roomName;

    private NetworkManager networkManager;
    #endregion

    #region Start Match Maker
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }
    #endregion

    #region Room Name
    public void SetRoomName(string _name)
    {
        roomName = _name;
    }
    #endregion

    #region Create Room
    public void CreateRoom()
    {
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
    #endregion
}
