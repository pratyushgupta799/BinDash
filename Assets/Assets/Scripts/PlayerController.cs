using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool _isDragging;
    private Vector2 _dragDelta;
    
    void Update()
    {
        //Debug.Log("Sensitivity: " + GameManager.Sensitivity);
        float drag = _dragDelta.x / (50f * (1 / GameManager.Sensitivity));
        if (GameManager.CanMove)
        {
            if (_isDragging && _dragDelta.x > 0 && transform.position.x <= 3f)
            {
                //transform.Translate(new Vector3(Mathf.Clamp(drag, -2.5f, 2.5f), 0f, 0f));
                transform.Translate(new Vector3(drag, 0f, 0f));
            }

            if (_isDragging && _dragDelta.x < 0 && transform.position.x >= -3f)
            {
                //transform.Translate(new Vector3(Mathf.Clamp(drag, -2.5f, 2.5f), 0f, 0f));
                transform.Translate(new Vector3(drag, 0f, 0f));
            }

            if (transform.position.x > 3f)
            {
                transform.position = new Vector3(3f, transform.position.y, transform.position.z);
            }

            if (transform.position.x < -3f)
            {
                transform.position = new Vector3(-3f, transform.position.y, transform.position.z);
            }
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
        if (!context.control.device.enabled)
        {
            Debug.LogError("swipe is disabled");
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Started:
                _isDragging = true;
                break;
            case InputActionPhase.Performed:
                if (_isDragging)
                {
                    _dragDelta = context.ReadValue<Vector2>();
                    
                    //Debug.Log("drag delta: " + _dragDelta);
                }
                break;
            case InputActionPhase.Canceled:
                _isDragging = false;
                _dragDelta = Vector2.zero;
                //Debug.Log("drag ended");
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.ScoreUp();
            //Debug.Log("Destroyed");
        }
    }
}
