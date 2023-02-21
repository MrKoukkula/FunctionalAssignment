open MineSweeper.GameLogic
open System

[<EntryPoint>]
let main argv =
    printfn "%A" "Welcome to play my Minesweeper"
    
    startGame()
    
    0
