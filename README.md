- # 🕹️ PawPawRush(포포러쉬) — 나만알조 팀

프로젝트 소개 > 유니티 3D 달리기 게임 입니다. 간결한 조작과 명확한 피드백(스코어·업적)을 통해 러닝 커브를 낮추고, 라운드 클리어 중심의 리플레이성을 설계한 팀 프로젝트입니다.
---
## 📅 개발 기간
- 시작: 2025.08.14  
- 종료: 2025.08.22  

## 🧰 개발 환경
- **Engine**: Unity 2022.3.17f1 (LTS)
- **Language**: C#
- **IDE**: JetBrains Rider / Visual Studio 2022
- **Target**: Windows (PC) *(선택적으로 Android/iOS 확장 가능)*
- **Version Control**: Git + GitHub
- **협업**: GitHub

## 👥 멤버 구성
| 역할 | 이름/깃허브 | 담당 |
|---|---|---|
| 팀리더 · 아이템, 스코어(데이터) | 이현수 | 팀전반 일정, 요구사항 관리, 아이템기능과 스코어 연산 |
| 맵담당 | 정희찬 | MapManager담당,무한랜덤 맵생성, 장애물과 아이템 랜덤위치 생성 로직 |
| 전반적인 UI/UX, PPT, 영상, 기획 | 주슬기 | 게임 기획 전반담당(게임 워크플로우& 게임컨셉, UI와이어프레임 제작, 아이템효과, 케릭터조작법), UI 디자인 및 상호작용 연동 |
| 플레이어, 플레이어 커스터마이징 | 차주원 | PlayerManager 담당, 플레이어 조작(점프, 슬라이드, 좌우 3레일 움직임), 플레이어와 장애물& 아이템 상호작용(효과발동, 피격등)|
| 게임매니저, 업적시스템 | 이영신 | 게임에 필요한 매니저들 제작& 관리& 연동 담당(UI, Sound, Resource, Pool, Evenet, Achievenment 등), 업적시스템 제작(업적데이터와 UI 상호작용 연동)|

---

## 🧩 게임 주요 기능
- <img width="767" height="433" alt="image" src="https://github.com/user-attachments/assets/11478e44-c9fb-4a1b-8c20-5b88d7e53c38" />

### 1) 코어 루프 (무한 달리기)
- **코인 = 점수**: 코인 획득 시 즉시 점수 누적, UI에 상시 노출
- **체력 3칸**: 장애물 충돌마다 1칸 감소 → 3회 충돌 시 **게임오버**
- **리트라이**: 게임오버 후 즉시 재시작(무한 러닝 사이클)

### 2) 조작법
- **A / D**: 좌·우 이동  
- **Space**: 점프  
- **Left Ctrl**: 슬라이드

### 3) UI/UX
- **커스터마이징 UI**: 외형 변경 상호작용 연동
- **토스트 알림**: 업적 조건 충족 시 **오른쪽 하단** 토스트 노출
- **볼륨 조절**: 설정 UI에서 전체 볼륨 슬라이더 제공

### 4) 오디오 & 데이터
- **Sound Manager**: **ScriptableObject** 기반으로 사운드 데이터 등록·관리
- **런타임 제어**: UI → Sound Manager 연동으로 볼륨/뮤트 실시간 반영

### 5) 아이템 종류
- **스피드 부스터 딸기**: 마시면 일정 시간 속도 1.5~2배. 무적
- **점수 보너스 크림케이크**: 먹으면 3~5초 동안 점수 2배
- **회복/HP 증가 딸기 타르트**: HP 한칸 회복

### 6) 캐릭터 커스터마이징
- <img width="300" height="250" alt="image" src="https://github.com/user-attachments/assets/72ae7241-0b4c-418e-99ad-e7303cbad14c" />
- 오른쪽 모자들을 누르면 해당 모자에 맞는 에셋이 왼쪽 케릭터 프리팹에 부착되어 보여짐
  
---

## 📦 활용 에셋 목록 (Assets Used)
- **캐릭터** : https://assetstore.unity.com/packages/3d/characters/animals/little-friends-cartoon-animals-lite-262505#content
- **맵&장애물&아이템** : https://kxrejii.itch.io/bakery-asset-pack , https://fertile-soil-productions.itch.io/modular-platformer , https://thecloudy.itch.io/cute-low-poly-caf-assets, https://tinytreats.itch.io/baked-goods

---

## ⬇️ 다운로드 (Builds)

### 최신 버전
- https://drive.google.com/file/d/1UaQHjm9vFGYvXRNMmh5_TUUKApIVou4P/view?usp=sharing

### 설치/실행 안내
1. ZIP을 다운로드 후 **압축 해제**  
2. (Windows) `PawPawRush.exe` 실행  
4. 방화벽/SmartScreen 경고가 뜨면 **실행 허용** 후 진행


## 🚀 실행 & 빌드
```bash
# 의존성 없음 (Unity 에디터에서 바로 실행)
# 빌드: File > Build Settings > PC, Mac & Linux Standalone (Windows)
# 권장: IL2CPP + Release 모드, Development Build OFF

