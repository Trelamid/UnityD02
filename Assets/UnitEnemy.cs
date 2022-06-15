using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnemy : Unit
{
    [HideInInspector]public int freedomLvl; //0 1 2 3
    private Vector2 _movePos;
    private GameObject _attackIt;
    private float speed = 3;
    private float _rateOfAttack = 2f;
    private float _forRate;
    [SerializeField]private int _attackDamage;
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (freedomLvl == 0)
        {
            // Debug.Log("Fidnd");
            float min = 1000000;
            GameObject friendAttack = null;
            
            UnitFriend[] friends = GameObject.FindObjectsOfType<UnitFriend>();
            for (int i = 0; i < friends.Length; i++)
            {
                if (friends[i]._type == type.friend && friends[i].name.Contains("Friend")&& (friends[i].gameObject.transform.position - transform.position).magnitude < min)
                {
                    min = (friends[i].gameObject.transform.position - transform.position).magnitude;
                    friendAttack = friends[i].gameObject;
                }
            }
            if (friendAttack != null)
            {
                AttackIt(friendAttack);
                freedomLvl = 2;
            }
            else
            {
                Unit[] friendsAll = GameObject.FindObjectsOfType<Unit>();
                for (int i = 0; i < friendsAll.Length; i++)
                {
                    if (friendsAll[i]._type == type.friend && friendsAll[i].name.Contains("Friend")&& (friendsAll[i].gameObject.transform.position - transform.position).magnitude < min)
                    {
                        min = (friendsAll[i].gameObject.transform.position - transform.position).magnitude;
                        friendAttack = friendsAll[i].gameObject;
                    }
                }
                if (friendAttack != null)
                {
                    AttackIt(friendAttack);
                    freedomLvl = 2;
                }
            }
        }
        else if(freedomLvl == 2)
        {
            if (!_attackIt)
            {
                freedomLvl = 0;
                return;
            }
            Vector2 myPos = transform.position;
            Vector2 endPos = _attackIt.transform.position;
            if ((endPos - myPos).magnitude > 0.5f)
            {
                _animator.SetBool("Attack", false);
                Vector2 auf = (endPos - myPos).normalized;
                transform.Translate((endPos - myPos).normalized * Time.deltaTime * speed);
                _animator.SetFloat("Horizontal", auf.x);
                _animator.SetFloat("Vertical", auf.y);
            }
            else
                freedomLvl = 3;
        }
        else if (freedomLvl == 3)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (!_attackIt)
        {
            freedomLvl = 0;
        }
        else if (Time.time > _forRate)
        {
            // if (!_attackIt)
            // {
            //     freedomLvl = 0;
            //     return;
            // }
            _forRate = Time.time + _rateOfAttack;
            Vector2 myPos = transform.position;
            Vector2 endPos = _attackIt.transform.position;
            if ((endPos - myPos).magnitude <= 0.7f)
            {
                _animator.SetBool("Attack", true);
                // Debug.Log(_attackIt.name);
                Unit unit;
                if (!_attackIt || !(unit = _attackIt.GetComponent<Unit>()))
                {
                    freedomLvl = 0;
                    return;
                }
                else
                {
                    unit.GetDamage(_attackDamage, gameObject);
                }
            }
            else
            {
                freedomLvl = 2;
            }
            
            // Debug.Log("Attack");
        }
    }

    public override void DebugAttack(GameObject attackObj)
    {
        Debug.Log("Orc Unit ["+health+"/100]HP has been attacked.");
    }

    public void MoveThere(Vector2 pos)
    {
        _movePos = pos;
        freedomLvl = 1;
    }

    public void AttackIt(GameObject attackIt)
    {
        _attackIt = attackIt;
        freedomLvl = 2;
    }
}
