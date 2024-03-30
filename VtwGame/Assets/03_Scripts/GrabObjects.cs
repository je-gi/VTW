
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class GrabObjects : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private LayerMask passableLayer;


    private GameObject grabbedObject;
    private bool isGrabbing;
    private bool canGrab = true;
    private PlayerControls controls;
    private Rigidbody2D rbGrabbedObject;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Grab.performed += _ => ToggleGrab();
        controls.Player.Throw.performed += _ => ThrowObject();
    }


    private void OnEnable()
    {
        controls.Enable();
    }


    private void OnDisable()
    {
        controls.Disable();
    }


    private void Update()
    {
        if (isGrabbing && grabbedObject != null)
        {
            rbGrabbedObject.MovePosition(grabPoint.position);
        }
    }


    private void ToggleGrab()
    {
        if (canGrab)
        {
            if (!isGrabbing)
            {
                var hit = Physics2D.Raycast(rayPoint.position, transform.right, playerData.rayDistance, LayerMask.GetMask("Light"));
                if (hit.collider != null)
                {
                    grabbedObject = hit.collider.gameObject;
                    rbGrabbedObject = grabbedObject.GetComponent<Rigidbody2D>();
                    if (rbGrabbedObject != null)
                    {
                        rbGrabbedObject.gravityScale = 0;
                        grabbedObject.transform.position = grabPoint.position;
                        isGrabbing = true;
                    }
                }
            }
            else
            {
                StopGrab();
            }
        }
    }


    private void ThrowObject()
    {
        if (isGrabbing && grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);
            rbGrabbedObject.gravityScale = 9f;


            Vector2 throwDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            rbGrabbedObject.AddForce(new Vector2(throwDirection.x * playerData.throwForce, playerData.throwForce * 2f), ForceMode2D.Impulse);


            StartCoroutine(StopObjectInAir(rbGrabbedObject, 0.5f));


            isGrabbing = false;
            grabbedObject = null;
            rbGrabbedObject = null;
        }
    }


    private IEnumerator StopObjectInAir(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);


        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }


    private void StopGrab()
    {
        if (isGrabbing && grabbedObject != null)
        {
            rbGrabbedObject.gravityScale = 9f;
            isGrabbing = false;
            grabbedObject = null;
            rbGrabbedObject = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (passableLayer == (passableLayer | (1 << other.gameObject.layer)))
        {
            canGrab = false;
            ReleaseObject();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (passableLayer == (passableLayer | (1 << other.gameObject.layer)))
        {
            canGrab = true;
        }
    }


    private void ReleaseObject()
    {
        if (isGrabbing && grabbedObject != null)
        {
            rbGrabbedObject.gravityScale = 9f;
            isGrabbing = false;
            grabbedObject = null;
            rbGrabbedObject = null;
        }
    }
}
