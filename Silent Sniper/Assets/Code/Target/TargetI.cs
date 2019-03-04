using UnityEngine;
using System.Collections;

public class TargetI : MonoBehaviour {
	public TextMesh HitText;

	void OnCollisionEnter(Collision other)
	{
		HitText.text = "You hit the Inner area";
	}
}
