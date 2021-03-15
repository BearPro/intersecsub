namespace IntersecSub

module Core =
    let intersec a b = Set.intersect a b

    let sub (a: 'a Set) (b: 'a Set) = a - b

    let intersecSub a b c =
        let ab' = intersec a b
        let r = sub ab' c
        r