Public Class Example
    <CAttribute> Public Property Message As String
    <CAttribute> Public Property Bool As Boolean
    <CAttribute> Public Property IntNumber As Integer
    <CAttribute> Public Property FloatNumber As Double
    Public Overrides Function ToString() As String
        Return String.Format("{0} {1} {2} {3}", Me.Message, Me.Bool, Me.IntNumber, Me.FloatNumber)
    End Function
End Class

