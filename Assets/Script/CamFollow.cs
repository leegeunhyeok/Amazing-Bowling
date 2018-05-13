using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {
	// 카메라 상태
	public enum State {
		Idle, Ready, Tracking
	}

	// 카메라 상태
	private State state {
		set {
			switch(value) {
				case State.Idle:
					targetZoomSize = roundReadyZoomSize;
					break;

				case State.Ready:
					targetZoomSize = readyShotZoomSize;
					break;

				case State.Tracking:
					targetZoomSize = trackingZoomSize;
					break;
			}
		}
	}

	private Transform target; // 카메라가 바라볼 타겟
	public float smoothTime = 0.2f; // 카메라 이동시 0.2초간 지연
	private Vector3 lastMovingVelocity; // 마지막 이동 속도
	private Vector3 targetPosition; // 타겟 위치
	private Camera cam; // 카메라
	private float targetZoomSize = 5f; // 상태에 따른 줌 배율

	private const float roundReadyZoomSize = 14.5f;
	private const float readyShotZoomSize = 5f;
	private const float trackingZoomSize = 10f;

	private float lastZoomSpeed; 

	// [생성자] 자식 컴포넌트에서 카메라 가져오기 
	void Awake()
	{
		cam = GetComponentInChildren<Camera>();
		state = State.Idle; // 상태는 대기 상태
	}

	// 카메라 이동
	private void Move() {
		targetPosition = target.transform.position;
		Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref lastMovingVelocity, smoothTime);
		transform.position = smoothPosition;
	}

	// 카메라 확대
	private void Zoom() {
		float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomSize, ref lastZoomSpeed, smoothTime);
		cam.orthographicSize = smoothZoomSize;
	}

	// Update
	private void FixedUpdate() {
		if(target != null) {
			Move();
			Zoom();
		}
	}

	// 리셋
	public void Reset() {
		state = State.Idle;
	}

	// 카메라 타겟 변경
	public void SetTarget(Transform newTarget, State newState) {
		target = newTarget;
		state = newState;
	}
}
