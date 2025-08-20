using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // 시간 관련 변수들
    [Tooltip("현재 시간 (0.0 ~ 1.0). 0.5가 정오")]
    [Range(0.0f, 1.0f)]
    public float time; // 0~1 사이의 값으로 표현되는 현재 시간

    // 길이를 늘리고 싶으면 fullDayLength를 조절
    [Tooltip("게임 속 하루의 실제 시간 길이(초 단위)")]
    public float fullDayLength; // 하루의 전체 길이 (초)

    [Tooltip("게임 시작 시의 시간")]
    public float startTime = 0.4f; // 시작 시간

    private float timeRate; // 시간의 흐름 속도 (1 / fullDayLength)

    [Tooltip("정오(시간 = 0.5)일 때의 태양 회전 값. (90, 0, 0)으로 설정하면 태양이 머리 위에 위치")]
    public Vector3 noon; // 정오일 때의 태양 각도


    // 태양 관련 변수들
    [Header("Sun")]
    public Light sun; // 태양 역할을 할 Directional Light
    public Gradient sunColor; // 시간에 따른 태양 색상 변화
    public AnimationCurve sunIntensity; // 시간에 따른 태양 빛의 세기 변화


    // 달 관련 변수들
    [Header("Moon")]
    public Light moon; // 달 역할을 할 Directional Light
    public Gradient moonColor; // 시간에 따른 달 색상 변화
    public AnimationCurve moonIntensity; // 시간에 따른 달 빛의 세기 변화


    // 기타 조명 설정
    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; // 시간에 따른 주변광(Ambient) 강도
    public AnimationCurve reflectionIntensityMultiplier; // 시간에 따른 반사(Reflection) 강도


    // 게임이 시작될 때 한 번 호출
    private void Start()
    {
        // 시간의 흐름 속도를 계산
        timeRate = 1.0f / fullDayLength;
        // 현재 시간을 설정된 시작 시간으로 맞춤
        time = startTime;
    }

    // 매 프레임마다 호출
    private void Update()
    {
        // 시간 업데이트
        // 이전 프레임으로부터 경과된 시간(Time.deltaTime)에 시간 속도를 곱하여 현재 시간을 갱신
        // % 1.0f (나머지 연산)을 통해 시간이 1.0을 넘어가면 다시 0.0으로 순환
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        // 조명 업데이트
        // UpdateLighting 함수를 사용하여 태양과 달의 상태를 각각 업데이트
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // 씬 전역 조명 설정 업데이트
        // RenderSettings를 통해 씬의 전반적인 조명 환경을 제어
        // AnimationCurve에서 현재 시간에 해당하는 값을 가져와 주변광과 반사광의 강도를 설정
        // Evaluate는 Inspector에 그린 그래프에서 time을 입력받으면 특정 값을 return
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    /// <summary>
    /// 광원(태양 또는 달)의 회전, 색상, 강도를 업데이트하는 범용 함수
    /// </summary>
    /// <param name="lightSource">업데이트할 Light 컴포넌트</param>
    /// <param name="colorGradiant">시간에 따른 색상 변화 Gradient</param>
    /// <param name="intensityCurve">시간에 따른 강도 변화 AnimationCurve</param>
    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        // 빛의 강도(밝기)를 AnimationCurve에서 가져옴
        float intensity = intensityCurve.Evaluate(time);

        // 광원의 회전 각도를 계산하고 적용
        //    - 태양은 0.25(일출) 시점을 기준으로, 달은 0.75(월출) 시점을 기준으로 회전
        //    - 이렇게 하면 태양과 달이 서로 반대편에서 뜨고 지게 된다.
        //    - (time - offset) * noon * 4.0f 계산을 통해 0~1 사이의 시간을 360도 회전으로 변환

        // 하루 시간(0 ~ 1)과 해/달의 자전주기(0 ~ 360)의 값을 동기화.
        // 해와 달은 180도 차이가 항상 나기 때문에 0.5f(180/360)의 차이
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;

        // 광원의 색상을 Gradient에서 가져와 적용
        lightSource.color = colorGradiant.Evaluate(time);

        // 광원의 강도를 적용
        lightSource.intensity = intensity;

        // 성능 최적화: 빛의 강도가 0이면 게임 오브젝트를 비활성화
        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        // 반대로 빛의 강도가 0보다 큰데 비활성화 상태이면 다시 활성화
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }
}
