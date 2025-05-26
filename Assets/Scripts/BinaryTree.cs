using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class BinaryTree : MonoBehaviour
{
    public Node startingNode;
    [SerializeField] List<Characters> characters;
    string toPrint = "";

    [SerializeField] Image char1;
    [SerializeField] TextMeshProUGUI char1Name;

    [SerializeField] Image char2;
    [SerializeField] TextMeshProUGUI char2Name;

    bool makingChoice = false;
    Direction direction = Direction.Tie;
    int maxNodes = 0;
    int amountOfNodesCurrently = 1;

    int comparisonsMade = 0;
    [SerializeField] List<int> nums;

    private void Start()
    {
        maxNodes = characters.Count;
        int randomInt = Random.Range(0, characters.Count);
        Characters character = characters[randomInt];
        characters.RemoveAt(randomInt);

        startingNode = new Node(character);

        //BuildTreeTest();
        StartCoroutine(BuildTree());
    }

    private void BuildTreeTest()
    {
        int randomInt = Random.Range(0, nums.Count);
        int holder = nums[randomInt];
        nums.RemoveAt(randomInt);

        startingNode.randomNumForTeasting = holder;

        while (nums.Count > 0)
        {
            randomInt = Random.Range(0, nums.Count);
            holder = nums[randomInt];
            nums.RemoveAt(randomInt);

            Node toCompare = new Node(holder);
            PlaceNode(toCompare, startingNode);
        }
        Debug.Log(comparisonsMade);
        PrintTree();
    }

    private void PlaceNode(Node toPlace, Node toCheck)
    {
        if (toPlace.randomNumForTeasting > toCheck.randomNumForTeasting)
        {
            if (toCheck.rightNode == null)
            {
                toCheck.rightNode = toPlace;
                toPlace.parentNode = toCheck;
                CheckBallanceAmount(toCheck);
            }
            else
            {
                PlaceNode(toPlace, toCheck.rightNode);
            }
            
        }
        else
        {
            if (toCheck.leftNode == null)
            {
                toCheck.leftNode = toPlace;
                toPlace.parentNode = toCheck;
                CheckBallanceAmount(toCheck);
            }
            else
            {
                PlaceNode(toPlace, toCheck.leftNode);
            }
        }
        comparisonsMade++;
    }

    private IEnumerator BuildTree()
    {
        while (characters.Count > 0)
        {
            int randomInt = Random.Range(0, characters.Count);
            Characters holder = characters[randomInt];
            characters.RemoveAt(randomInt);

            Node currentNode = startingNode;
            yield return MakingChoice(new Node(holder), currentNode);
        }
    }

    private IEnumerator MakingChoice(Node toPlace, Node toCompare)
    {
        if (toCompare == null)
        {
            toCompare = startingNode;
        }
        makingChoice = true;
        char1.sprite = toCompare.characters[0].Image;
        char1Name.text = toCompare.characters[0].Name;
        char2.sprite = toPlace.characters[0].Image;
        char2Name.text = toPlace.characters[0].Name;


        while (makingChoice)
        {
            yield return null;
        }

        switch (direction)
        {
            case Direction.Tie:
                toCompare.characters.Add(toPlace.characters[0]);
                amountOfNodesCurrently++;
                if (amountOfNodesCurrently == maxNodes)
                    PrintTreeByLevel();

                break;

            case Direction.Right:
                if (toCompare.rightNode == null)
                {
                    toCompare.rightNode = toPlace;
                    toPlace.parentNode = toCompare;
                    amountOfNodesCurrently++;
                    CheckBallanceAmount(toPlace);
                    
                    if (amountOfNodesCurrently == maxNodes)
                        PrintTreeByLevel();
                }
                else
                {
                    yield return MakingChoice(toPlace, toCompare.rightNode);
                }

                break;

            case Direction.Left:
                if (toCompare.leftNode == null)
                {
                    toCompare.leftNode = toPlace;
                    toPlace.parentNode = toCompare;
                    amountOfNodesCurrently++;
                    CheckBallanceAmount(toPlace);
                    if (amountOfNodesCurrently == maxNodes)
                        PrintTreeByLevel();
                }
                else
                {
                    yield return MakingChoice(toPlace, toCompare.leftNode);
                }

                break;
        }

        
    }

    private void SetHeight(Node currentNode)
    {
        if (currentNode == null)
            return;
        int leftHeight = currentNode.leftNode == null ? -1 : currentNode.leftNode.height;
        int rightHeight = currentNode.rightNode == null ? -1 : currentNode.rightNode.height;

        currentNode.height = Mathf.Max(leftHeight, rightHeight) + 1;
    }

    private void CheckBallanceAmount(Node newLeaf)
    {
        Node currentNode = newLeaf.parentNode;
        
        while (currentNode != null)
        {       
            //changing height of everything
            SetHeight(currentNode);
            
            if (Mathf.Abs(currentNode.Balance()) > 1)
            {
                if (currentNode.Balance() > 1) //More nodes on right
                {
                    /*if (currentNode.rightNode.Balance() < 0)
                        RotateRight(currentNode.rightNode);*/
                    RotateLeft(currentNode);
                    
                }
                else //more nodes on left
                {
                    /*if (currentNode.leftNode.Balance() > 0)
                        RotateLeft(currentNode.leftNode);*/
                    RotateRight(currentNode);
                }
                currentNode = null;
            }
            else
            {
                currentNode = currentNode.parentNode;
            }
        }
    }

    private void RotateLeft(Node currentNode)
    {
        Node nodeA = currentNode;
        Node nodeB = currentNode.rightNode;
        Node nodeC = nodeB.leftNode;

        nodeB.parentNode = nodeA.parentNode;
        nodeA.parentNode = nodeB;
        if (nodeB.parentNode != null)
        {
            nodeB.parentNode.rightNode = nodeB;
        }

        if (nodeC != null)
            nodeC.parentNode = nodeA;

        nodeA.rightNode = nodeC;
        nodeB.leftNode = nodeA;
        if (nodeA == startingNode)
            startingNode = nodeB;
        
        SetHeight(nodeA);
        SetHeight(nodeC);
        SetHeight(nodeB);
    }
    private void RotateRight(Node currentNode)
    {
        Node nodeA = currentNode;
        Node nodeB = currentNode.leftNode;
        Node nodeC = nodeB.rightNode;

        nodeB.parentNode = nodeA.parentNode;
        nodeA.parentNode = nodeB;
        if (nodeB.parentNode != null)
        {
            nodeB.parentNode.leftNode = nodeB;
        }
        if (nodeC != null)
            nodeC.parentNode = nodeA;
        nodeA.leftNode = nodeC;
        nodeB.rightNode = nodeA;

        if (nodeA == startingNode)
            startingNode = nodeB;

        SetHeight(nodeA);
        SetHeight(nodeC);
        SetHeight(nodeB);
    }

    public void RightButtonClicked()
    {
        direction = Direction.Right;
        makingChoice = false;
    }

    public void LeftButtonClicked()
    {
        direction = Direction.Left;
        makingChoice = false;
    }

    public void MiddleButtonClicked()
    {
        direction = Direction.Tie;
        makingChoice = false;
    }

    private void PrintTree()
    {
        PrintTree(startingNode);
        Debug.Log(toPrint);
    }

    private void PrintTree(Node node)
    {
        if (node == null)
            return;
        PrintTree(node.leftNode);
        toPrint += ", " + node.characters[0].Name;
        //toPrint += ", " + node.randomNumForTeasting;
        PrintTree(node.rightNode);
    }

    private void PrintTreeByLevel()
    {
        //Why? This does nothing
/*        Queue<Node> queue = new Queue<Node>();
        if (startingNode.leftNode != null)
            queue.Enqueue(startingNode.leftNode);
        if (startingNode.rightNode != null)
            queue.Enqueue(startingNode.rightNode);

        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();

            if (node.leftNode != null)
                queue.Enqueue(node.leftNode);
            if (node.rightNode != null)
                queue.Enqueue(node.rightNode);
        }*/
        PrintTree();
    }
}

public enum Direction
{
    Tie,
    Right,
    Left
}
