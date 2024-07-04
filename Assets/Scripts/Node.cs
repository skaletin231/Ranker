using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Node
{
    public int randomNumForTeasting = 0;
    public int height = 0;
    public List<Characters> characters = new List<Characters>();
    public Node rightNode = null;
    public Node leftNode = null;
    public Node parentNode = null;

    public Node(Characters character)
    {
        characters.Add(character);
    }

    public Node(int myNum)
    {
        randomNumForTeasting = myNum;
    }

    public int Balance()
    {
        int leftHeight = leftNode == null ? -1 : leftNode.height;
        int rightHeight = rightNode == null ? -1 : rightNode.height;

        return rightHeight - leftHeight;
    }
}
