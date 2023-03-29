using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public enum CollisionX { None, Left, Middle, Right }
    public enum CollisionY { None, Up, Middle, Down }
    public enum CollisionZ { None, Forward, Middle, Backward }

    private PlayerController playerController;
    [SerializeField] private CollisionX collisionX;
    [SerializeField] private CollisionY collisionY;
    [SerializeField] private CollisionZ collisionZ;

    private void Awake() {
        playerController = gameObject.GetComponent<PlayerController>();
    }

    public void OnCharacterCollision(Collider collider)
    {
        collisionX = GetCollisionX(collider);
        collisionY = GetCollisionY(collider);
        collisionZ = GetCollisionZ(collider);
    }

    private CollisionX GetCollisionX(Collider collider)
    {
        Bounds characterColliderBounds = playerController.CharacterController.bounds;
        Bounds colliderBounds = collider.bounds;

        float minX = Mathf.Max(colliderBounds.min.x, characterColliderBounds.min.x);
        float maxX = Mathf.Min(colliderBounds.max.x, characterColliderBounds.max.x);
        float average = (minX + maxX) / 2 - colliderBounds.min.x;

        if (average > colliderBounds.size.x - 0.33f)
        {
            collisionX = CollisionX.Right;
        }
        else if (average < 0.33f)
        {
            collisionX = CollisionX.Left;
        }
        else
        {
            collisionX = CollisionX.Middle;
        }
        return collisionX;
    }

    private CollisionY GetCollisionY(Collider collider)
    {
        Bounds characterColliderBounds = playerController.CharacterController.bounds;
        Bounds colliderBounds = collider.bounds;

        float minY = Mathf.Max(colliderBounds.min.y, characterColliderBounds.min.y);
        float maxY = Mathf.Min(colliderBounds.max.y, characterColliderBounds.max.y);
        float average = (minY + maxY) / 2 - colliderBounds.min.y;

        if (average > colliderBounds.size.y - 0.33f)
        {
            collisionY = CollisionY.Up;
        }
        else if (average < 0.33f)
        {
            collisionY = CollisionY.Down;
        }
        else
        {
            collisionY = CollisionY.Middle;
        }
        return collisionY;
    }

    private CollisionZ GetCollisionZ(Collider collider)
    {
        Bounds characterColliderBounds = playerController.CharacterController.bounds;
        Bounds colliderBounds = collider.bounds;

        float minZ = Mathf.Max(colliderBounds.min.z, characterColliderBounds.min.z);
        float maxZ = Mathf.Min(colliderBounds.max.z, characterColliderBounds.max.z);
        float average = (minZ + maxZ) / 2 - colliderBounds.min.z;

        if (average > colliderBounds.size.z - 0.33f)
        {
            collisionZ = CollisionZ.Forward;
        }
        else if (average < 0.33f)
        {
            collisionZ = CollisionZ.Backward;
        }
        else
        {
            collisionZ = CollisionZ.Middle;
        }
        return collisionZ;
    }
}
