module IntersecSub.FunctionalTests

open Xunit

[<Fact>]
let ``Substraction of calculated intersection correct with normal input`` () =
    let a = set [ 1; 2; 3; 4; 5; ]
    let b = set    [ 2; 3; 4; 5; 6; ]
    let c = set          [ 5; 6; ]
    let expected = set [ 2; 3; 4 ]
    let actual = Core.intersecSub a b c
    Assert.Equal<int>(expected, actual)


[<Fact>]
let ``Substraction of calculated intersection correct with empty sets`` () =
    let a = set [ ]
    let b = set [ ]
    let c = set [ ]
    let expected = set [ ]
    let actual = Core.intersecSub a b c
    Assert.Equal<int>(expected, actual)

[<Fact>]
let ``Substraction of calculated intersection correct with large sets`` () =
    let a = set [ 1..999 ]
    let b = set [ 2..1000 ]
    let c = set [ 500..1000 ]
    let expected = set [ 2..499 ]
    let actual = Core.intersecSub a b c
    Assert.Equal<int>(expected, actual)