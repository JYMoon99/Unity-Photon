using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject preasureBox;

    public Vector3 RandomPosition(int value)
    {
        Vector3 direction = Random.insideUnitSphere;

        direction *= value;

        direction.y = 0;

        return direction;
    }

    private void Awake()
    {
        PhotonNetwork.Instantiate
        (
            "Character",
            RandomPosition(5),
            Quaternion.identity
        );
    }

}
