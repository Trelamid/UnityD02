using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private RTSController _Controller;
    public enum type
    {
        friend, enemy
    }
    [SerializeField]public int health;
    private SpriteRenderer _sprite;
    public type _type;

    public void GetDamage(int damage, GameObject attackObj)
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = new Color(_sprite.color.r + 0.5f, _sprite.color.g - 0.5f, _sprite.color.b - 0.5f, _sprite.color.a);
        health -= damage;

        DebugAttack(attackObj);
        
        if (health <= 0)
        {
            _Controller = GameObject.FindObjectOfType<RTSController>();
            // foreach (var VARIABLE in _Controller._unitsActive)
            // {
            //     if (VARIABLE == this)
            //     {
            //         _Controller._unitsActive.Remove(VARIABLE);
            //     }
            // }
            Destroy(gameObject);
        }

        Invoke("bloodOff", 0.5f);
    }

    public virtual void DebugAttack(GameObject attackObj){}

    void bloodOff()
    {
        _sprite.color = new Color(_sprite.color.r - 0.5f, _sprite.color.g + 0.5f, _sprite.color.b + 0.5f, _sprite.color.a + 0.5f);
    }
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
