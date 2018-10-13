// #time

// Plain old recursive function
let fib n : int64 =
    let rec loop x =
        if x <= 1L then x
        else
            loop (x-1L) + loop (x-2L)
    loop (int64 n)







// Tail Call Optimization
let fibTCO n : int64 = 
    let rec loop (c, x, y) =
        if c > n-1 then y
        else
            loop (c+1, y, y+x)
    loop (1, 0L, 1L)






// Memoization
let fibMem n : int64 =
    let cache = System.Collections.Generic.Dictionary<int64, int64>()
    let addCache f y = 
        match cache.TryGetValue y with
        | true, v  -> v
        | _        -> cache.[y] <- (f y)
                      cache.[y]
    let rec loop x =
        if x <= 1L then x
        else 
            (addCache loop (x-1L)) + (addCache loop (x-2L))
    loop (int64 n)





// Non-recursive function
let fibSeq = (0L,1L) |> Seq.unfold (fun (x, y) -> Some (y, (y, y+x))) 

let fibSeq' n : int64 =
    fibSeq
    |> Seq.skip (n-1)
    |> Seq.head