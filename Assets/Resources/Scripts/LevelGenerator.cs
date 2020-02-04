using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelGenerator
{
  public static Block.BlockColor[,] GenerateColors(List<int> rankedProbabilities,Block.BlockColor[,] initialGrid = null)   //generat ranked probs
    {
        Block.BlockColor[,] colors;
        int enumSize = (int)Block.BlockColor.Size;
        
        if (initialGrid == null)
        {
            colors = new Block.BlockColor[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    colors[i, j] = (Block.BlockColor)Random.Range(0, enumSize);
        }
        else colors = initialGrid;

        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                if (Random.Range(1, rankedProbabilities[(int)colors[i, j]]) != 1)
                    colors[i, j] = Block.BlockColor.Clear;

                if (colors[i, j] == Block.BlockColor.Clear)
                    colors[i, j] = UpdateColor(rankedProbabilities[0]);
            }

        return colors;
    }

    public static float[,] GenerateSeverities(int severityChange, float[,] initialSeverities = null)
    {
        float[,] severities;
        if (initialSeverities == null)
        {
            severities = new float[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    severities[i, j] = Random.Range(0.5f, 1);
        }
        else severities = initialSeverities;
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
            {
                if (Random.Range(0, severityChange) != 1)
                    severities[i, j] = Random.Range(0.5f,1f);
            }
        return severities;
    }

    public static Block.BlockColor UpdateColor(int probability)
    {
        Block.BlockColor newColor = new Block.BlockColor();

        if (Random.Range(1, probability)  == 2)
            newColor = (Block.BlockColor)Random.Range(0, (int)Block.BlockColor.Size);
        return newColor;
    }
}
