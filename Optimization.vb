'AU import solver foundations so vb can connect
Imports Microsoft.SolverFoundation.Common
Imports Microsoft.SolverFoundation.Services
Imports Microsoft.SolverFoundation.Solvers
'****************************************************************************************************************
Public Class Optimization
    ' AU: This class optimizes the best solution based on the constraints, decision variables and objectives functions '
    ' AU: These public vields are availible throug out the class
    Public HW5AUModel As New SimplexSolver
    Dim dvKey As String
    Dim dvIndex As Integer
    Dim coefficient As Single
    Dim constraintKey As String
    Dim constraintIndex As Integer
    Dim objKey As String = "Objective Function"
    Dim objIndex As Integer
    Public optimalObj As Single
    Public dvValues(Employee.EmployeeList.Count - 1, Shift.ShiftList.Count - 1) As Single
    ' AU: This class builds all contraints, decison variables and the objective function ' 
    Public Sub AUBuildModel()
        'AU This set of loops builds our decison variables 
        For Each myEmployee As Employee In Employee.EmployeeList
            For Each myShift As Shift In Shift.ShiftList
                dvKey = myEmployee.EmployeeName & "_" & myShift.ShiftName
                HW5AUModel.AddVariable(dvKey, dvIndex)

                HW5AUModel.SetBounds(dvIndex, 0, 1)
                HW5AUModel.SetIntegrality(dvIndex, True)
            Next
        Next

        ' AU These lopps build the constraints ' 

        ' AU: Right most constraint '
        For Each myPeriod As Period In Period.PeriodList
            constraintKey = "Period Constraint" & "_" & myPeriod.PeriodName
            HW5AUModel.AddRow(constraintKey, constraintIndex)
            HW5AUModel.SetBounds(constraintIndex, Process.ProcessList(0).ProcessTime * Demand.DemandList(0).Period(Period.PeriodList.IndexOf(myPeriod)), Rational.PositiveInfinity)

            'AU contraint for period overlaps ' 
            For Each myEmployee As Employee In Employee.EmployeeList
                For Each myShift As Shift In Shift.ShiftList

                    dvIndex = HW5AUModel.GetIndexFromKey(myEmployee.EmployeeName & "_" & myShift.ShiftName)
                    coefficient = myShift.PeriodOverlap(Period.PeriodList.IndexOf(myPeriod))
                    HW5AUModel.SetCoefficient(constraintIndex, dvIndex, coefficient)
                Next
            Next
        Next
        'AU: Left most constraint ' 
        For Each myEmployee As Employee In Employee.EmployeeList
            For Each myShift As Shift In Shift.ShiftList
                constraintKey = "Employee Constraint" & "_" & myEmployee.EmployeeName & "_" & "Shift Constraint" & myShift.ShiftName
                HW5AUModel.AddRow(constraintKey, constraintIndex)
                HW5AUModel.SetBounds(constraintIndex, 0, myEmployee.ShiftAvailability(Shift.ShiftList.IndexOf(myShift)))


            Next
        Next





        '----------------------------------------------------------------------------------------------------------
        'AU: Define the objective function

        HW5AUModel.AddRow(objKey, objIndex)
        For Each myEmployee As Employee In Employee.EmployeeList
            For Each myShift As Shift In Shift.ShiftList

                dvIndex = HW5AUModel.GetIndexFromKey(myEmployee.EmployeeName & "_" & myShift.ShiftName)
                coefficient = myEmployee.WageRate * myShift.ShiftLength
                HW5AUModel.SetCoefficient(objIndex, dvIndex, coefficient)



            Next
        Next
        HW5AUModel.AddGoal(objIndex, 0, True)
    End Sub
    'AU the code is run, it is tested for feasibility and boundaries and if a soultion is found the answer is shown
    Public Sub AURunModel()
        Dim mySolverParms As New SimplexSolverParams
        mySolverParms.MixedIntegerGapTolerance = 1              'AU: For IP only - 1 percent gap tolerance between upper and lower bounds of objective function
        mySolverParms.VariableFeasibilityTolerance = 0.00001    'AU: For IP only - required closeness to a whole number of each variable
        mySolverParms.MaxPivotCount = 1000                      'AU: Number of iterations.  Increase as necessary
        HW5AUModel.Solve(mySolverParms)

        'AU: We check to see if we got an answer and boundaries and if not test its feasibility right below,showe answer if solution is found
        If HW5AUModel.Result = LinearResult.UnboundedPrimal Then
            MessageBox.Show("Solution is unbounded")
            Exit Sub
        ElseIf _
        HW5AUModel.Result = LinearResult.InfeasiblePrimal Then
            MessageBox.Show("Decision model is infeasible")
            Exit Sub
        Else
            ShowAnswer()
        End If
    End Sub

    Public Sub ShowAnswer()
        optimalObj = CSng(HW5AUModel.GetValue(objIndex).ToDouble)

        'AU: We transfer the values of the decision variables to an array 
        Dim rowIndex As Integer = 0
        Dim columnIndex As Integer = 0


        For Each myEmployee As Employee In Employee.EmployeeList
            rowIndex = Employee.EmployeeList.IndexOf(myEmployee)
            For Each myShift As Shift In Shift.ShiftList
                columnIndex = Shift.ShiftList.IndexOf(myShift)
                dvKey = myEmployee.EmployeeName & "_" & myShift.ShiftName
                dvIndex = HW5AUModel.GetIndexFromKey(dvKey)
                dvValues(rowIndex, columnIndex) = CSng(HW5AUModel.GetValue(dvIndex).ToDouble)
            Next
        Next

        Solution.AUTableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        '
        'AU: We enter the column headings into the table
        For column As Integer = 1 To Solution.AUTableLayoutPanel1.ColumnCount - 1
            Dim myLabel As New Label
            myLabel.Text = "Shift " & CStr(column)
            Solution.AUTableLayoutPanel1.Controls.Add(myLabel)
            myLabel.Visible = True
            myLabel.TextAlign = ContentAlignment.MiddleCenter
            Solution.AUTableLayoutPanel1.SetRow(myLabel, 0)
            Solution.AUTableLayoutPanel1.SetColumn(myLabel, column)
            myLabel.Anchor = AnchorStyles.Bottom
            myLabel.Anchor = AnchorStyles.Top
            myLabel.Anchor = AnchorStyles.Left
            myLabel.Anchor = AnchorStyles.Right

        Next
        '
        'AU: We enter the row headings into the table
        rowIndex = 0
        For Each myEmployee As Employee In Employee.EmployeeList
            Dim myLabel As New Label
            myLabel.Text = myEmployee.EmployeeName
            myLabel.Visible = True
            myLabel.TextAlign = ContentAlignment.MiddleCenter
            Solution.AUTableLayoutPanel1.SetRow(myLabel, rowIndex + 1)
            Solution.AUTableLayoutPanel1.SetColumn(myLabel, 0)
            Solution.AUTableLayoutPanel1.Dock = DockStyle.Fill
            Solution.AUTableLayoutPanel1.Controls.Add(myLabel)
            myLabel.Anchor = AnchorStyles.Bottom
            myLabel.Anchor = AnchorStyles.Top
            myLabel.Anchor = AnchorStyles.Left
            myLabel.Anchor = AnchorStyles.Right
            rowIndex += 1
        Next
        'Au Implement the table layout pannel so it displays the answer
        For row As Integer = 1 To Solution.AUTableLayoutPanel1.RowCount - 1
            For column As Integer = 1 To Solution.AUTableLayoutPanel1.ColumnCount - 1
                Dim myLabel As New Label
                myLabel.Text = CStr(dvValues(row - 1, column - 1))
                myLabel.Visible = True
                myLabel.TextAlign = ContentAlignment.MiddleCenter
                Solution.AUTableLayoutPanel1.SetRow(myLabel, row)
                Solution.AUTableLayoutPanel1.SetColumn(myLabel, column)
                Solution.AUTableLayoutPanel1.Dock = DockStyle.Fill
                Solution.AUTableLayoutPanel1.Controls.Add(myLabel)
                myLabel.Anchor = AnchorStyles.Bottom
                myLabel.Anchor = AnchorStyles.Top
                myLabel.Anchor = AnchorStyles.Left
                myLabel.Anchor = AnchorStyles.Right
            Next
        Next
        Solution.Show() ' Au finally call on the form solution to show our solution

    End Sub

End Class





