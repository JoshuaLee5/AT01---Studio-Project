using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
    private Vector3 currentDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };

    //public Node TargetNode { get; private set; }
   // public Node CurrentNode { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
       /// InitializeAgent();
       // foreach (Node node in GameManager.Instance.Nodes)
      //  {
           // if (node.Parents.Length > 2 && node.Children.Length == 0)
            {
           //     CurrentNode = node;
            //    break;
            }
       /// }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (currentNode != null)
            {
                //If beyond 0.25 units of the current node.
                if (Vector3.Distance(transform.position, currentNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                //Implement path finding here
                else 
                {
                    //find new target node

                    //if target node is not the AIs current node is not null
                    //if (Player != currentNode)
                    //{
                        
                    //}
;
                        //set current node to traget node
                        //else if player traget not null and player traget node not current node
                        
                    //set current node to players target node

                        //if current node is not null
                        //set current direction towards node
                        //normalize current direction
                }

            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
            }

            Debug.DrawRay(transform.position, currentDir, Color.cyan);
        }
    }

    //Called when a collider enters this object's trigger collider.
    //Player or enemy must have rigidbody for this to function correctly.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.tag == "Player")
            {
                playerCaught = true;
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
    void InitializeAgent()
    {
        currentNode = GameManager.Instance.Nodes[0];
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    //Implement DFS algorithm method here
    private Node DepthFirstSearch()
    {
        Stack nodeStack = new Stack(); //Stacks the unvisited nodes, last one added to stack is next visited
        List<Node> visitedNodes = new List<Node> (); //track visited nodes
        nodeStack.Push(GameManager.Instance.Nodes[0]); //add root node to stack 

        while (nodeStack.Count > 0) //while stack is not empty
        { 
            Node current = (Node)nodeStack.Pop(); //pop the last node added to stack
            visitedNodes.Add(currentNode); //mark current node as visited
            foreach (Node child in currentNode.Children)
            {
                if (visitedNodes.Contains(child) == false && nodeStack.Contains(child) == false)
                {
                    if (child == GameManager.Instance.Player.CurrentNode) //check if the child is equal to players current node
                    {
                        //if so, return the child
                        return child;
                    }
                    nodeStack.Push(child); //push child to node
                }
            }
        }
        return null; //traget not found
    }
}