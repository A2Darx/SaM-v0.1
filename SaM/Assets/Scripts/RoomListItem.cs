using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {

    #region Variables
    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomNameText;

    private MatchInfoSnapshot match;
    #endregion

    #region Room List Setup
    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
    {
        match = _match;
        joinRoomCallback = _joinRoomCallback;

        roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }
    #endregion

    #region Join Game
    public void JoinRoom()
    {
        joinRoomCallback.Invoke(match);
    }
    #endregion
}
