# Player 4단계 계층 아키텍처

## 목표
플레이어 스탯 변경과 UI 갱신의 의존성 방향을 명확하게 정의한다.

1. `ResourceValue`
2. `PlayerStat`
3. `PlayerController`
4. `UI` (`PlayerStatusUIAbility`)

이 구조의 핵심은 도메인 규칙은 도메인 계층에, 이벤트 발행은 컨트롤러 계층에 두는 것이다.

## 계층별 책임

### 1) ResourceValue (값 객체)
파일: `Assets/02.Scripts/Player/Core/ResourceValue.cs`

- 제한이 있는 자원 상태(`Current`, `Max`)를 소유한다.
- 도메인 규칙을 적용한다.
  - 값을 `[0, Max]`로 Clamp
  - 값이 실제로 바뀌지 않으면 무시
- 다음 API를 제공한다.
  - `SetCurrent(float value)`
  - `Add(float delta)`
  - `Normalized`

`ResourceValue`는 컨트롤러, UI, 입력, 네트워크를 알지 않는다.

### 2) PlayerStat (도메인 집합)
파일: `Assets/02.Scripts/Player/Core/PlayerStat.cs`

- 도메인 값을 조합한다.
  - `Health` (`ResourceValue`)
  - `Stamina` (`ResourceValue`)
- 스탯 수준 동작을 제공한다.
  - `SetHealth`
  - `SetStamina`
  - `AddStamina`
  - `EnsureInitialized`
- 그 외 스탯 필드(`MoveSpeed`, `RunSpeed`, `RunStaminaUsage` 등)를 보관한다.

`PlayerStat`은 어떤 변경이 유효한지 정의하지만, 외부 시스템용 이벤트는 발행하지 않는다.

### 3) PlayerController (애플리케이션/오케스트레이션)
파일: `Assets/02.Scripts/Player/Core/PlayerController.cs`

- 이벤트 발행을 담당한다.
  - `event Action<PlayerStat> OnStatChanged`
- 상태 변경은 `PlayerStat` API를 호출해 적용한다.
- 값이 실제로 바뀐 경우에만 `OnStatChanged`를 발행한다.
- Photon 동기화(`OnPhotonSerializeView`)를 처리하고, 원격 수신 후 필요 시 이벤트를 발행한다.

`PlayerController`는 도메인 상태와 외부 소비자 사이의 경계 역할을 한다.

### 4) UI (표현 계층)
파일: `Assets/02.Scripts/Player/UI/PlayerStatusUIAbility.cs`

- `PlayerController.OnStatChanged`를 구독한다.
- `stat.Health.Normalized`, `stat.Stamina.Normalized`를 읽는다.
- 게이지 fill 값을 갱신한다.
- 도메인 상태를 변경하지 않는다.

UI는 플레이어 스탯에 대해 읽기 전용 소비자다.

## 의존성 방향

허용되는 방향:

`ResourceValue <- PlayerStat <- PlayerController <- UI`

의미:

- `PlayerStat`은 `ResourceValue`에 의존한다.
- `PlayerController`는 `PlayerStat`에 의존한다.
- `UI`는 `PlayerController` 이벤트에 의존한다.

금지되는 방향:

- `ResourceValue` 또는 `PlayerStat`이 `PlayerController`나 `UI`를 아는 것
- UI가 `PlayerStat` 내부 값을 직접 변경하는 것

## 런타임 흐름

### 로컬 스태미나 변경 (달리기 소모/회복)
1. `PlayerMoveAbility`가 스태미나 증감량을 계산한다.
2. `PlayerController.AddStamina(delta)`를 호출한다.
3. `PlayerController`가 `PlayerStat.AddStamina(delta)`를 호출한다.
4. `ResourceValue`가 Clamp 후 값을 반영한다.
5. 값이 바뀌었으면 `PlayerController`가 `OnStatChanged(stat)`를 발행한다.
6. UI가 이벤트를 받아 게이지를 다시 그린다.

### 원격 네트워크 업데이트
1. `PlayerController.OnPhotonSerializeView`에서 health/stamina를 수신한다.
2. `PlayerStat.SetHealth/SetStamina`로 반영한다.
3. 값이 바뀌었으면 `OnStatChanged(stat)`를 발행한다.
4. UI는 normalized 값을 기준으로 다시 그린다.

## 이렇게 분리한 이유

- 도메인 규칙이 한곳에 모인다 (`Clamp`는 `ResourceValue`에 위치).
- 이벤트 생명주기가 한곳에 모인다 (`OnStatChanged`는 `PlayerController`에 위치).
- 표현 계층은 수동적으로 구독/렌더링만 수행한다.
- 테스트와 확장이 쉬워진다.

## 확장 가이드

- 마나, 실드 같은 자원을 추가할 때는 `PlayerStat`에 `ResourceValue`를 추가한다.
- 도메인 동작은 먼저 `PlayerStat`에 추가하고, 외부 진입점은 `PlayerController`에 노출한다.
- 이벤트, 네트워크 같은 외부 부수효과는 `PlayerController`에 둔다.
- UI 로직은 읽기 전용으로 유지한다.
