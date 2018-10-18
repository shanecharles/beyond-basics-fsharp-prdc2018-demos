

// string -> string -> string -> string -> string
let shortStory setting subject location action = 
    sprintf "%s, %s was %s %s." setting subject location action


shortStory "It was a dark and stormy night" 
           "the cat" 
           "in the library" 
           "reading War and Peace"










let shortStory' =
    fun setting ->
     fun subject ->
      fun location ->
       fun action -> 
        sprintf "%s, %s was %s %s." setting subject location action


 
shortStory' "It was a dark and stormy night" "the cat" "in the library" "reading War and Peace" 













// string -> string -> string
let darkAndStormyCatStory = 
        shortStory "It was a dark and stormy night" 
                   "the cat"






[ "on the roof", "with a lightning rod to bring its creation to life"
  "in the cellar", "making funny faces"
  "on the chair", "ignoring the dog"
  "on the table", "watching funny cat videos" ]
  |> List.map (fun (location, event) -> darkAndStormyCatStory location event)


 
let shortStory'' =
    fun setting ->
     printfn "Setting: %s" setting
     fun subject ->
      printfn "Subject: %s" subject
      fun location ->
       printfn "Location: %s" subject
       fun action -> 
        sprintf "%s, %s was %s %s." setting subject location action


shortStory'' "At PrDC18 Regina"