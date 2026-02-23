# 📌 코드 리뷰 가이드라인

이 문서는 Unity 기반 C# 프로젝트의 코드 리뷰를 위한 가이드입니다.
모든 PR은 아래 규칙을 기준으로 검토합니다.

---

# 0. 코드 리뷰 봇 기본 지침

## 0.1 리뷰 언어

* 모든 코드 리뷰 요약 및 코멘트는 **한국어(Korean)**로 작성합니다.

## 0.2 필수 검토 항목

모든 PR에 대해 다음을 반드시 검토합니다:

* SOLID 원칙 위반 여부
* 디미터의 법칙(Law of Demeter) 위반 여부
* 계층 구조 위반 여부
* 의존성 방향 위반 여부
* 이벤트 구독/해제 누락 여부
* 런타임 데이터와 설정 데이터 혼재 여부
* 성능상 위험 요소 존재 여부

---

# 1. 아키텍처 규칙

## 1.1 계층 분리 원칙

프로젝트는 최소한 다음 계층을 분리해야 합니다:

* **Presentation (UI / View)**
* **Application / Manager (유스케이스 조정, 상태 변경, 이벤트 발행)**
* **Domain (비즈니스 로직, Value Object, Enum)**
* **Infrastructure (저장소, 네트워크, 외부 SDK 연동)**

### 의존성 규칙

* Presentation → Application → Domain 방향으로만 의존합니다.
* Infrastructure는 Domain 인터페이스를 구현합니다.
* Domain은 Unity 또는 외부 SDK에 의존하지 않습니다.
* 하위 계층은 상위 계층을 참조하면 안 됩니다.

### 리뷰 체크

* ❌ Infrastructure가 UI를 직접 참조
* ❌ Domain이 MonoBehaviour 상속
* ❌ UI가 Repository를 직접 호출
* ❌ 순환 참조

---

## 1.2 Repository 패턴

* 반드시 인터페이스를 먼저 정의합니다.
* Manager는 구현체가 아닌 인터페이스에 의존해야 합니다.
* 저장/로드는 Infrastructure 계층에서만 수행합니다.

```csharp
public interface ICurrencyRepository
{
    UniTask<CurrencySaveData> Load();
    UniTask Save(CurrencySaveData data);
}
```

### 리뷰 체크

* 구현체에 직접 의존하고 있지 않은가?
* Repository에 비즈니스 로직이 들어가 있지 않은가?

---

## 1.3 이벤트 기반 통신

계층 간 결합도를 낮추기 위해 이벤트 기반 통신을 사용합니다.

```csharp
public static event Action OnDataChanged;
```

### 리뷰 체크

* 이벤트 구독 시 해제 코드가 반드시 존재하는가?
* Awake/OnEnable에서 구독하고 OnDisable/OnDestroy에서 해제하는가?
* 직접 참조 대신 이벤트로 통신할 수 있는데 강결합을 만들고 있지는 않은가?

---

## 1.4 설정 데이터 vs 런타임 데이터 분리

### 설정 데이터

* ScriptableObject
* JSON 설정
* 변경 불가 데이터

### 런타임 데이터

* 플레이 중 변경되는 값
* 저장 대상 데이터

### 리뷰 체크

* ScriptableObject 값을 런타임에 수정하고 있지 않은가?
* 설정 데이터와 상태 데이터가 한 클래스에 섞여 있지 않은가?

---

# 2. 디자인 원칙

## 2.1 SOLID 원칙 검토

### SRP (단일 책임 원칙)

* 클래스는 하나의 변경 이유만 가져야 합니다.
* UI + 계산 로직 + 저장 로직이 한 클래스에 섞여 있지 않은가?

### OCP (개방-폐쇄 원칙)

* 기능 추가 시 기존 코드를 수정해야 하는 구조인가?
* switch/case 남용 여부

### LSP

* 하위 타입이 상위 타입을 완전히 대체 가능한가?

### ISP

* 거대한 인터페이스를 강요하고 있지 않은가?

### DIP

* 구체 클래스 대신 인터페이스에 의존하는가?

---

## 2.2 디미터의 법칙

### ❌ 위반 예시

```csharp
manager.Get(type).MetaData.Effects[0].BaseValue;
```

### ✅ 권장

```csharp
manager.GetEffectValue(type);
```

### 리뷰 체크

* 객체 내부 구조를 과도하게 탐색하고 있지 않은가?
* 체이닝이 2 depth 이상 반복되는가?

---

# 3. 명명 규칙

## 3.1 네이밍 컨벤션

| 대상               | 규칙                     |
| ---------------- | ---------------------- |
| Private 필드       | `_camelCase`           |
| SerializeField   | `_camelCase`           |
| Public 속성        | PascalCase             |
| 메서드              | PascalCase             |
| Coroutine        | `MethodName_Coroutine` |
| Interface        | `I` + PascalCase       |
| Enum             | `E` + PascalCase       |
| UI 클래스           | `UI_기능명`               |
| ScriptableObject | 기능명 + `SO`             |

---

## 3.2 클래스 멤버 순서

1. Static 멤버
2. SerializeField
3. Private 필드
4. Properties
5. Unity Lifecycle
6. Public 메서드
7. Private 메서드
8. Coroutine

---

# 4. 코드 스타일

## 필수 규칙

* 접근 제어자 명시
* readonly 적극 사용
* var는 타입 명확할 때만
* Allman 스타일
* 들여쓰기 4 spaces
* 한 줄 한 문장
* XML 주석 작성하지 않음

---

# 5. 비동기 처리 기준

| 상황        | 사용        |
| --------- | --------- |
| I/O       | UniTask   |
| 타이밍 기반 연출 | Coroutine |

### 리뷰 체크

* I/O에 Coroutine 사용 여부
* async UniTaskVoid에 .Forget() 누락 여부

---

# 6. 에러 처리 패턴

## 1️⃣ 단순 성공/실패

```csharp
public bool TrySpend(...)
```

## 2️⃣ 실패 원인 필요

```csharp
public UniTask<LoginResult> TryLogin(...)
```

### 리뷰 체크

* 불필요한 Result 남용 여부
* TryXxx에서 예외 throw 여부

---

# 7. 성능 리뷰 포인트

* Update() 내 할당 발생 여부
* GetComponent 매 프레임 호출 여부
* 문자열 할당 반복 여부
* 오브젝트 풀링 미사용 여부
* LINQ 남용 여부

---

# 8. 리뷰 출력 형식 (권장)

리뷰는 항상 다음 구조를 따릅니다:

```
## 📌 PR 요약
- 무엇을 변경했는지 요약

## ❗ 주요 문제점
- [심각] 계층 위반
- [중간] SOLID 위반
- [경미] 네이밍 문제

## 💡 개선 제안
- 코드 예시 포함

## ✅ 잘된 점
- 설계상 우수한 부분
```

---