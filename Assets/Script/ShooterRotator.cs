using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRotator : MonoBehaviour {
    private enum RotateState { // 회전 상태 열거형
        Idle, Vertical, Horizontal, Ready
    }

    private RotateState state = RotateState.Idle; // 현재 상태 
    public float verticalRotateSpeed = 360f; // 1초에 회전할 각도
    public float horizontalRotateSpeed=  360f; // 1초에 회전할 각도
    public BallShooter ballShooter; // 발사 스크립트 저장 변수

    void Update()
    {
        switch(state) {
            case RotateState.Idle: // 대기상태 
                if(Input.GetButtonDown("Jump")) {
                    state = RotateState.Horizontal; // 가로 회전 상태로 전환 
                }
                break;
            case RotateState.Horizontal: // 가로 회전 
                if(Input.GetButton("Jump")) { //누르고 있으면 계속 회전
                    transform.Rotate(new Vector3(0, horizontalRotateSpeed * Time.deltaTime, 0));
                } else if(Input.GetButtonUp("Jump")) { // 누르고 있다가 때면 회전 멈춤 + 세로 회전 상태 전환
                    state = RotateState.Vertical;
                }
                break;
            case RotateState.Vertical: // 세로 회전 
                if(Input.GetButton("Jump")) { // 누르고 있으면 계속 회전
                    transform.Rotate(new Vector3(-verticalRotateSpeed * Time.deltaTime, 0, 0));
                } else if(Input.GetButtonUp("Jump")) { // 누르고 있다가 때면 발사 준비상태
                    state = RotateState.Ready;
                }
                break;
            case RotateState.Ready: // 발사 준비
                ballShooter.enabled = true; // 발사 스크립트 활성화
                break;  
        }
    }

    // 활성화 되었을 때 호출 
    private void OnEnable() {
        transform.rotation = Quaternion.identity; // 회전 초기화
        state = RotateState.Idle; // 회전 상태: 대기
        ballShooter.enabled = false; // 회전이 끝난 후 발사 스크립트 enable
    }
}
