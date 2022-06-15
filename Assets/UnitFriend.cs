using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitFriend : Unit
{
    [HideInInspector]public int freedomLvl; //0 1 2 3
    private Vector2 _movePos;
    private GameObject _attackIt;
    private float speed = 3;
    private float _rateOfAttack = 2;
    private float _forRate;
    [SerializeField]private int _attackDamage;
    private Animator _animator;
    private Sprite _sprite;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (freedomLvl == 1)
        {
            Vector2 myPos = transform.position;
            if ((_movePos - myPos).magnitude > 0.5f)
                transform.Translate((_movePos - myPos).normalized * Time.deltaTime * speed);
            else
                freedomLvl = 0;
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
                transform.Translate((endPos - myPos).normalized * Time.deltaTime * speed);
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
        if (Time.time > _forRate)
        {
            _forRate = Time.time + _rateOfAttack;
            if (_attackIt)
            {
                _animator.Play("A_ManAttack");
                _attackIt.GetComponent<Unit>().GetDamage(_attackDamage, gameObject);
            }
        }
    }
    
    public override void DebugAttack(GameObject attackObj)
    {
        Debug.Log("Human Unit ["+health+"/100]HP has been attacked.");
    }
    public void MoveThere(Vector2 pos)
    {
        const float f = 0.5f;
        _animator.StopPlayback();
        if (_movePos.x < pos.x && Math.Abs(_movePos.y - pos.y) < f)
        {
            if (!_faceRight) Flip();
            _animator.Play("A_ManWalkRight");
        }
        else if (_movePos.x > pos.x && Math.Abs(_movePos.y - pos.y) < f)
        {
            if (_faceRight) Flip();
            _animator.Play("A_ManWalkRight");
        }
        else if (Math.Abs(_movePos.x - pos.x) < f && _movePos.y < pos.y)
        {
            _animator.Play("A_ManWalkFRWD");
        }
        else if (Math.Abs(_movePos.x - pos.x) < f && _movePos.y > pos.y)
        {
            _animator.Play("A_ManWalkBack");
        }
        else
        {
            if (_movePos.x < pos.x && _movePos.y < pos.y)
            {
                if (!_faceRight) Flip();
                _animator.Play("A_ManWalkFRight");
            }
            else if (_movePos.x > pos.x && _movePos.y < pos.y)
            {
                if (_faceRight) Flip();
                _animator.Play("A_ManWalkFRight");
            }
            else if (_movePos.x > pos.x && _movePos.y < pos.y)
            {
                if (_faceRight) Flip();
                _animator.Play("A_ManWalkFRight");
            }
            else if (_movePos.x > pos.x && _movePos.y > pos.y)
            {
                if (_faceRight) Flip();
                _animator.Play("A_ManWalkBRight");
            }
        }
        _movePos = pos;
        freedomLvl = 1;
    }

    public void AttackIt(GameObject attackIt)
    {
        _attackIt = attackIt;
        freedomLvl = 2;
    }
    
}
