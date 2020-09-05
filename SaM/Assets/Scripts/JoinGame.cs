using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class JoinGame : MonoBehaviour {

    #region Variables
    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    private NetworkManager networkManager;
    #endregion

    #region Start Match Maker + Refresh List
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }
    #endregion

    #region Refresh List
    public void RefreshRoomList()
    {
        ClearRoomList();

        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        status.text = "Loading...";
    }
    #endregion

    #region Match List Settings
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        status.text = "";

        if (matches == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }

        foreach (MatchInfoSnapshot match in matches) // Note: When Brackeys says MatchDesc, use MatchInfoSnapshot.
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGO);
        }

        if (roomList.Count == 0)
        {
            status.text = "No rooms at the moment.";
        }
    }
    #endregion

    #region Clear Room List
    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }
    #endregion

    #region Join Game
    public void JoinRoom (MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin ()
    {
        ClearRoomList();
        
        int countdown = 10;
        while (countdown > 0)
        {
            status.text = "Joining...(" + countdown + ")";

            yield return new WaitForSeconds(1);

            countdown--;
        }

        // Failed to connect
        status.text = "Failed to connect";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = networkManager.matchInfo;
        if (matchInfo != null)
        {
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
        
        RefreshRoomList();
    }
    #endregion
}
