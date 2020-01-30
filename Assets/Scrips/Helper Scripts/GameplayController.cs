using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{

    public static GameplayController instance;
    public GameObject fruit_Pickup;

    private float min_X = 354f, max_X = 362f, min_Y = 86f, max_Y = 90f;
    private float z_Pos = 5.5f;

    private Text score_Text;
    private int scoreCount;


    void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        score_Text = GameObject.Find("Score").GetComponent<Text>();

        Invoke("startSpawning", 0.5f);
    }

    // Update is called once per frame
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void startSpawning()
    {
        StartCoroutine(spawnPickups());
    }

    public void cancelSpawning()
    {
        CancelInvoke("startSpawning");
    }

    IEnumerator spawnPickups()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(fruit_Pickup, 
                new Vector3(Random.Range(min_X, max_X),Random.Range(min_Y, max_Y), z_Pos),
                Quaternion.identity);
                                                  

        }
        Invoke("startSpawning", 0f);
    }

    public void increaseScore()
    {
        scoreCount++;
        score_Text.text = "Score : " + scoreCount;
    }
}
