using UnityEngine;
using System.Collections;

public class TargetC : MonoBehaviour {
	public TextMesh HitText;

	void OnCollisionEnter(Collision other)
	{
		HitText.text = "You hit the Center area";
	}
}
