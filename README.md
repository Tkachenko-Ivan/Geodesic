# Geodesic

Библиотека предназначена для решения геометрических задач на выгнутой поверхности сфероида или эллипсоида вращения.

С помощью библиотеки можно:
* [решить прямую геодезическую задачу](#Header1);
* [решить обратную геодезическую задачу](#Header2);
* [найти точку пересечения двух ортодромий, заданных кординатами концов ортодромии](#Header3);
* [зная координаты концов ортодромии, найти значение широты по известной долготе](#Header4);
* [зная координаты концов ортодромии, найти значение долготы по известной широте](#Header4);

[Описание тестовых случаев](https://github.com/Tkachenko-Ivan/Geodesic/tree/master/GeodesicLibrary/GeodesicLibraryTests);

## Термины

Прямой геодезической задачей называют вычисление геодезических координат - широты и долготы точки, лежащей эллипсоиде, по координатам другой точки и по известным длине и дирекционному углу направления, соединяющей эти точки.

Обратная геодезическая задача заключается в определении по геодезическим координатам двух точек на эллипсоиде длины и дирекционного угла направления между этими точками.

Ортодромия - название кратчайшего расстояния между двумя точками на поверхности Земли или другого эллипсоида вращения.

## Примеры

### <a name="Header1"></a> Решение прямой геодезической задачи

Для решения прямой геодезической задачи, неоходимо создать объект класса `DirectProblemService`.

```C#
var directEllipsoid = new DirectProblemService(new Ellipsoid());
var directSpheroid = new DirectProblemService(new Spheroid());
```

В качестве параметра в конструктор следует передать объект реализующий интерфейс `IEllipsoid`, в котором задаются полярный и экваториальный радиус, а так же коэффициент полярного сжатия.

Для решения прямой задачи вызвать метод `DirectProblem`, в который передать в качестве параметров, начальную точку, азимут = направление и расстояние:

```C#
var point1 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);
var azimuth = 21;
var distance = 2000;
var directAnswer = directEllipsoid.DirectProblem(point1, azimuth, distance);
```

Ответ `directAnswer` содержит вторую точку ортодромии `Сoordinate` и обратный азимут `ReverseAzimuth`.

### <a name="Header2"></a> Решение обратной геодезической задачи

Для решения обратной геодезической задачи, неоходимо создать объект класса `InverseProblemService`.

```C#
var inverseEllipsoid = new InverseProblemService(new Ellipsoid());
var inverseSpheroid = new InverseProblemService(new Spheroid());
```

В качестве параметра в конструктор следует передать объект реализующий интерфейс `IEllipsoid`, в котором задаются полярный и экваториальный радиус, а так же коэффициент полярного сжатия.

Для решения обратной задачи вызвать метод `OrthodromicDistance`, в который передать в качестве параметров, две точки:

```C#
var point1 = new Point(15, 25, 53, CardinalLongitude.W, 28, 7, 38, CardinalLatitude.N);
var point2 = new Point(59, 36, 30, CardinalLongitude.W, 13, 5, 46, CardinalLatitude.N);
var inverseAnswer = inverseEllipsoid.OrthodromicDistance(point1, point2);
```

Ответ `inverseAnswer` содержит прямой и обратный азимуты `ForwardAzimuth`, `ReverseAzimuth`, а также расстояние между точками `Distance`.

### <a name="Header3"></a> Вычисление точки пересечения ортодромий

Для вычисления точки пересечения ортодромий, неоходимо создать объект класса `IntersectService`.

```C#
var intersectEllipsoid = new IntersectService(new Ellipsoid());
var intersectSpheroid = new IntersectService(new Spheroid());
```

В качестве параметра в конструктор следует передать объект реализующий интерфейс `IEllipsoid`, в котором задаются полярный и экваториальный радиус, а так же коэффициент полярного сжатия.

Для рассчёта вызвать метод `IntersectOrthodromic`, в который передать в качестве параметров, по две точки для каждой из двух ортодромий:

```C#
var point1 = new Point(22, 36, 30, CardinalLongitude.E, 13, 5, 46, CardinalLatitude.N);
var point2 = new Point(27, 25, 53, CardinalLongitude.E, 15, 7, 38, CardinalLatitude.N);
var point3 = new Point(20, 36, 30, CardinalLongitude.E, 17, 5, 46, CardinalLatitude.N);
var point4 = new Point(26, 25, 53, CardinalLongitude.E, 13, 7, 38, CardinalLatitude.N);
var intersectCoord = intersectEllipsoid.IntersectOrthodromic(point1, point2, point3, point4);
```

Ответом будет точка - объект класса `Point`, в котором определены долгота и широта, в десятичных градуса (`Longitude`,`Latitude`) или в радианах (`LonR`,`LatR`).

### <a name="Header4"></a> Вычисление широты по долготе или долготы по широте 

Для рассчётов, неоходимо создать объект класса `IntermediatePointService`.

```C#
var intermediateEllipsoid = new IntermediatePointService(new Ellipsoid());
var ntermediateSpheroid = new IntermediatePointService(new Spheroid());
```

В качестве параметра в конструктор следует передать объект реализующий интерфейс `IEllipsoid`, в котором задаются полярный и экваториальный радиус, а так же коэффициент полярного сжатия.

Для вычисления широты вызвать метод `GetLatitude`, в который передать значение долготы, для которого мы вычисляем широту, и две координаты характеризующие ортодромию.

```C#
var coord1 = new Point(10, 10);
var coord2 = new Point(30, 50);
var lat = intermediateEllipsoid.GetLatitude(20, coord1, coord2);
```

Для вычисления долготы вызвать метод `GetLongitude`, в который передать значение широты, для которого мы вычисляем долготу, и две координаты характеризующие ортодромию.

```C#
var coord1 = new Point(10, 10);
var coord2 = new Point(30, 50);
var lat = intermediateEllipsoid.GetLongitude(20, coord1, coord2);
```

В обоих случая ответом будет значение типа `double`.
