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
        if (mousedOver.Count == 0 || context.State == InputContext.InputState.Canceled || Possess.GetCurrentPossessed != player)//sl�pper musen g�r ingenting just nu
            return;

        PickUpGo(mousedOver[0]);
    }

    public void Throw(InputContext context)
    {
        if (heldItem == null || Possess.GetCurrentPossessed != player)
            return;
        
        heldItemRB.AddForce(currentDiffVector.normalized * throwForce);
        ReleaseItemPrivate();
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
        ReleaseItemPrivate();
    }
    
    private void ReleaseItemPrivate()
    {
        heldItem.GetComponent<Plug>().held = false;
        heldItemRB = null;
        heldItem = null;
    }

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


        #region Hitbox positionuppdatering
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerScreenPos = _camera.WorldToScreenPoint(player.transform.position);

        Vector2 diffVector = mousePosition - playerScreenPos;
        if (diffVector.magnitude > range)
            diffVector = diffVector.normalized * range;//Normalisera, multiplicera med range s� f�r vi den p� gr�nsen.

        currentDiffVector = diffVector;

        Vector2 pos = _camera.ScreenToWorldPoint(playerScreenPos + diffVector);
        transform.position = pos;
        #endregion
        
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diffVector.y, diffVector.x) * Mathf.Rad2Deg);

        if (heldItem != null)
        {
            Vector2 hiPos = heldItem.transform.position;
            heldItemRB.AddForce((pos - hiPos) * grabForce);
            sr.sprite = closed_sprite;
        }
        else
            sr.sprite = open_sprite;
    }
}
