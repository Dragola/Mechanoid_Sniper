using UnityEngine;
using System.Collections;

public class TargetO : MonoBehaviour {
	public TextMesh HitText;

	void OnCollisionEnter(Collision other)
	{
		HitText.text = "You hit the Outer area";
	}
}
