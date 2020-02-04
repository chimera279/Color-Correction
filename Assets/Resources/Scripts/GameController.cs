using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Block.BlockColor activeColor;
    public bool paintPressed;
    public AudioClip painting, wrongColor;

    public Texture2D[] cursorTextures;
    ColorToMusic musicObj;

    // Start is called before the first frame update
    public void Start()
    {
        activeColor = (Block.BlockColor) Random.Range(1, (int)Block.BlockColor.Size);
        musicObj = gameObject.GetComponentInChildren<ColorToMusic>();
        Cursor.SetCursor(cursorTextures[(int)activeColor - 1], Vector2.zero, CursorMode.Auto);
        painting = Resources.Load<AudioClip>("Sounds/Painting");
        wrongColor = Resources.Load<AudioClip>("Sounds/WrongColor");
    }

    // Update is called once per frame
    public void Update()
    {
        SetActiveColor();
        musicObj.transpose = musicObj.notes[(int)activeColor];
        if (Input.GetMouseButton(0))
        {
            paintPressed = true;
            CheckIfPainting();
        }
        else
        {
            paintPressed = false;
        }
    }

    public void SetActiveColor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))       //Red
            activeColor = (Block.BlockColor)1;
        if (Input.GetKeyDown(KeyCode.Alpha2))       //Blue
            activeColor = (Block.BlockColor)2;
        if (Input.GetKeyDown(KeyCode.Alpha3))       //Magenta
            activeColor = (Block.BlockColor)3;
        if (Input.GetKeyDown(KeyCode.Alpha4))       //Green
            activeColor = (Block.BlockColor)4;
        if (Input.GetKeyDown(KeyCode.Alpha5))       //Yellow
            activeColor = (Block.BlockColor)5;
        if (Input.GetKeyDown(KeyCode.Alpha6))       //Cyan
            activeColor = (Block.BlockColor)6;

        Cursor.SetCursor(cursorTextures[(int)activeColor-1], Vector2.zero, CursorMode.Auto);

    }

    public void SetButtonPressColor(int button)
    {
        activeColor = (Block.BlockColor)button;
    }

    void CheckIfPainting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Block target;

        if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Block")))
        {
            target = hit.transform.GetComponent<Block>();
            if (activeColor == target.color)
            {
                target.beingPainted = true;
            }
            else
            {
                target.wrongColor = true;
            }
        }
    }
}
