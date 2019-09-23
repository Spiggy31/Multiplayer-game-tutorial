using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;

public class CowBoy : MonoBehaviourPun
{
    public float moveSpeed = 5;
    public bool disableInputs = false;

    public GameObject playerCam;
    public SpriteRenderer sprite;
    public PhotonView photonview;
    public Animator anim;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPointRight;
    public Transform bulletSpawnPointLeft;
    public Text playerName;

    private bool allowMoving = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.localPlayer = this.gameObject;
            playerCam.SetActive(true);
            playerName.text = "You : " + PhotonNetwork.NickName;
            playerName.color = Color.green;
        }
        else
        {
            playerName.text = photonview.Owner.NickName;
            playerName.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && !disableInputs)
        {
            checkInputs();
        }
    }

    private void checkInputs()
    {
        if (allowMoving)
        { 
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += movement * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.RightControl) && anim.GetBool("IsMove") == false)
        {
            Shot();
        }
        else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            anim.SetBool("IsShot", false);
            allowMoving = true;
        }
        
        if (Input.GetKeyDown(KeyCode.D) && anim.GetBool("IsShot") == false)
        {
            anim.SetBool("IsMove", true);
            //FlipSprite_Right()
            photonView.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && anim.GetBool("IsShot") == false)
        {
            anim.SetBool("IsMove", true);
            //FlipSprite_Left()
            photonView.RPC("FlipSprite_Left", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }

    public void Shot()
    {

        if (sprite.flipX == false)
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(bulletSpawnPointRight.position.x, bulletSpawnPointRight.position.y), Quaternion.identity, 0);

        }
        
        if (sprite.flipX == true)
        {
            Debug.Log("firing");
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(bulletSpawnPointLeft.position.x, bulletSpawnPointLeft.position.y), Quaternion.identity, 0);

            bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }

        anim.SetBool("IsShot", true);
        allowMoving = false;
    }

    [PunRPC]
    private void FlipSprite_Right()
    {
        sprite.flipX = false;
    }

    [PunRPC]
    private void FlipSprite_Left()
    {
        sprite.flipX = true;
    }
}
