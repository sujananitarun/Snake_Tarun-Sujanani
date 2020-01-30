using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;

    [HideInInspector]
    public float step_Lenght = 0.2f;

    [HideInInspector]
    public float movement_Frequency = 0.1f;

    private float counter;
    private bool move;

    [SerializeField]
    private GameObject tailPrefab;

    private List<Vector3> deltaPosition;

    private List<Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;

    private bool create_Node_At_Tail;

    void Awake()
    {
        tr = transform;
        main_Body = GetComponent<Rigidbody>();

        initSnakeNodes();
        initPlayer();

        deltaPosition = new List<Vector3>() {
            new Vector3(-step_Lenght, 0f),  //left
            new Vector3(0f,step_Lenght),  // up
            new Vector3(step_Lenght, 0f), // right
            new Vector3(0f,-step_Lenght)  // down 

        };
        

    }

    // Update is called once per frame
    void Update()
    {
        checkMovementFrequency();
    }

    void FixedUpdate()
    {
        if (move)
        {
            move = false;

            Move();
        }
        
    }

    void initSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        head_Body = nodes[0];        
    }

    void setDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;

    }

    void initPlayer()
    {
        setDirectionRandom();

        switch (direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;

            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;

            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2f, 0f);
                break;
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);
                break;
        }
    }

    void Move()
    {
        Vector3 dPosition = deltaPosition[(int)direction];
        Vector3 parentPos = head_Body.position;
        Vector3 prevPosotion;
        main_Body.position = main_Body.position + dPosition;
        head_Body .position = head_Body.position + dPosition;

        for (int i = 1; i < nodes.Count; i++)
        {
            prevPosotion = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPosotion;
        }

        //check if we need to create a new node beacause we ate a fruit
        if (create_Node_At_Tail)
        {
            create_Node_At_Tail = false;

            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);

            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());

        }
        
    }

    void checkMovementFrequency()
    {
        counter += Time.deltaTime;
        if(counter>= movement_Frequency)
        {
            counter = 0f;
            move = true;
        }
    }

    public void setInputDirection(PlayerDirection dir)
    {
        if(dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
           dir == PlayerDirection.DOWN && direction == PlayerDirection.UP||
           dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT||
           dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT)
        {
            return;
        }

        direction = dir;
        forceMove();
    }
    void forceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.FRUIT)
        {
            target.gameObject.SetActive(false);

            create_Node_At_Tail = true;
            GameplayController.instance.increaseScore();
        }

        if (target.tag == Tags.WALL ||target.tag == Tags.TAIL)
        {
            Time.timeScale = 0f;
        }
    }
}
