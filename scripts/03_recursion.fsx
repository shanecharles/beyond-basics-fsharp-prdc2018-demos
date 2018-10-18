// #time

// Plain old recursive function
let fib n : int64 =
    let rec loop x =
        if x <= 1L then x
        else
            loop (x-1L) + loop (x-2L)
    loop (int64 n)




fib 41







// Tail Call Optimization
let fibTCO n : int64 = 
    let rec loop (c, x, y) =
        if c > n-1 then y
        else
            loop (c+1, y, y+x)
    loop (1, 0L, 1L)



fibTCO 41
fibTCO 41 = fib 41




// Memoization
let fibMem n : int64 =
    let cache = System.Collections.Generic.Dictionary<int64, int64>()

    let rec loop x =
        if x <= 1L then x
        elif cache.ContainsKey x then cache.[x]
        else 
            let r = (loop (x-1L)) + (loop (x-2L))
            cache.[x] <- r
            r
    loop (int64 n)







fibMem 41
fibMem 41 = fibTCO 41








// Non-recursive function
let fibSeq = (0L,1L) |> Seq.unfold (fun (x, y) -> Some (y, (y, y+x))) 

fibSeq |> Seq.take 10 |> Seq.toList

let fibUnfold n : int64 =
    fibSeq
    |> Seq.skip (n-1)
    |> Seq.head


fibUnfold 41
fibUnfold 41 = fibMem 41