using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
	public UnityEvent onReset; 
	public static GameManager instance; // 게임 매니저 인스턴스는 1개만(싱글톤)
	public GameObject readyPanel; // 게임 준비 패널
	public Text scoreText;
	public Text bestScoreText;
	public Text messageText;
	public bool isRoundActive = false; // 라운드 진행 여부 
	private int score = 0; // 현재 점수
	public ShooterRotator shooterRotator;
	public CamFollow cam; // 카메라

	// [생성자] 싱글톤 패턴, UI 업데이트
	void Awake() {
		instance = this;
		UpdateUI();
	}

	// 코루틴 시작
	private void Start() {
		StartCoroutine("RoundRoutine");
	}

	// 점수 누적 
	public void AddScore(int newScore) {
		score += newScore;
		UpdateBestScore();
		UpdateUI();
	}

	// 최고점수 < 현재점수인 경우 새로 저장
	private void UpdateBestScore() {
		if(GetBestScore() < score) {
			PlayerPrefs.SetInt("bestscore", score);
		}
	}

	// 저장된 최고점수 반환
	private int GetBestScore(){
		return PlayerPrefs.GetInt("bestscore");
	}

	// UI 업데이트 
	private void UpdateUI() {
		scoreText.text = "Score: " + score; // 텍스트 변경
		bestScoreText.text = "Best Score: " + GetBestScore();
	}
	
	// 발사체 제거(라운드 종료)
	public void OnBallDestroy() {
		UpdateUI(); // UI 업데이트
		isRoundActive = false; // 라운드 종료
	}

	// 게임 리셋
	public void Reset() {
		score = 0; // 점수 0으로 설정
		UpdateUI(); // UI 재설정
		StartCoroutine("RoundRoutine"); // 코루틴 실행
	}
	
	// Escape 버튼이 눌리면 종료
	private void Update() {
		if(Input.GetButtonDown("Cancel")) {
			Application.Quit();
		}
	}

	// 코루틴
	IEnumerator RoundRoutine() {
		onReset.Invoke(); // 이벤트 발생
		readyPanel.SetActive(true); // 게임 준비 패널 보이기 
		cam.SetTarget(shooterRotator.transform, CamFollow.State.Idle); // 카메라 타겟, 상태 변경
		shooterRotator.enabled = false; // 회전 스크립트 비활성화
		isRoundActive = false; // 라운드 시작 전
		messageText.text = "Ready"; // UI 텍스트 변경
		yield return new WaitForSeconds(3f); // 3초 후 시작

		isRoundActive = true; // 라운드 시작 
		readyPanel.SetActive(false); // 게임 준비 패널 숨기기
		shooterRotator.enabled = true; // 회전 스크립트 활성화
		cam.SetTarget(shooterRotator.transform, CamFollow.State.Ready); // 카메라 타겟, 상태 변경
		while(isRoundActive == true) { // 라운드가 종료될 때 까지 무한 반복
			yield return null;
		}
		
		readyPanel.SetActive(true); // 게임 준비 패널 보이기 
		shooterRotator.enabled = false; // 회전 스크립트 비활성화
		messageText.text = "Wait for next round"; // UI 텍스트 변경
		yield return new WaitForSeconds(3f); // 3초 후 리셋
		Reset(); // 리셋
	}
}
