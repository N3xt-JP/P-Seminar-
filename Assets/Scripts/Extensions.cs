using UnityEngine;


public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default","Ground");

    
    public static bool Raycast(this Rigidbody2D rb, Vector2 direction)
    {

        float radius = 0.25f;
        float offset = 1.00000001f;
        
        RaycastHit2D hit = Physics2D.CircleCast(rb.position, radius, direction.normalized, offset, layerMask);
        Debug.DrawRay(rb.position, direction * offset, Color.red);
        return hit.collider != null && hit.rigidbody != rb;
    } 
    public static bool DotTest(this Transform transform, Transform other, Vector2 testdirection)
    {
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testdirection) > 0.25f;
    }

}
