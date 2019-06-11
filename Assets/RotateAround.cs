﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

	public float speed = 5f;

	private void Update() {
		transform.RotateAround(Vector3.zero,Vector3.up, speed * Time.deltaTime);
	}
}
