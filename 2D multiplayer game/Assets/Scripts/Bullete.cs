using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bullete : MonoBehaviourPun {

    public bool MovingDirection;
    public float MoveSpeed = 8;
    public float DestroyTime=  2f;
    public float bulletDamage = 0.3f;
    void Start()
    {
        StartCoroutine(destroyBullete());
    }
    IEnumerator destroyBullete()
    {
        yield return new WaitForSeconds(DestroyTime);
        Debug.Log("destroying bullet");
        PhotonNetwork.Destroy(this.gameObject);
    }

    void Update()
    {
        if (!MovingDirection)
        {
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        }
        else {
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);

        }
    }


    [PunRPC]
    public void ChangeDirection()
    {
        MovingDirection = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (!photonView.IsMine)
        {
            return;
        }

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();
       
        if (target != null && (!target.IsMine || target.IsSceneView))
        {
            if (target.tag == "Player")
            {
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, bulletDamage);
            }
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
