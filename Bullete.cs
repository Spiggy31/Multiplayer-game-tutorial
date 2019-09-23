using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bullete : MonoBehaviourPun {

    public bool MovingDirection;
    public float MoveSpeed = 8;

    public float DestroyTime=  2f;

    void Start()
    {
       
    }
    IEnumerator destroyBullete()
    {
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
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

    [PunRPC]
    void Destroy()
    {
        Destroy(this.gameObject);
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
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);

        }
    }
}
