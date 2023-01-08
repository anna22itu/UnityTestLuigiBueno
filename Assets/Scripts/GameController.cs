using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public const int columns = 6;
    public const int rows = 3;

    public const float Xspace = 3f;
    public const float Yspace = -2f;

    [SerializeField] private MainImageScript startObject;
    [SerializeField] private Sprite[] images;

    private int[] Randomiser (int[] locations)
    {
        int[] array = locations.Clone() as int[];

        for (int i=0; i<array.Length; i++)
        {
            int newArray = array[i];
            int j = Random.Range(i,array.Length);
            array[i] = array[j];
            array[j] = newArray;
        }
        return array;
    }


    private void Start()
    {

        int[] cartas = new int[9];
        for (int a = 0; a < cartas.Length; a++)
        {
            int aleatorio = Random.Range(0, images.Length);
            while (!cartas.Contains(aleatorio) )
            {
                cartas[a] = aleatorio;
            }
        }
        Debug.Log(cartas);

        int[] locations = new int[18];
        int count = 0;
        foreach (int value in cartas)
        {
            locations[count] = value;
            locations[count+1] = value;
            count = count + 2;
        }

        //int[] locations = {0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9};
        locations = Randomiser(locations);

        Vector3 startPosition = startObject.transform.position;

        int index = 0;

        for (int i=0; i < columns; i++)
        {
            for (int j=0; j<rows; j++)
            {
                MainImageScript gameImage;
                if (i == 0 && j == 0)
                {
                    gameImage = startObject;
                }
                else 
                {
                    gameImage = Instantiate(startObject) as MainImageScript;
                }

                //int index = j + columns + i;
                //int id = locations[index];
                //gameImage.ChangeSprite(id, images[id]);

              
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id]);
                index=index +1;

                float positionX = (Xspace*i) + startPosition.x;
                float positionY = (Yspace*j) + startPosition.y;

                gameImage.transform.position = new Vector3(positionX, positionY, startPosition.z);

            }
        }
        
    }

    private MainImageScript firstOpen;
    private MainImageScript secondOpen;

    private int score = 0;
    private int attempts = 20;

    [SerializeField] private TextMesh scoreText;
    [SerializeField] private TextMesh attemptsText;

    public bool canOpen
    {
        get { return secondOpen == null; }
    }

    public void imageOpened(MainImageScript startObject) 
    {
        if (firstOpen == null)
        {
            firstOpen = startObject;
        }
        else 
        {
            secondOpen = startObject;
            StartCoroutine(CheckGuessed());
        }
             
    }

    private IEnumerator CheckGuessed()
    {
        if (firstOpen.spriteId == secondOpen.spriteId)  // Compares the two objects
        {
            score++; // Add Score
            scoreText.text = "Score: " + score;
        }
        else 
        {
            yield return new WaitForSeconds(0.5f); // Start timer

            firstOpen.Close();
            secondOpen.Close();
        }

        attempts--;
        attemptsText.text = "Attempts: " + attempts;

        firstOpen = null;
        secondOpen = null;

        if(attempts==0)
        {
            GameOver();
        }
        if (score == 9)
        {
            GameSucces();
        }
    }

    [SerializeField] private GameObject SuccessBackGround;
    [SerializeField] private GameObject GameOverBackGround;

    
    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        GameOverBackGround.transform.position = new Vector3(0, 0, -20);
    }

    public void GameSucces()
    {
        //Set levelText to display number of levels passed and game over message
        SuccessBackGround.transform.position = new Vector3(0, 0, -20);
    }

}
