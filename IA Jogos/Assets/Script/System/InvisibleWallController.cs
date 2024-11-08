using UnityEngine;

public class InvisibleWallController : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void EnableWall(bool enable)
    {
        boxCollider.enabled = enable;
    }
}