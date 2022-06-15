using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RTSController : MonoBehaviour
{
    private Vector3 _startPos;
    [HideInInspector]public List<UnitFriend> _unitsActive;
    private Camera _main;

    [SerializeField]
    private GameObject _selectedAreaTran;

    public GameObject MainEnemy;
    public GameObject MainFriend;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _unitsActive = new List<UnitFriend>();
        _main = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        AI();
        CheckButtons();
        if (!MainEnemy)
        {
            Debug.Log("The Human Team wins.");
            Destroy(this);
        }
        else if (!MainFriend)
        {
            Debug.Log("The Orc Team wins.");
            Destroy(this);
        }
    }

    void AI()
    {
        
    }

    void CheckButtons()
    {
        if (Input.GetMouseButtonDown(0) && (_unitsActive.Count == 0 || Input.GetKey(KeyCode.Space)))
        {
            _selectedAreaTran.SetActive(true);
            _startPos = _main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log(_startPos);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Collider2D enemyOrNo = Physics2D.OverlapPoint(_main.ScreenToWorldPoint(Input.mousePosition));
            Unit unit;
            if (enemyOrNo)
            {
                unit = enemyOrNo.gameObject.GetComponent<Unit>();
            }
            else
                unit = null;

            for (int i = 0; i < _unitsActive.Count; i++)
            {
                if (_unitsActive[i])
                {
                    if(unit == null || unit._type == Unit.type.friend)
                        _unitsActive[i].MoveThere(_main.ScreenToWorldPoint(Input.mousePosition));
                    else
                        _unitsActive[i].AttackIt(unit.gameObject);
                }
            }
            
            foreach (var VARIABLE in _unitsActive)
            {
                if(unit == null || unit._type == Unit.type.friend)
                    VARIABLE.MoveThere(_main.ScreenToWorldPoint(Input.mousePosition));
                else
                    VARIABLE.AttackIt(unit.gameObject);
            }
        }

        if (Input.GetMouseButton(0) && (_unitsActive.Count == 0 || Input.GetKey(KeyCode.Space)) )
        {
            Vector3 currentMousePos = _main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lowerLeft = new Vector3(Mathf.Min(_startPos.x, currentMousePos.x),
                Mathf.Min(_startPos.y, currentMousePos.y));
            Vector3 upperRight = new Vector3(Mathf.Max(_startPos.x, currentMousePos.x),
                Mathf.Max(_startPos.y, currentMousePos.y));
            _selectedAreaTran.transform.position = lowerLeft;
            _selectedAreaTran.transform.localScale = upperRight - lowerLeft;
        }
        
        if (Input.GetMouseButtonUp(0) && (_unitsActive.Count == 0 || Input.GetKey(KeyCode.Space)))
        {
            // _unitsActive.Clear();
            _selectedAreaTran.SetActive(false);
            Vector3 endPos = _main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] units = Physics2D.OverlapAreaAll(_startPos, endPos);
            foreach (var unit in units)
            {
                UnitFriend friend = unit.GetComponent<UnitFriend>();
                if (friend)
                {
                    // Debug.Log(friend.name);
                    _unitsActive.Add(friend);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _unitsActive.Clear();
        }
    }
}
