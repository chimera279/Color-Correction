using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManageBars : MonoBehaviour
{
    BlockManager manager;
    Image paintingIntegrity, publicAppreciation;
    Text score,timer;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Painting").GetComponent<BlockManager>();
        paintingIntegrity = transform.Find("PaintingIntegrity").GetComponent<Image>();
        publicAppreciation = transform.Find("Appreciation").GetComponent<Image>();
        score = transform.Find("Score").GetComponent<Text>();
        timer = transform.Find("Timer").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        paintingIntegrity.fillAmount = manager.paintingIntegrity / 500f;
        publicAppreciation.fillAmount = manager.publicAppreciation / 100f;
        score.text = manager.totalScore.ToString();
        timer.text = ((int)manager.remTime / 60).ToString() + ":" + ((int)manager.remTime % 60).ToString();
    }
}
