using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIManagerBehiviour : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI enemyThoughtsText;
    //Reference to Enemy
    public EnemyAI enemyAI;
    // Update is called once per frame
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }
    void Update()
    {
        EnemyAI.States state = enemyAI.GetState();
        UpdateGUI(state);
    }
    void UpdateGUI(EnemyAI.States state)
    {
        stateText.text = "Enemy State: " + state.ToString();
        switch(state)
        {
            case EnemyAI.States.patrol:
            enemyThoughtsText.text = "La La La I am patroling";
            break;
            case EnemyAI.States.chase:
            enemyThoughtsText.text = "HEY YOU GET BACK HERE!";
            break;
            case EnemyAI.States.search:
            enemyThoughtsText.text ="Where did you go?";
            break;
            case EnemyAI.States.attack:
            enemyThoughtsText.text = "Pew Pow Bang!";
            break;
            case EnemyAI.States.retreat:
            enemyThoughtsText.text = "RUN AWAY!";
            break;
            default:
            enemyThoughtsText.text ="Enemy Thoughts";
            break;
        }
    }
}
