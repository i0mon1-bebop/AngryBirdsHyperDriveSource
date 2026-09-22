using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private LayerMask _testGround;
    [SerializeField] private Transform _testA;
    [SerializeField] private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        _testA = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D groundhit = Physics2D.Raycast(_testA.position, new Vector2(0, -1), rayDistance, _testGround);

        if (groundhit.collider != null)
        {
            sprite.enabled = true;
            transform.position = groundhit.point;
        }
        else if (groundhit.collider == null)
        {
            sprite.enabled = false;
          
        }
    }
}
