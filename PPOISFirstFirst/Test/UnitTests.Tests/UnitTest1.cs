using Xunit;
using PPOISFirstFirst;
using System;

namespace VectorTests
{
    public class VectorTests
    {
        [Fact]
        public void Constructor_WithThreeCoordinates_SetsPropertiesCorrectly()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            Assert.Equal(1.0, vector.X);
            Assert.Equal(2.0, vector.Y);
            Assert.Equal(3.0, vector.Z);
        }

        [Fact]
        public void Constructor_WithSixCoordinates_CalculatesDifferencesCorrectly()
        {
            var vector = new Vector(1.0, 4.0, 2.0, 6.0, 3.0, 9.0);
            Assert.Equal(3.0, vector.X);
            Assert.Equal(4.0, vector.Y);
            Assert.Equal(6.0, vector.Z);
        }

        [Fact]
        public void Addition_TwoVectors_ReturnsCorrectSum()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(4.0, 5.0, 6.0);
            var result = a + b;
            Assert.Equal(5.0, result.X);
            Assert.Equal(7.0, result.Y);
            Assert.Equal(9.0, result.Z);
        }

        [Fact]
        public void Addition_WithZeroVector_ReturnsSameVector()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var zero = new Vector(0.0, 0.0, 0.0);
            var result = a + zero;
            Assert.Equal(a.X, result.X);
            Assert.Equal(a.Y, result.Y);
            Assert.Equal(a.Z, result.Z);
        }

        [Fact]
        public void Subtraction_TwoVectors_ReturnsCorrectDifference()
        {
            var a = new Vector(5.0, 7.0, 9.0);
            var b = new Vector(1.0, 2.0, 3.0);
            var result = a - b;
            Assert.Equal(4.0, result.X);
            Assert.Equal(5.0, result.Y);
            Assert.Equal(6.0, result.Z);
        }

        [Fact]
        public void Subtraction_FromZeroVector_ReturnsNegativeVector()
        {
            var zero = new Vector(0.0, 0.0, 0.0);
            var a = new Vector(1.0, 2.0, 3.0);
            var result = zero - a;
            Assert.Equal(-1.0, result.X);
            Assert.Equal(-2.0, result.Y);
            Assert.Equal(-3.0, result.Z);
        }

        [Fact]
        public void CrossProduct_PerpendicularVectors_ReturnsCorrectVector()
        {
            var i = new Vector(1.0, 0.0, 0.0);
            var j = new Vector(0.0, 1.0, 0.0);
            var result = i * j;
            Assert.Equal(0.0, result.X);
            Assert.Equal(0.0, result.Y);
            Assert.Equal(1.0, result.Z);
        }

        [Fact]
        public void CrossProduct_AnticommutativeProperty()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(4.0, 5.0, 6.0);
            var result1 = a * b;
            var result2 = b * a;
            Assert.Equal(-result1.X, result2.X);
            Assert.Equal(-result1.Y, result2.Y);
            Assert.Equal(-result1.Z, result2.Z);
        }

        [Fact]
        public void CrossProduct_ParallelVectors_ReturnsZeroVector()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(2.0, 4.0, 6.0);
            var result = a * b;
            Assert.Equal(0.0, result.X);
            Assert.Equal(0.0, result.Y);
            Assert.Equal(0.0, result.Z);
        }

        [Fact]
        public void ScalarMultiplication_ValidScalar_ReturnsScaledVector()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            double scalar = 2.0;
            var result = vector * scalar;
            Assert.Equal(2.0, result.X);
            Assert.Equal(4.0, result.Y);
            Assert.Equal(6.0, result.Z);
        }

        [Fact]
        public void ScalarMultiplication_CommutativeProperty()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            double scalar = 2.0;
            var result1 = vector * scalar;
            var result2 = scalar * vector;
            Assert.Equal(result1.X, result2.X);
            Assert.Equal(result1.Y, result2.Y);
            Assert.Equal(result1.Z, result2.Z);
        }

        [Fact]
        public void ScalarMultiplication_ByZero_ReturnsZeroVector()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            double scalar = 0.0;
            var result = vector * scalar;
            Assert.Equal(0.0, result.X);
            Assert.Equal(0.0, result.Y);
            Assert.Equal(0.0, result.Z);
        }

        [Fact]
        public void ScalarMultiplication_ByNegative_ReturnsNegativeVector()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            double scalar = -1.0;
            var result = vector * scalar;
            Assert.Equal(-1.0, result.X);
            Assert.Equal(-2.0, result.Y);
            Assert.Equal(-3.0, result.Z);
        }

        [Fact]
        public void Division_ValidScalar_ReturnsCorrectVector()
        {
            var vector = new Vector(4.0, 6.0, 8.0);
            double scalar = 2.0;
            var result = vector / scalar;
            Assert.Equal(2.0, result.X);
            Assert.Equal(3.0, result.Y);
            Assert.Equal(4.0, result.Z);
        }

        [Fact]
        public void Division_ByOne_ReturnsSameVector()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            double scalar = 1.0;
            var result = vector / scalar;
            Assert.Equal(vector.X, result.X);
            Assert.Equal(vector.Y, result.Y);
            Assert.Equal(vector.Z, result.Z);
        }

        [Fact]
        public void Division_ByZero_ThrowsException()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            double scalar = 0.0;
            Assert.Throws<DivideByZeroException>(() => vector / scalar);
        }

        [Fact]
        public void DotProduct_PerpendicularVectors_ReturnsZero()
        {
            var i = new Vector(1.0, 0.0, 0.0);
            var j = new Vector(0.0, 1.0, 0.0);
            var result = i ^ j;
            Assert.Equal(0.0, result, 10);
        }

        [Fact]
        public void DotProduct_ParallelVectors_ReturnsOne()
        {
            var a = new Vector(2.0, 0.0, 0.0);
            var b = new Vector(3.0, 0.0, 0.0);
            var result = a ^ b;
            Assert.Equal(1.0, result, 10);
        }

        [Fact]
        public void DotProduct_CommutativeProperty()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(4.0, 5.0, 6.0);
            var result1 = a ^ b;
            var result2 = b ^ a;
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void GreaterThan_XComponentGreater_ReturnsTrue()
        {
            var a = new Vector(5.0, 3.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a > b);
        }

        [Fact]
        public void GreaterThan_YComponentGreater_ReturnsTrue()
        {
            var a = new Vector(4.0, 4.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a > b);
        }

        [Fact]
        public void GreaterThan_ZComponentGreater_ReturnsTrue()
        {
            var a = new Vector(4.0, 3.0, 3.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a > b);
        }

        [Fact]
        public void LessThan_XComponentLess_ReturnsTrue()
        {
            var a = new Vector(3.0, 3.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a < b);
        }

        [Fact]
        public void LessThan_YComponentLess_ReturnsTrue()
        {
            var a = new Vector(4.0, 2.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a < b);
        }

        [Fact]
        public void LessThan_ZComponentLess_ReturnsTrue()
        {
            var a = new Vector(4.0, 3.0, 1.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a < b);
        }

        [Fact]
        public void GreaterThanOrEqual_EqualVectors_ReturnsTrue()
        {
            var a = new Vector(4.0, 3.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a >= b);
        }

        [Fact]
        public void GreaterThanOrEqual_GreaterVector_ReturnsTrue()
        {
            var a = new Vector(5.0, 3.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a >= b);
        }

        [Fact]
        public void LessThanOrEqual_EqualVectors_ReturnsTrue()
        {
            var a = new Vector(4.0, 3.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a <= b);
        }

        [Fact]
        public void LessThanOrEqual_LesserVector_ReturnsTrue()
        {
            var a = new Vector(3.0, 3.0, 2.0);
            var b = new Vector(4.0, 3.0, 2.0);
            Assert.True(a <= b);
        }

        [Fact]
        public void Comparison_WithZeroVector_WorksCorrectly()
        {
            var zero = new Vector(0.0, 0.0, 0.0);
            var nonZero = new Vector(1.0, 0.0, 0.0);
            Assert.True(nonZero > zero);
            Assert.False(zero > nonZero);
        }

        [Fact]
        public void Operations_WithNegativeComponents_WorkCorrectly()
        {
            var a = new Vector(-1.0, -2.0, -3.0);
            var b = new Vector(2.0, 3.0, 4.0);
            var sum = a + b;
            var diff = a - b;
            Assert.Equal(1.0, sum.X);
            Assert.Equal(1.0, sum.Y);
            Assert.Equal(1.0, sum.Z);
            Assert.Equal(-3.0, diff.X);
            Assert.Equal(-5.0, diff.Y);
            Assert.Equal(-7.0, diff.Z);
        }

        [Fact]
        public void Addition_IsCommutative()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(4.0, 5.0, 6.0);
            var result1 = a + b;
            var result2 = b + a;
            Assert.Equal(result1.X, result2.X);
            Assert.Equal(result1.Y, result2.Y);
            Assert.Equal(result1.Z, result2.Z);
        }

        [Fact]
        public void Addition_IsAssociative()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(4.0, 5.0, 6.0);
            var c = new Vector(7.0, 8.0, 9.0);
            var result1 = (a + b) + c;
            var result2 = a + (b + c);
            Assert.Equal(result1.X, result2.X);
            Assert.Equal(result1.Y, result2.Y);
            Assert.Equal(result1.Z, result2.Z);
        }

        [Fact]
        public void ScalarMultiplication_DistributiveOverAddition()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(4.0, 5.0, 6.0);
            double scalar = 2.0;
            var result1 = scalar * (a + b);
            var result2 = (scalar * a) + (scalar * b);
            Assert.Equal(result1.X, result2.X);
            Assert.Equal(result1.Y, result2.Y);
            Assert.Equal(result1.Z, result2.Z);
        }

        [Fact]
        public void CrossProduct_DistributiveOverAddition()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(4.0, 5.0, 6.0);
            var c = new Vector(7.0, 8.0, 9.0);
            var result1 = a * (b + c);
            var result2 = (a * b) + (a * c);
            Assert.Equal(result1.X, result2.X);
            Assert.Equal(result1.Y, result2.Y);
            Assert.Equal(result1.Z, result2.Z);
        }

        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            var vector = new Vector(1.0, 2.0, 3.0);
            var result = vector.ToString();
            Assert.Contains("X: 1", result);
            Assert.Contains("Y: 2", result);
            Assert.Contains("Z: 3", result);
            Assert.Contains("Length", result);
        }

        [Fact]
        public void Equals_ReturnsTrueForEqualVectors()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(1.0, 2.0, 3.0);
            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_ReturnsFalseForDifferentVectors()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(1.0, 2.0, 4.0);
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void GetHashCode_ReturnsSameForEqualVectors()
        {
            var a = new Vector(1.0, 2.0, 3.0);
            var b = new Vector(1.0, 2.0, 3.0);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
    }
}