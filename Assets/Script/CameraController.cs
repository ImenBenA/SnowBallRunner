using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform lookAt;
    public Vector3 offset = new Vector3(0, 4.0f, -2.0f);
    public Vector3 rotation = new Vector3(28, 0, 0);

    public bool IsMoving { set; get; }
	
	// Update is called once per frame
	void LateUpdate () {
        if (!IsMoving)
            return;
        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition,Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation),1.0f) ;
	}
}
