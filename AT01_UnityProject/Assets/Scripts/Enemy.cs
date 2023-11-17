using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node targetNode;
    private Vector3 currentDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };

    private void DepthFirstSearch()
    {
        //variable for the 'Node currently being searched'
        Node currentNode = GameManager.Instance.Nodes[0];

        //boolean for target found
        bool targetFound = false;

        //List var of type 'Node' storing our unsearched node (our 'stack')
        List<Node> unsearchedNode = new List<Node> ();

        //list var of type 'Node' storing our searched node
        List<Node> searchedNode = new List<Node> ();

        //Add 'currentNode' to the 'stack'
        unsearchedNode.Add(currentNode);

        
        //Debug 'loop limit'
        int debugLoopLimit = 0;

        Node playerCurrentNode = GameManager.Instance.Player.CurrentNode;

        while (targetFound == false)
        {
            if (debugLoopLimit > 100)
            { 
                targetFound = true;
                Debug.Log("Hit the limit without finding our target");
            }

            //check if 'currentnode' is equal to our target destination
            if (currentNode == playerCurrentNode)
            { 
                //assign the value of the 'currentNode' to 'targetNode'
                targetNode = currentNode;
                currentDir = targetNode.transform.position - transform.position;
                currentDir = currentDir.normalized;
                //set 'targetFound' to true to break the loop
                targetFound = true;
                break;
            }

            //iterate though the child nodes of the 'currentNode' and add them to the unsearchedNodes list
            for (int i = 0; i < currentNode.Children.Length; i++)
            {
                Node childNode = currentNode.Children[i];

                if (!unsearchedNode.Contains(childNode))
                {
                    if (!searchedNode.Contains(childNode))
                    {
                        unsearchedNode.Add(childNode);
                    }
                }
            }
            //remove currentNode from the unsearchedNode
            unsearchedNode.Remove(currentNode);

            //add current to the 'searched' list
            searchedNode.Add(currentNode);

            //update the currentNote to the node at the front of the 'queue' (BFS IMPLEMENTION)
            if (unsearchedNode.Count != 0)
            {
                currentNode = unsearchedNode[0];
            }
            else
            {
                Debug.Log("failed to find the target");
                break;
            }
        }



    }
    
    //public Node TargetNode { get; private set; }
    //public Node CurrentNode { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
       InitializeAgent();
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
            if (targetNode != null)
            {
                //If beyond 0.25 units of the current node.
                if (Vector3.Distance(transform.position, targetNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                //Implement path finding here
                else 
                {
                    //call DFS Method to Update the current Node
                    //find new target node

                    //if target node is not the AIs current node is not null
                    //if (Player != currentNode)
                    //{
                        
                    //}

                        //set current node to traget node
                        //else if player traget not null and player traget node not current node
                        
                    //set current node to players target node

                        //if current node is not null
                        //set current direction towards node
                        //normalize current direction

                    DepthFirstSearch();
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
        targetNode = GameManager.Instance.Nodes[0];
        currentDir = targetNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    //Implement DFS algorithm method here
    private Node DepthFristSearch()
    {
        Stack nodeStack = new Stack(); //Stacks the unvisited nodes, last one added to stack is next visited
        List<Node> visitedNodes = new List<Node> (); //track visited nodes
        nodeStack.Push(GameManager.Instance.Nodes[0]); //add root node to stack 

        while (nodeStack.Count > 0) //while stack is not empty
        { 
            Node current = (Node)nodeStack.Pop(); //pop the last node added to stack
            visitedNodes.Add(targetNode); //mark current node as visited
            foreach (Node child in targetNode.Children)
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