using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockColor {Clear,Red, Blue, Magenta, Green, Yellow, Cyan, Size };
    public BlockColor color;
    public float severity;
    public int inverseColorChance;

    public bool beingPainted,wrongColor;

    Renderer colorMat;
    AudioSource source;
    BlockManager blockManager;
    public ParticleSystem[] paintSplash;
    public GameObject splashObject,blob;

    // Start is called before the first frame update
    public void Initialize(BlockColor _color, float _severity, int _inverseColorChance, BlockManager _blockManager)
    {
        color = _color;
        severity = _severity;
        inverseColorChance = _inverseColorChance;
        colorMat = GetComponent<Renderer>();
        colorMat.material.color = EnumToColor(color);
        source = GetComponent<AudioSource>();
        blockManager = _blockManager;
        blockManager.rankedProbabilities[(int)color] %= 8;
        blockManager.rankedProbabilities[(int)color] += 1;

        splashObject.SetActive(false);
        beingPainted = false;
        wrongColor = false;

        paintSplash = splashObject.GetComponentsInChildren<ParticleSystem>();
        if (color != BlockColor.Clear)
            InitializePaintSplash();
    }

    // Update is called once per frame
    public void Refresh()
    {
        Color severeColor = colorMat.material.color;
        severeColor.a = severity;
        colorMat.material.color = severeColor;

        if (beingPainted)
        {
            if (color != BlockColor.Clear)
            {
                source.enabled = true;
                source.clip = blockManager.controller.painting;
                source.Play();
                severity -= Time.deltaTime;
                if (severity <= 0)
                {
                    severity = 0;
                    color = BlockColor.Clear;
                    blockManager.paintingIntegrity += 27f;
                }
            }
        }

        if(wrongColor)
        {
            if (color != BlockColor.Clear)
            {
                severity += Time.deltaTime;
                source.enabled = true;
                source.clip = blockManager.controller.wrongColor;
                source.Play();
                if (severity > 1)
                {

                    color = (BlockColor)(BlockColor.Size - color);
                    colorMat.material.color = EnumToColor(color);
                    severity = Random.Range(0.5f, 1);
                    blockManager.paintingIntegrity -= 27f;

                }
            }
        }

        if(!blockManager.controller.paintPressed)
        {
            beingPainted = false;
            wrongColor = false;
            source.Stop();
            source.enabled = false;
        }

        if (color == BlockColor.Clear)
        {
            splashObject.SetActive(false);
            colorMat.enabled = false;
        }
        else
        {
            colorMat.enabled = true;
        }
        ColorBlob();
        UpdateSplash();
    }

    Color EnumToColor(Block.BlockColor _color)
    {
        Color color = new Color();
        switch(_color)
        {
            case Block.BlockColor.Clear:
                color = Color.white;
                color.a = 0;
                break;
            case Block.BlockColor.Red:
                color = Color.red;
                break;
            case Block.BlockColor.Blue:
                color = Color.blue;
                break;
            case Block.BlockColor.Yellow:
                color = Color.yellow;
                break;
            case Block.BlockColor.Green:
                color = Color.green;
                break;
            case Block.BlockColor.Cyan:
                color = Color.cyan;
                break;
            case Block.BlockColor.Magenta:
                color = Color.magenta;
                break;
            default:
                Debug.LogError("Error : Enum does not exist!");
                break;
        }
        return color;
    }

    void InitializePaintSplash()
    {
        splashObject.SetActive(true);
        foreach (var p in paintSplash)
        {
            p.GetComponent<Renderer>().material.color = colorMat.material.color;
            p.GetComponent<Renderer>().material.SetColor("_TintColor", colorMat.material.color);
        }
       // paintSplash.rend
    }

    void ColorBlob()
    {
        var renderer = blob.GetComponent<Renderer>();
        renderer.material.color = colorMat.material.color;
        if (color == BlockColor.Clear)
            renderer.enabled = false;
        else renderer.enabled = true;
    }

    void UpdateSplash()
    {
        foreach (var p in paintSplash)
        {
            p.GetComponent<Renderer>().material.color = colorMat.material.color;
            p.GetComponent<Renderer>().material.SetColor("_TintColor", colorMat.material.color);
        }
    }
}
