using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	Vector3 angle;

	void Start()
	{
		angle = transform.eulerAngles;
	}

	void Update()
	{
		angle.y += Managers.Time.DeltaTime * 100;
		transform.eulerAngles = angle;
	}

}
