using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour {
	public CamFollow cam;
	public Rigidbody ball; 
	public Transform firePos;
	public Slider powerSlider; // 힘 게이지
	public AudioSource shootingAudio; 
	public AudioClip fireClip; // 발사 사운드
	public AudioClip chargingClip; // 충전 사운드
	public float minForce = 15f; // 최소 힘
	public float maxForce = 30f; // 최대 힘
	public float chargingTime = 1f; // 최대 충전 시간
	private float currentForce; // 현재 힘
	private float chargeSpeed; // 충전 속도
	private bool fired; // 발사 되었는지 여부
	void OnEnable()
	{
		currentForce = minForce;
		powerSlider.value = currentForce;
		fired = false;
	}

	// 충전 속도 계산
	private void Start() {
		chargeSpeed = (maxForce - minForce)/chargingTime; 
	}

	// 발사체 충전
	private void Update() {
		if(fired) {
			return;
		}

		if(currentForce >= maxForce && !fired) { // 최대 파워일 경우 바로 발사
			currentForce = maxForce;
			Fire();
		} else if(Input.GetButtonDown("Jump")) { // 처음 누른 경우 사운드 재생
			currentForce = minForce;
			shootingAudio.clip = chargingClip;
			shootingAudio.Play();
		} else if(Input.GetButton("Jump") && !fired) { // 누르고 있으면 force 충전
			currentForce += chargeSpeed * Time.deltaTime;
			powerSlider.value = currentForce;
		} else if(Input.GetButtonUp("Jump") && !fired) { // 때면 발사
			Fire();
		}
	}

	// 발사
	private void Fire() {
		fired = true;
		Rigidbody ballinstance = Instantiate(ball, firePos.position, firePos.rotation);
		ballinstance.velocity = currentForce * firePos.forward;
		shootingAudio.clip = fireClip;
		shootingAudio.Play(); // 발사 사운드 재생
		powerSlider.value = minForce; // 게이지 초기화
		currentForce = minForce; // 힘 초기화
		cam.SetTarget(ballinstance.transform, CamFollow.State.Tracking); // 카메라 타겟, 상태 변경
	}
}
