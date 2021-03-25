module IntersecSub.Tests

open Xunit

[<Fact>]
let ``Intersection correct`` () =
    let a = set [ 1; 2; 3; 4; 5; ]
    let b = set    [ 2; 3; 4; 5; 6; ]
    let expected = set [ 2; 3; 4; 5; ]
    let actual = Core.intersec a b
    Assert.Equal<int>(expected, actual)


[<Fact>]
let ``Substraction correct`` () =
    let a = set [ 1; 2; 3; 4; 5; ]
    let b = set    [ 2; 3; 4; 5; 6; ]
    let expected = set [ 1 ]
    let actual = Core.sub a b
    Assert.Equal<int>(expected, actual)

[<Fact>]
let ``Substraction of intersection correct`` () =
    let a = set [ 1; 2; 3; 4; 5; ]
    let b = set    [ 2; 3; 4; 5; 6; ]
    let c = set          [ 5; 6; ]
    let expected = set [ 2; 3; 4 ]
    let actual = Core.intersecSub a b c
    Assert.Equal<int>(expected, actual)

