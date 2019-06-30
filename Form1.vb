Public Class frmRDBMain
    Private Sub btnRDBOptimize_Click(sender As Object, e As EventArgs) Handles btnRDBOptimize.Click
        Dim myObjectCreator As New ObjectCreator
        myObjectCreator.CreateObjectsAndLists()

        Dim myOptimization As New Optimization
        myOptimization.AUBuildModel()
        myOptimization.AURunModel()

    End Sub
End Class
