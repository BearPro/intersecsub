namespace IntersecSub.Gui

module EditList =
    open Elmish
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open System.Diagnostics
    open System.Runtime.InteropServices
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.DSL

    type State =
        { Name: string
          ViewItems: string list
          Items: Set<int>
          Errors: string list }

    type Msg = 
        | AddItem
        | UpdateItem of index: int * newValue: string
        | DeleteItem of index: int

    let init name = 
        { Name = name; ViewItems = List.empty; Items = Set.empty; Errors = [] }, Cmd.none

    let parseValue stringValue =
        try Ok (int stringValue) with _ -> Error $"Not integer value '{stringValue}'."

    let replaceList value index list =
        list |> List.mapi (fun i v -> if i = index then value else v)

    let removeList index list =
        list
        |> List.mapi (fun i v -> i, v)
        |> List.filter (fun (i, _) -> i <> index)
        |> List.map (fun (_, v) -> v)

    let duplicates list =
        list
        |> List.groupBy id
        |> List.filter (fun (x, xs) -> 1 < List.length xs )
        |> List.map (fun (x, _) -> $"Value {x} already presented.")

    let updateResult viewItems state =
        let itemsResults = viewItems |> List.map parseValue
        let parsedItems = 
            itemsResults 
            |> List.filter (function Ok _ -> true | Error _ -> false)
            |> List.map (function Ok x -> x | _ -> failwith "CRIT")

        let items' = parsedItems |> Set.ofList
        let errs' =
            itemsResults 
            |> List.filter (function Ok _ -> false | Error _ -> true)
            |> List.map (function Error m -> m | _ -> failwith "CRIT")

        let duplicates = duplicates parsedItems
                    
        { state with Items = items'
                     ViewItems = viewItems
                     Errors = errs' @ duplicates }

    let update (msg: Msg) (state: State) =
        match msg with
        | AddItem -> { state with ViewItems = "" :: state.ViewItems }, Cmd.none
        | UpdateItem(viewIndex, newStringValue) -> 
            let viewItems' = state.ViewItems |> replaceList newStringValue viewIndex
            let state' = updateResult viewItems' state
            state', Cmd.none
        | DeleteItem index ->
            let viewItems' = state.ViewItems |> removeList index
            let state' = updateResult viewItems' state
            state', Cmd.none
        | _ -> state, Cmd.none

    let view (state: State) (dispatch: Msg -> unit) =
        let dispatchItemUpdate index text =
            dispatch (UpdateItem (index, text))

        let listItemView index item =
            Grid.create [
                Grid.columnDefinitions "* 20"
                Grid.children [
                    TextBox.create [
                        Grid.column 0
                        TextBox.text item
                        TextBox.onTextChanged (dispatchItemUpdate index)
                    ]
                    Button.create [
                        Grid.column 1
                        Button.content "x"
                        Button.onClick (fun _ -> dispatch (DeleteItem index))
                    ]
                ]
            ] :> IView

        DockPanel.create [
            DockPanel.children [
                yield TextBlock.create [
                    TextBlock.text state.Name
                    DockPanel.dock Dock.Top ]
                yield StackPanel.create [ 
                        DockPanel.dock Dock.Bottom
                        StackPanel.children (state.Errors |> List.map (fun x -> TextBlock.create [ TextBlock.text x ] :> IView)) ]
                yield StackPanel.create [
                    StackPanel.children (
                        let listItems = 
                            state.ViewItems 
                            |> List.mapi (listItemView)
                        let addButton = 
                            Button.create [ 
                                Button.content "+"
                                Button.onClick (fun _ -> dispatch AddItem ) ] :> IView
                        addButton :: listItems
                    )
                ]
            ]
        ]