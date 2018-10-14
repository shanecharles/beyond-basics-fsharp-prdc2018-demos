
// string -> string -> string -> string -> string
let shortStory setting subject location action = 
    sprintf "%s, %s was %s %s." setting subject location action


shortStory "It was a dark and stormy night" "the cat" "in the library" "reading War and Peace" 


// string -> string -> string -> string
let darkAndStormyStory = shortStory "It was a dark and storym night"


darkAndStormyStory "the cat" "on the roof" "with a lightning rod to bring its creation to life"


// string -> string -> string
let darkAndStormyCatStory = darkAndStormyStory "the cat"



[ "in the cellar", "making funny faces"
  "on the chair", "ignoring the dog"
  "on the computer", "watching funny cat videos" ]
  |> List.map (fun (location, event) -> darkAndStormyCatStory location event)