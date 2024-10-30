using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject selector;
    public Vector3 GetPosition()
    {
        return new Vector3(transform.position.x - 0.5f, transform.localScale.y + 1, transform.position.z - 0.5f);
    }
}
