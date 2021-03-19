namespace IntersecSub.Gui

module Shell =
    open Elmish
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Input
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.Elmish

    type State =
      { editListA: EditList.State
        editListB: EditList.State
        editListC: EditList.State
        resultSet: int Set }

    type Msg =
        | ListAMsg of EditList.Msg
        | ListBMsg of EditList.Msg
        | ListCMsg of EditList.Msg

    let init =
        let listAState, listACmd = EditList.init "Set A"
        let listBState, listBCmd = EditList.init "Set B"
        let listCState, listCCmd = EditList.init "Set C"
        { editListA = listAState
          editListB = listBState
          editListC = listCState 
          resultSet = Set.empty },
        Cmd.batch [ listACmd; listBCmd; listCCmd ]

    let update (msg: Msg) (state: State): State * Cmd<_> =
        let state', cmd =
            match msg with
            | ListAMsg listMsg ->
                let listState, cmd = EditList.update listMsg state.editListA
                { state with editListA = listState }, Cmd.map ListAMsg cmd
            | ListBMsg listMsg ->
                let listState, cmd = EditList.update listMsg state.editListB
                { state with editListB = listState }, Cmd.map ListBMsg cmd
            | ListCMsg listMsg ->
                let listState, cmd = EditList.update listMsg state.editListC
                { state with editListC = listState }, Cmd.map ListCMsg cmd
        let resultSet = 
            IntersecSub.Core.intersecSub 
                state'.editListA.Items 
                state'.editListB.Items 
                state'.editListC.Items
        { state' with resultSet = resultSet }, cmd

    let resultView resultSet =
        let items = String.concat " " (resultSet |> Seq.map string)
        TextBlock.create [ TextBlock.text $"(A ∩ B) - C = {{{items}}}" ]

    let view (state: State) (dispatch) =
        DockPanel.create
            [ DockPanel.children
                [ Grid.create [
                    DockPanel.dock Dock.Bottom
                    Grid.children [
                        resultView (seq state.resultSet)
                    ]
                  ]
                  Grid.create [
                    Grid.columnDefinitions "* * *"
                    Grid.children [
                        Grid.create [ 
                            Grid.column 0
                            Grid.children [ EditList.view (state.editListA) (ListAMsg >> dispatch) ] ]
                        Grid.create [ 
                            Grid.column 1
                            Grid.children [ EditList.view (state.editListB) (ListBMsg >> dispatch) ] ]
                        Grid.create [ 
                            Grid.column 2
                            Grid.children [ EditList.view (state.editListC) (ListCMsg >> dispatch) ] ]
                    ]
                ]
                ]
            ]
    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Разность с пересечением."
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 800.0
            base.MinHeight <- 600.0

            //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
            |> Program.run
