using UnityEngine;
using Photon.Pun;

public class OnlinePlayer : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
        else
        {
            ApplyRemoteData();
        }
    }
    private void ApplyRemoteData()
    {
        // odczyt InstantiationData i CarAppearance.SetNameAndColor(...)
    }
}
