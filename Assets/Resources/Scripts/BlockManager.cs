using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{


    public List<Block> blocks;
    public GameObject blockObj;
    public Block.BlockColor[,] colorGrid;
    public float[,] severityGrid;
    public List<int> rankedProbabilities;
    public int severityChangeChance = 2;
    public GameController controller;

    float blockScale = 0.75f;
    public float totalSeverity, paintingIntegrity, publicAppreciation, gameTime,remTime;
    public int activeBlockCount,totalScore;

    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        blockObj = Resources.Load<GameObject>("Prefabs/Block");
        controller = GameObject.FindGameObjectWithTag("Field").GetComponent<GameController>();
        RandomizeRankedProbabilities();
        colorGrid = LevelGenerator.GenerateColors(rankedProbabilities);
        severityGrid = LevelGenerator.GenerateSeverities(severityChangeChance);
        SetupBlocks();
        paintingIntegrity = 500f;
        publicAppreciation = 100f;
        totalScore = 0;
        gameTime = 0f;
        Time.timeScale = 1;
     
    }

    // Update is called once per frame
    void Update()
    {
        int tempBlockCount = 0;
        float tempSeverity = 0;
        foreach(Block b in blocks)
        {
            b.Refresh();
            if (b.color != Block.BlockColor.Clear)
            {
                tempBlockCount++;
                tempSeverity += b.severity;
            }
        }
        
        activeBlockCount = tempBlockCount;
        if(activeBlockCount<3)
        {
            colorGrid = LevelGenerator.GenerateColors(rankedProbabilities, colorGrid);
            RefreshRanks();
            RefreshBlocks();
        }
        totalSeverity = tempSeverity;
        paintingIntegrity -= Time.deltaTime * totalSeverity;
        if (paintingIntegrity > 500f)
            paintingIntegrity = 500f;
        if (paintingIntegrity <= 0f)
        {
            paintingIntegrity = 0f;
        }

        if (!gameOverScreen.active)
        {
            CalculateAppreciation();
            CalculateScore();
        }
        gameTime += Time.deltaTime;
        remTime = 270f - gameTime;
        if(gameTime>270f)
        {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);

        }
        else Time.timeScale = 1;
    }

    void RandomizeRankedProbabilities()
    {
        rankedProbabilities = new List<int> { 1 };
        for (int i = 0; i < System.Enum.GetValues(typeof(Block.BlockColor)).Length; i++)
        {
            int probability = Random.Range(3, 8);    //difficulty goes here later
            rankedProbabilities.Add(probability);
        }
    }

    void SetupBlocks()
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                    Block block = GameObject.Instantiate(blockObj, transform).GetComponent<Block>();
                    block.transform.localPosition = new Vector3(i - 4.5f, 0, j - 4.5f);
                    block.transform.localScale = new Vector3(blockScale, blockScale, blockScale);
                    block.Initialize(colorGrid[i, j], severityGrid[i, j], rankedProbabilities[(int)colorGrid[i, j]], this);
                    blocks.Add(block);
                
               
            }

    }
    
    void RefreshRanks()
    {

    }

    void CalculateAppreciation()
    {
        if(paintingIntegrity<100f)
                publicAppreciation -= 0.1f;
        else if (paintingIntegrity >= 100f && paintingIntegrity < 200f)
            publicAppreciation -= 0.05f;
        else if (paintingIntegrity >= 200f && paintingIntegrity < 400f)           
                publicAppreciation += 0.05f;
        else if (paintingIntegrity >= 400f && paintingIntegrity < 450f)
            publicAppreciation += 0.075f;
        else if (paintingIntegrity >= 450f && paintingIntegrity <= 500f)
            publicAppreciation += 0.1f;
        
        if (publicAppreciation > 100)
            publicAppreciation = 100;
        if (publicAppreciation < 0)
            publicAppreciation = 0;
    }

    void CalculateScore()
    {
        if (publicAppreciation < 20f)
            totalScore -= 1;
        else if (publicAppreciation >= 20f && publicAppreciation < 40f)
            totalScore += 0;
        else if (publicAppreciation >= 40f && publicAppreciation < 50f)
            totalScore += 0;
        else if (publicAppreciation >= 50f && publicAppreciation < 75f)
            totalScore += 1;
        else if (publicAppreciation >= 75f && publicAppreciation <= 90f)
            totalScore += 1;
        else if (publicAppreciation >= 90f && publicAppreciation <= 100f)
            totalScore += 2;

    }

    void RefreshBlocks()
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                blocks[(i * 10) + j].Initialize(colorGrid[i, j], severityGrid[i, j], rankedProbabilities[(int)colorGrid[i, j]], this);
    }
}
