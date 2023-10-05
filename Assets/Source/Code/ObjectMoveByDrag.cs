using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMoveByDrag : MonoBehaviour
{
    [SerializeField] List<GameObject> particleVFXs;

    private Vector3 startPos;
    private Transform target;
    
    [SerializeField] private MeshRenderer mesh;
    public int id;
    private void Start()
    {
        transform.Rotate(new Vector3(0,0,Random.Range(0,360)));
    }
    
    public void SetData(int ids)
    {
        id = ids;
        mesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        mesh.material.color = GameManager.Instance.listColor[id];

        switch (id)
        {
            case 0: gameObject.tag = "b1";break;
            case 1: gameObject.tag = "b2";break;
            case 2: gameObject.tag = "b3";break;
            case 3: gameObject.tag = "b4";break;
            default: gameObject.tag = "b1";break;
        }
    }

    private void OnEnable()
    {
        startPos = transform.position;
    }

    public void PickUp()
    {
        transform.rotation = new Quaternion(0,0,0,0);
    }

    public void CheckOnMouseUp()
    {
        //transform.position = startPos;
        if (target)
        {
            transform.position = target.position;
            GameObject explosion = Instantiate(particleVFXs[Random.Range(0,particleVFXs.Count)], new Vector3(transform.position.x,transform.position.y,-1), transform.rotation);
            Destroy(explosion, .75f);
            GameManager.Instance.RemoveListObjMove(this);
            Destroy(gameObject);
        }
    }

    private Vector3 mousePos;

    Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDrag()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
        pos.z = 0;
        transform.position = pos;
    }

    private void OnMouseUp()
    {
        CheckOnMouseUp();
    }

    private void OnMouseDown()
    {
        mousePos = Input.mousePosition - GetMousePos();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.CompareTag("NotUse")) return;
        if (gameObject.tag == collision.gameObject.tag)
        {
            target = collision.transform;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (transform.CompareTag("NotUse")) return;
        if (gameObject.tag == collision.gameObject.tag)
        {
            target = null;
        }
    }
}
