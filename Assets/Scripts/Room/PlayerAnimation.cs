using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    private float moveY;
    private float moveX;

    public GameObject Photo;

	enum Direction { Down = 1, Up = 2, Right = 3, Left = 4 };

    void Update()
    {
		moveX = Input.GetAxisRaw("Horizontal");
		moveY = Input.GetAxisRaw("Vertical");

		if (!UIController.instance.isShowingDialogueBox)
		{
			if (Input.GetKeyDown (KeyCode.F))
			{
				Invoke ("Photos", 1f);
			}
			if (Input.GetKeyDown (KeyCode.G))
			{
				Photo.SetActive (false);
			}
			if (moveY > 0 && moveX == 0) 
			{
				GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Up);
			}
			else if (moveY < 0 && moveX == 0) 
			{
				GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Down);
			}
			else if (moveX > 0 && moveY == 0) 
			{
				GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Right);
			}
			else if (moveX < 0 && moveY == 0) 
			{
				GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Left);
			} 
			else
			{
				GetComponent<Animator> ().SetInteger ("Direction", 0);
			}
		}
    }

    void Photos()
    {
        Photo.SetActive(true);
    }
}
