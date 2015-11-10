namespace Test

open NUnit.Framework
open CoreLib

[<TestFixture>]
type CoreTest() =
    let test ax ay bx by cx cy px py : PointLocation =
        let a = new Point(ax, ay)
        let b = new Point(bx, by)
        let c = new Point(cx, cy)
        let p = new Point(px, py)
        TriangleCheck.CalcPointLocation(a, b, c, p)

    [<TestCase (0, 0, 5, 0, 0, 5, 5, 5)>]
    [<TestCase (0, 0, 5, 0, 0, 5, 6, 0)>]
    [<TestCase (0, 0, 5, 0, 0, 5, -1, 0)>]
    member this.testOutside ax ay bx by cx cy px py =
        Assert.AreEqual(PointLocation.Outside, (test ax ay bx by cx cy px py))

    [<TestCase (0, 0, 5, 0, 0, 5, 1, 1)>]
    [<TestCase (0, -1, -2, 2, 2, 2, 0, -0.75)>]
    member this.testInside ax ay bx by cx cy px py =
        Assert.AreEqual(PointLocation.Inside, (test ax ay bx by cx cy px py))
        
    [<TestCase (0, 0, 5, 0, 0, 5, 0, 1)>]
    [<TestCase (0, 0, 5, 0, 0, 5, 2.5, 2.5)>]
    member this.testOnBorder ax ay bx by cx cy px py =
        Assert.AreEqual(PointLocation.OnBorder, (test ax ay bx by cx cy px py))
        
    [<TestCase (0, 0, 5, 0, 0, 5, 0, 5)>]
    member this.testOnVertex ax ay bx by cx cy px py =
        Assert.AreEqual(PointLocation.OnVertex, (test ax ay bx by cx cy px py))