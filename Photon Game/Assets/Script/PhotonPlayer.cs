using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonPlayer : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI nickName;
    [SerializeField] float score;
    [SerializeField] float mouseX;
    [SerializeField] float rotateSpeed;
    [SerializeField] float speed = 5.0f;

    [SerializeField] Vector3 direction;
    [SerializeField] Camera temporaryCamera;

    void Awake()
    {
        nickName.text = photonView.Owner.NickName;
    }

    void Start() 
    {
        if(photonView.IsMine) 
        {
            Camera.main.gameObject.SetActive(false);
        }
        else
        {
            temporaryCamera.enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    
    }


    public void Movement(float time)
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");

        direction.Normalize();

        transform.position += transform.TransformDirection(direction) * speed * time;
    }

}
