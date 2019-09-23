using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Health : MonoBehaviourPun
{
    public float health = 1;

    public Image fillImage;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D playerCollider;
    public GameObject playerCanvas;
    public CowBoy playerScript;

    public void CheckHealth()
    {
        if (photonView.IsMine && health <= 0)
        {
            GameManager.instance.EnableRespawn();
            playerScript.disableInputs = true;
            this.GetComponent<PhotonView>().RPC("Death", RpcTarget.AllBuffered);
        }
    }

    public void EnableInputs()
    {
        playerScript.disableInputs = false;
    }

    [PunRPC]
    public void Death()
    {
        rb.gravityScale = 0;
        playerCollider.enabled = false;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }

    [PunRPC]
    public void Revive()
    {
        rb.gravityScale = 1;
        playerCollider.enabled = true;
        sr.enabled = true;
        playerCanvas.SetActive(true);
        health = 1;
        fillImage.fillAmount = 1;
    }

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        fillImage.fillAmount -= damage;
        health = fillImage.fillAmount;
        CheckHealth();
    }
}
