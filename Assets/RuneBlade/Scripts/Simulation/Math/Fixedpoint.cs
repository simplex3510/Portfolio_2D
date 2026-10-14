using System;

namespace RuneBlade.Simulation
{
    /// <summary>
    /// 정수로 소수를 표현하는 고정소수점 숫자(소수부 16비트, 1.0 = 65536).
    /// 시뮬레이션의 위치·속도는 float 대신 이 타입을 쓴다.
    /// float은 CPU·플랫폼·컴파일러에 따라 계산 결과가 미세하게 달라질 수 있어서,
    /// 두 클라이언트가 같은 입력으로 같은 결과를 내야 하는 결정적 시뮬레이션에는 쓸 수 없다.
    /// 정수 연산은 어디서나 결과가 같다.
    ///
    /// 내부 값은 int 하나(Raw)이다. 표현 범위는 약 -32768 ~ +32767.99998, 해상도는 1/65536.
    /// 범위를 넘는 연산은 wrap-around가 일어나며 따로 검사하지 않는다.
    /// (아레나 크기와 속도가 이 범위에 한참 못 미치므로 현재는 검사 비용을 들이지 않는다.)
    /// Raw 값은 스냅샷·해시·직렬화에 그대로 들어간다.
    /// </summary>
    public readonly struct FixedPoint : IEquatable<FixedPoint>, IComparable<FixedPoint>
    {
        /// <summary>소수부 비트 수. 값이 곧 기록 형식이므로 바꾸면 안 된다.</summary>
        public const int FractionBits = 16;

        /// <summary>1.0에 해당하는 Raw 값(65536).</summary>
        public const int OneRawValue = 1 << FractionBits;

        public static readonly FixedPoint Zero = new FixedPoint(0);
        public static readonly FixedPoint One = new FixedPoint(OneRawValue);

        /// <summary>내부 원본 값(실제 숫자 × 65536). 해시·스냅샷·직렬화용.</summary>
        public readonly int RawValue;

        private FixedPoint(int rawValue)
        {
            RawValue = rawValue;
        }

        /// <summary>Raw 값에서 직접 만든다(스냅샷·직렬화 복원, 상수 정의용).</summary>
        public static FixedPoint CreateFromRaw(int rawValue)
        {
            return new FixedPoint(rawValue);
        }

        /// <summary>정수에서 만든다. 예: FromInt(3) = 3.0</summary>
        public static FixedPoint CreateFromInt(int value)
        {
            return new FixedPoint(value << FractionBits);
        }

        /// <summary>
        /// 분수에서 만든다. 예: FromRatio(1, 2) = 0.5, FromRatio(5, 60) = 5/60.
        /// 설정값을 "초당 N" 같은 사람이 읽기 쉬운 형태에서 "틱당"으로 바꿀 때 쓴다.
        /// 나눗셈은 0 방향으로 버림하며(C# long 나눗셈 규칙) 이는 모든 플랫폼에서 같다.
        /// </summary>
        public static FixedPoint CreateFromRatio(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException();
            }

            // 먼저 16비트 올린 뒤 나눠야 소수부 정밀도가 유지된다. 오버플로를 피하려고 long으로 계산한다.
            long scaledNumerator = (long)numerator << FractionBits;
            return new FixedPoint((int)(scaledNumerator / denominator));
        }

        /// <summary>소수부를 버린 정수부(음수는 더 작은 쪽으로 내림). 예: 2.7 → 2, -2.7 → -3</summary>
        public int ToIntFloor()
        {
            // 부호 있는 int의 >> 는 부호를 유지하는 산술 시프트이므로 내림이 된다.
            return RawValue >> FractionBits;
        }

        public static FixedPoint operator +(FixedPoint left, FixedPoint right)
        {
            return new FixedPoint(left.RawValue + right.RawValue);
        }

        public static FixedPoint operator -(FixedPoint left, FixedPoint right)
        {
            return new FixedPoint(left.RawValue - right.RawValue);
        }

        public static FixedPoint operator -(FixedPoint value)
        {
            return new FixedPoint(-value.RawValue);
        }

        /// <summary>정수 배수. 예: 속도 × 방향(-1, 0, +1)에 쓴다. 소수부가 늘어나지 않으므로 시프트가 필요 없다.</summary>
        public static FixedPoint operator *(FixedPoint value, int multiplier)
        {
            return new FixedPoint(value.RawValue * multiplier);
        }

        /// <summary>
        /// 고정소수점끼리의 곱. 두 Raw를 곱하면 소수부가 32비트가 되므로
        /// long으로 곱한 뒤 16비트를 내려서 원래 자리로 되돌린다.
        /// 예: 1.5(98304) × 2.0(131072) = 12884901888 → >> 16 → 196608 = 3.0
        /// </summary>
        public static FixedPoint operator *(FixedPoint left, FixedPoint right)
        {
            long product = (long)left.RawValue * right.RawValue;
            return new FixedPoint((int)(product >> FractionBits));
        }

        public static bool operator < (FixedPoint left, FixedPoint right) => left.RawValue < right.RawValue;

        public static bool operator > (FixedPoint left, FixedPoint right) => left.RawValue > right.RawValue;

        public static bool operator <= (FixedPoint left, FixedPoint right) => left.RawValue <= right.RawValue;

        public static bool operator >= (FixedPoint left, FixedPoint right) => left.RawValue >= right.RawValue;

        public bool Equals(FixedPoint other) => RawValue == other.RawValue;

        public override bool Equals(object obj) => obj is FixedPoint other && Equals(other);

        // int의 해시는 Raw 자신이므로 프로세스·플랫폼에 상관없이 같다(HashCode.Combine과 다른 점).
        public override int GetHashCode() => RawValue;

        public int CompareTo(FixedPoint other) => RawValue.CompareTo(other.RawValue);

        public static bool operator ==(FixedPoint left, FixedPoint right) => left.RawValue == right.RawValue;

        public static bool operator !=(FixedPoint left, FixedPoint right) => left.RawValue != right.RawValue;

        /// <summary>디버그·로그용. double 변환을 쓰므로 시뮬레이션 로직에서 호출하면 안 된다.</summary>
        public override string ToString() => (RawValue / (double)OneRawValue).ToString("F4");
    }
}