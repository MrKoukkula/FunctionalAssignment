namespace MineSweeper
open Microsoft.FSharp.Collections
open System
open System.Collections.Generic

module GameLogic =
    let random = new Random()
    let mine = "*"
    let empty = "_"
    let flag = "f"
    let mutable boardSize = 10
    let mutable amountOfMines = 10
    let mutable map = Array2D.init boardSize boardSize (fun _ _ -> empty)
    let mutable gameMap = Array2D.init boardSize boardSize (fun _ _ -> empty)

    let private placeMines() =
        let mutable minesPlaced = 0
        while minesPlaced < amountOfMines do
            let row = random.Next(boardSize)
            let col = random.Next(boardSize)
            if map.[row, col] = empty then
                map.[row, col] <- mine
                minesPlaced <- minesPlaced + 1

    let private DidPlayerWin() : bool =
        let mutable minesLeft = 0
        for i in 0 .. map.GetLength(0) - 1 do
            for j in 0 .. map.GetLength(1) - 1 do
                if map.[i, j] = mine then
                    minesLeft <- minesLeft + 1
            printfn ""
        match minesLeft with
            | 0 -> true
            | _ -> false

    let private calculateMinesNearby(x:int, y:int) : string = 
        let mutable minesNearby = 0
        let cleared = "c"

        let myList = [(x-1, y-1); (x, y-1); (x+1, y-1); (x-1, y); (x+1, y); (x-1, y+1); (x, y+1); (x+1, y+1);]
        for (x, y) in myList do
            let mutable newX = x
            let mutable newY = y
            if x < 0 then
                newX <- 0
            if y < 0 then
                newY <- 0
            if map.[newX,newY] = mine then
                minesNearby <- minesNearby + 1

        if minesNearby = 0 then
            cleared
        else 
            minesNearby.ToString()

    let private setGameConditions() =
        printfn "How big map do you want?"
        let size = Console.ReadLine()
        boardSize <- int size
        map <- Array2D.init boardSize boardSize (fun _ _ -> empty)
        gameMap <- Array2D.init boardSize boardSize (fun _ _ -> empty)
        printfn "How many mines do you want?"
        let mines = Console.ReadLine()
        amountOfMines <- int mines
        placeMines()

    let public startGame() =
        
        let mutable gameOver = false
        setGameConditions()

        while not gameOver do
            printfn "%A" gameMap
            printfn "Enter command (r x y) to reveal or (f x y) to flag"
            let command = Console.ReadLine().Split()
            let action = command.[0]
            let col = int command.[2]
            let row = int command.[1]
            match action with
            | "r" -> 
                if map.[col, row] = mine then
                    map.[col, row] <- mine
                    printfn "You stepped on a mine! Game over."
                    printfn "%A" map
                    gameOver <- true
                else
                    printfn "Safe!"
                    gameMap.[col, row] <- calculateMinesNearby(col, row)
            | "f" -> 
                map.[col, row] <- flag
                gameMap.[col, row] <- flag
                printfn "Flagged!"
                gameOver <- DidPlayerWin()

            | _ -> printfn "Invalid command"
        printfn "Thanks for playing!"