using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpScript : MonoBehaviour
{
    public GameObject heldItem = null;
    public Rigidbody2D heldItemRB = null;

    [Tooltip("I PIXLAR")]
    public float range = 200;//pixlar i range man kan flytta saken
    public float throwSpeed = 200;//i newtons typ antar jag
    public float pushForce = 200;

    public Sprite open_sprite;
    public Sprite closed_sprite;

    private SpriteRenderer sr;

    public float grabForce = 500;
    private List<GameObject> mousedOverPlugs = new List<GameObject>(5);
    public List<GameObject> mousedOverDynamicObstacles = new List<GameObject>(10);
    private Camera _camera;
    private GameObject player;
    private Rigidbody2D playerRB;

    private Vector2 currentDiffVector;

    private void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = transform.root.gameObject;
        playerRB = player.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void PickUp(InputContext context)
    {
        if (mousedOverPlugs.Count == 0 || context.State == InputContext.InputState.Canceled || Possess.GetCurrentPossessed != player)//släpper musen gör ingenting just nu
            return;

        PickUpGo(mousedOverPlugs[0]);
    }

    public void Throw(InputContext context)
    {
        if (Possess.GetCurrentPossessed != player || context.State == InputContext.InputState.Canceled)
            return;

        if(heldItem != null)
        {
            heldItemRB.velocity = currentDiffVector.normalized * throwSpeed;
            ReleaseItemPrivate();
        }

        if(mousedOverDynamicObstacles.Count != 0)
        {
            foreach(GameObject gobj in mousedOverDynamicObstacles)
            {
                gobj.GetComponent<Rigidbody2D>().AddForce(currentDiffVector.normalized * pushForce);
            }
        }
    }

    public void PickUpGo(GameObject go)
    {
        if (heldItem != null || Possess.GetCurrentPossessed != player)
            return;

        heldItem = go;
        heldItem.GetComponent<Plug>().held = true;
        heldItemRB = heldItem.GetComponent<Rigidbody2D>();

        if (heldItemRB == null)
            Debug.Log("No RB");
    }

    public void ReleaseItem(InputContext context)
    {
        if (context.State == InputContext.InputState.Canceled)
            ReleaseItemPrivate();
    }

    private void ReleaseItemPrivate()
    {
        if (heldItem == null)
        {
            heldItemRB = null;
            return;
        }

        heldItem.GetComponent<Plug>().held = false;
        heldItemRB = null;
        heldItem = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Plug") && !mousedOverPlugs.Contains(collision.gameObject))
        {
            mousedOverPlugs.Add(collision.gameObject);
        }

        if (collision.transform.CompareTag("Player") && !mousedOverDynamicObstacles.Contains(collision.gameObject))
        {
            mousedOverDynamicObstacles.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (mousedOverPlugs.Contains(collision.gameObject))
        {
            mousedOverPlugs.Remove(collision.gameObject);
        }

        if (mousedOverDynamicObstacles.Contains(collision.gameObject))
        {
            mousedOverDynamicObstacles.Remove(collision.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Possess.GetCurrentPossessed != player)
        {
            if (sr.enabled)
                sr.enabled = false;

            return;
        }
        else
        {
            if (!sr.enabled)
                sr.enabled = true;
        }

        if (mousedOverPlugs.Count != 0)
        {
            if (mousedOverPlugs[0] == null)
                mousedOverPlugs.RemoveAt(0);
        }
        if (mousedOverDynamicObstacles.Count != 0)
        {
            if (mousedOverDynamicObstacles[0] == null)
                mousedOverDynamicObstacles.RemoveAt(0);
        }

        #region Hitbox positionuppdatering
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerScreenPos = _camera.WorldToScreenPoint(player.transform.position);

        Vector2 diffVector = mousePosition - playerScreenPos;
        if (diffVector.magnitude > range)
            diffVector = diffVector.normalized * range;//Normalisera, multiplicera med range så får vi den på gränsen.

        currentDiffVector = diffVector;

        Vector2 pos = _camera.ScreenToWorldPoint(playerScreenPos + diffVector);
        transform.position = pos;
        #endregion

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diffVector.y, diffVector.x) * Mathf.Rad2Deg);

        if (heldItem != null)
        {
            if (heldItemRB == null)
                heldItemRB = heldItem.GetComponent<Rigidbody2D>();

            Vector2 hiPos = heldItem.transform.position;
            heldItemRB.AddForce((pos - hiPos) * grabForce);
            sr.sprite = closed_sprite;
        }
        else
            sr.sprite = open_sprite;
    }
}
