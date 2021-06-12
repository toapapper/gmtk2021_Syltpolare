using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpScript : MonoBehaviour
{
    public GameObject heldItem = null;
    public Rigidbody2D heldItemRB = null;

    [Tooltip("I PIXLAR")]
    public float range = 200;//pixlar i range man kan flytta saken
    public float throwForce = 50;//i newtons typ antar jag

    public Sprite open_sprite;
    public Sprite closed_sprite;

    private SpriteRenderer sr;

    public float grabForce = 500;
    private List<GameObject> mousedOver = new List<GameObject>(5);
    private Camera _camera;
    private GameObject player;
    private Rigidbody2D playerRB;

    private void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void PickUp(InputContext context)
    {
        if (mousedOver.Count == 0 || context.State == InputContext.InputState.Canceled)//släpper musen gör ingenting just nu
            return;

        PickUpGo(mousedOver[0]);
    }

    public void Throw(InputContext context)
    {
        if (heldItem == null)
            return;

        //annars kasta..
    }

    public void PickUpGo(GameObject go)
    {
        Debug.Log("PickupGO");
        if (heldItem != null)
            return;

        heldItem = go;
        heldItem.GetComponent<Plug>().held = true;
        heldItemRB = heldItem.GetComponent<Rigidbody2D>();

        if (heldItemRB == null)
            Debug.Log("No RB");
    }

    public void ReleaseItem()
    {
        heldItem.GetComponent<Plug>().held = false;
        heldItemRB = null;
        heldItem = null;
    }

    //FULING
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Plug"))
        {
            mousedOver.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (mousedOver.Contains(collision.gameObject))
            mousedOver.Remove(collision.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region Hitbox positionuppdatering
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerScreenPos = _camera.WorldToScreenPoint(player.transform.position);

        Vector2 diffVector = mousePosition - playerScreenPos;
        if (diffVector.magnitude > range)
            diffVector = diffVector.normalized * range;//Normalisera, multiplicera med range så får vi den på gränsen.

        Vector2 pos = _camera.ScreenToWorldPoint(playerScreenPos + diffVector);
        transform.position = pos;
        #endregion


        if (heldItem != null)
        {
            //debug kanske
            if (Input.GetKeyDown(KeyCode.X))
            {
                ReleaseItem();
                return;
            }
            Vector2 hiPos = heldItem.transform.position;
            heldItemRB.AddForce((pos - hiPos) * grabForce);
            playerRB.AddForce(heldItem.GetComponent<HingeJoint2D>().reactionForce);
            sr.sprite = closed_sprite;
        }
        else
            sr.sprite = open_sprite;
    }
}
