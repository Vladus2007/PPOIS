using System;

namespace PPOISFirstFirst
{
    /// <summary>
    /// Представляет трехмерный вектор с координатами X, Y, Z и вычисляемой длиной
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Координата X вектора
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Координата Y вектора
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Координата Z вектора
        /// </summary>
        public double Z { get; private set; }

        /// <summary>
        /// Длина вектора
        /// </summary>
        public double Length { get; private set; }

        private readonly ILength _lengthCalculator;

        /// <summary>
        /// Инициализирует новый экземпляр вектора по двум точкам в пространстве
        /// </summary>
        /// <param name="x1">X-координата первой точки</param>
        /// <param name="x2">X-координата второй точки</param>
        /// <param name="y1">Y-координата первой точки</param>
        /// <param name="y2">Y-координата второй точки</param>
        /// <param name="z1">Z-координата первой точки</param>
        /// <param name="z2">Z-координата второй точки</param>
        public Vector(double x1, double x2, double y1, double y2, double z1, double z2)
            : this(x2 - x1, y2 - y1, z2 - z1, new DefaultLengthCalculator())
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр вектора по координатам
        /// </summary>
        /// <param name="x">X-координата</param>
        /// <param name="y">Y-координата</param>
        /// <param name="z">Z-координата</param>
        public Vector(double x, double y, double z)
            : this(x, y, z, new DefaultLengthCalculator())
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр вектора с пользовательским калькулятором длины
        /// </summary>
        /// <param name="x">X-координата</param>
        /// <param name="y">Y-координата</param>
        /// <param name="z">Z-координата</param>
        /// <param name="lengthCalculator">Калькулятор длины вектора</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если lengthCalculator равен null</exception>
        public Vector(double x, double y, double z, ILength lengthCalculator)
        {
            X = x;
            Y = y;
            Z = z;
            _lengthCalculator = lengthCalculator ?? throw new ArgumentNullException(nameof(lengthCalculator));
            Length = _lengthCalculator.CalculateLength(X, Y, Z);
        }

        /// <summary>
        /// Складывает два вектора
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>Новый вектор, являющийся суммой двух векторов</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        public static Vector operator +(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Вычитает один вектор из другого
        /// </summary>
        /// <param name="a">Вектор, из которого вычитают</param>
        /// <param name="b">Вектор, который вычитают</param>
        /// <returns>Новый вектор, являющийся разностью двух векторов</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        public static Vector operator -(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Вычисляет векторное произведение двух векторов
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>Новый вектор, являющийся векторным произведением</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        public static Vector operator *(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            return new Vector(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        /// <summary>
        /// Умножает вектор на скаляр
        /// </summary>
        /// <param name="vector">Вектор для умножения</param>
        /// <param name="scalar">Скалярное значение</param>
        /// <returns>Новый вектор, умноженный на скаляр</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если вектор равен null</exception>
        public static Vector operator *(Vector vector, double scalar)
        {
            if (vector == null) throw new ArgumentNullException(nameof(vector));

            return new Vector(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Умножает скаляр на вектор
        /// </summary>
        /// <param name="scalar">Скалярное значение</param>
        /// <param name="vector">Вектор для умножения</param>
        /// <returns>Новый вектор, умноженный на скаляр</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если вектор равен null</exception>
        public static Vector operator *(double scalar, Vector vector)
        {
            if (vector == null) throw new ArgumentNullException(nameof(vector));

            return new Vector(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        /// <summary>
        /// Делит вектор на скаляр
        /// </summary>
        /// <param name="vector">Вектор для деления</param>
        /// <param name="scalar">Скалярное значение</param>
        /// <returns>Новый вектор, разделенный на скаляр</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если вектор равен null</exception>
        /// <exception cref="DivideByZeroException">Выбрасывается, если скаляр равен нулю</exception>
        public static Vector operator /(Vector vector, double scalar)
        {
            if (vector == null) throw new ArgumentNullException(nameof(vector));
            if (scalar == 0) throw new DivideByZeroException("Cannot divide by zero");

            return new Vector(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
        }

        /// <summary>
        /// Вычисляет косинус угла между двумя векторами
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>Косинус угла между векторами</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если длина любого из векторов равна нулю</exception>
        public static double operator ^(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (a.Length == 0 || b.Length == 0)
                throw new InvalidOperationException("Cannot calculate cosine for zero-length vectors");

            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z) / (a.Length * b.Length);
        }

        /// <summary>
        /// Проверяет, больше ли один вектор другого по двум координатам при равенстве третьей
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>True если вектор a больше вектора b по двум координатам</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        public static bool operator >(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            int equalCount = (a.X == b.X ? 1 : 0) + (a.Y == b.Y ? 1 : 0) + (a.Z == b.Z ? 1 : 0);

            return equalCount == 2 &&
                   ((a.X > b.X && a.Y == b.Y && a.Z == b.Z) ||
                    (a.Y > b.Y && a.X == b.X && a.Z == b.Z) ||
                    (a.Z > b.Z && a.X == b.X && a.Y == b.Y));
        }

        /// <summary>
        /// Проверяет, меньше ли один вектор другого по двум координатам при равенстве третьей
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>True если вектор a меньше вектора b по двум координатам</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        public static bool operator <(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            int equalCount = (a.X == b.X ? 1 : 0) + (a.Y == b.Y ? 1 : 0) + (a.Z == b.Z ? 1 : 0);

            return equalCount == 2 &&
                   ((a.X < b.X && a.Y == b.Y && a.Z == b.Z) ||
                    (a.Y < b.Y && a.X == b.X && a.Z == b.Z) ||
                    (a.Z < b.Z && a.X == b.X && a.Y == b.Y));
        }

        /// <summary>
        /// Проверяет, больше или равен ли один вектор другого по двум координатам при равенстве третьей
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>True если вектор a больше или равен вектору b по двум координатам</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        public static bool operator >=(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            int equalCount = (a.X == b.X ? 1 : 0) + (a.Y == b.Y ? 1 : 0) + (a.Z == b.Z ? 1 : 0);

            return equalCount >= 2 &&
                   ((a.X >= b.X && a.Y == b.Y && a.Z == b.Z) ||
                    (a.Y >= b.Y && a.X == b.X && a.Z == b.Z) ||
                    (a.Z >= b.Z && a.X == b.X && a.Y == b.Y));
        }

        /// <summary>
        /// Проверяет, меньше или равен ли один вектор другого по двум координатам при равенстве третьей
        /// </summary>
        /// <param name="a">Первый вектор</param>
        /// <param name="b">Второй вектор</param>
        /// <returns>True если вектор a меньше или равен вектору b по двум координатам</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если любой из векторов равен null</exception>
        public static bool operator <=(Vector a, Vector b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            int equalCount = (a.X == b.X ? 1 : 0) + (a.Y == b.Y ? 1 : 0) + (a.Z == b.Z ? 1 : 0);

            return equalCount >= 2 &&
                   ((a.X <= b.X && a.Y == b.Y && a.Z == b.Z) ||
                    (a.Y <= b.Y && a.X == b.X && a.Z == b.Z) ||
                    (a.Z <= b.Z && a.X == b.X && a.Y == b.Y));
        }

        /// <summary>
        /// Возвращает строковое представление вектора
        /// </summary>
        /// <returns>Строка в формате "Vector(X: {X}, Y: {Y}, Z: {Z}, Length: {Length})"</returns>
        public override string ToString()
        {
            return $"Vector(X: {X}, Y: {Y}, Z: {Z}, Length: {Length})";
        }

        /// <summary>
        /// Определяет, равен ли указанный объект текущему вектору
        /// </summary>
        /// <param name="obj">Объект для сравнения</param>
        /// <returns>True если объекты равны, иначе False</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
            }
            return false;
        }

        /// <summary>
        /// Возвращает хэш-код для текущего вектора
        /// </summary>
        /// <returns>Хэш-код, основанный на координатах X, Y, Z</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}