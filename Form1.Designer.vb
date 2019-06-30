<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRDBMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnRDBOptimize = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnRDBOptimize
        '
        Me.btnRDBOptimize.Location = New System.Drawing.Point(194, 144)
        Me.btnRDBOptimize.Name = "btnRDBOptimize"
        Me.btnRDBOptimize.Size = New System.Drawing.Size(268, 54)
        Me.btnRDBOptimize.TabIndex = 0
        Me.btnRDBOptimize.Text = "Optimize My Schedule"
        Me.btnRDBOptimize.UseVisualStyleBackColor = True
        '
        'frmRDBMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnRDBOptimize)
        Me.Name = "frmRDBMain"
        Me.Text = "Scheduler"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnRDBOptimize As Button
End Class
