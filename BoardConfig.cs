using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardConfig : ScriptableObject {

    public BoardManager.Count wallCount;                    //Range for random walls per level
    public BoardManager.Count foodCount;                    //Range for random food per level
    public BoardManager.Count enemyCount;                   //Range for random enemies per level

    public List<BoardConfigTransition> transitions;          //List of possible transitions from level to level

    public BoardConfig GetNext()
    {
        float totalWeights = transitions.Sum(tr => tr.weight);  //Sums the weights of the transitions in transition list
        float rnd = Random.Range(0, totalWeights);

        float sum = 0f;

        foreach( var transition in transitions)
        {
            sum += transition.weight;
            if (sum >= rnd)
            {
                return transition.target;
            }
        }

        return transitions.Last().target;                   //If no board is found for given rand return highest weighted
    }

}

[System.Serializable]
public class BoardConfigTransition
{
    public BoardConfig target;
    public float weight;

    public BoardConfigTransition(BoardConfig target, float weight)
    {
        this.target = target;
        this.weight = weight;
    }
}