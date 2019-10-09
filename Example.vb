Public Class Example
    <CAttribute> Public Property Message As String
    <CAttribute> Public Property Bool As Boolean
    <CAttribute> Public Property IntNumber As Integer
    <CAttribute> Public Property FloatNumber As Double
    <CAttribute> Public Property CollectionA As String()
    <CAttribute> Public Property CollectionB As Integer()
    <CAttribute> Public Property CollectionC As Double()
    <CAttribute> Public Property CollectionD As Boolean()
    <CAttribute> Public Property CollectionE As Byte()
    Public Overrides Function ToString() As String
        Return String.Format("{0} {1} {2} {3} [{4}/{5}/{6}/{7}/{8}]", Me.Message,
                                                                      Me.Bool,
                                                                      Me.IntNumber,
                                                                      Me.FloatNumber,
                                                                      Me.CollectionA.Count,
                                                                      Me.CollectionB.Count,
                                                                      Me.CollectionC.Count,
                                                                      Me.CollectionD.Count,
                                                                      Me.CollectionE.Count)
    End Function
End Class

