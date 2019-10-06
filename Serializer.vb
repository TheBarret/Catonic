Imports System.IO
Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports System.Security.Cryptography
Imports Catonic.Ciphers

Public Class Serializer
    Public Shared Property Encoder As Encoding = Encoding.UTF8
    Public Shared Property Culture As CultureInfo = New CultureInfo("en-US")
    ''' <summary>
    ''' Export type into binary data format
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <param name="instance"></param>
    Public Shared Sub Export(filename As String, instance As Object, Optional scrambler As Scrambler = Scrambler.None)
        Dim ref() As Byte, value() As Byte, hash() As Byte
        Using bw As New BinaryWriter(File.Open(filename, FileMode.Create))
            bw.Write(Serializer.Header)
            For Each cv In Serializer.CollectProperties(instance)
                bw.Write(CByte(cv.Type))
                ref = Serializer.Scramble(cv.Reference, scrambler)
                value = Serializer.Scramble(cv.Value, scrambler)
                hash = Serializer.Hash(value)
                bw.Write(ref.Length)
                bw.Write(value.Length)
                bw.Write(hash)
                bw.Write(ref)
                bw.Write(value)
            Next
        End Using
    End Sub
    ''' <summary>
    ''' Import binary data format into T
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="filename"></param>
    ''' <param name="target"></param>
    ''' <returns></returns>
    Public Shared Function Import(Of T As New)(filename As String, target As T, Optional scrambler As Scrambler = Scrambler.None) As T
        If (File.Exists(filename)) Then
            Dim values As New List(Of CValue)
            Using br As New BinaryReader(File.Open(filename, FileMode.Open))

                If (br.ReadBytes(2).SequenceEqual(Serializer.Header)) Then

                    Dim dt, h() As Byte, r, v As String, l1, l2 As Int32
                    Do While br.BaseStream.Position + &H29 < br.BaseStream.Length
                        dt = br.ReadByte
                        l1 = br.ReadInt32
                        l2 = br.ReadInt32
                        h = br.ReadBytes(32)

                        If ((br.BaseStream.Position + l1 + l2) > br.BaseStream.Length) Then
                            Throw New Exception("length mismatch")
                        End If

                        r = Serializer.Descramble(br.ReadBytes(l1), scrambler)
                        v = Serializer.Descramble(br.ReadBytes(l2), scrambler, h)

                        values.Add(New CValue(r, v, CType(dt, CTypes)))
                    Loop
                    If (values.Any) Then
                        For Each element As CValue In values
                            Dim pdata As PropertyInfo = target.GetType.GetProperty(element.Reference)
                            If (pdata Is Nothing) Then
                                Throw New Exception("property reference mismatch")
                            End If
                            pdata.SetValue(target, Serializer.Convert(element.Type, element.Value))
                        Next
                    End If
                    Return target
                End If
                Throw New Exception("header mismatch")
            End Using
        End If
    End Function
    ''' <summary>
    ''' Create collection info from properties
    ''' </summary>
    ''' <param name="instance"></param>
    ''' <returns></returns>
    Private Shared Function CollectProperties(instance As Object) As List(Of CValue)
        Dim values As New List(Of CValue)
        For Each p In Serializer.GetProperties(instance.GetType)
            values.Add(New CValue(p.Name, p.GetValue(instance, Nothing), Serializer.GetCType(p.PropertyType)))
        Next
        Return values
    End Function
    ''' <summary>
    ''' Collect properties of type with custom attribute
    ''' </summary>
    ''' <param name="type"></param>
    ''' <returns></returns>
    Private Shared Function GetProperties(type As Type) As PropertyInfo()
        Return type.GetProperties.Where(Function(x) x.GetCustomAttributes(GetType(CAttribute), False).Any).ToArray
    End Function
    ''' <summary>
    ''' Scrabmles plain text into unreadable format
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="scrambler"></param>
    ''' <returns></returns>
    Private Shared Function Scramble(data As String, scrambler As Scrambler) As Byte()
        Select Case scrambler
            Case Scrambler.Rot13
                data = New Rot13().Input(data)
            Case Scrambler.Rot47
                data = New Rot47().Input(data)
            Case Scrambler.Bifid
                data = New Bifid().Input(data)
        End Select
        Return Serializer.Encoder.GetBytes(data)
    End Function
    ''' <summary>
    ''' Descrables scrambled text into readable format
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="scrambler"></param>
    ''' <param name="hash"></param>
    ''' <returns></returns>
    Private Shared Function Descramble(data() As Byte, scrambler As Scrambler, Optional hash() As Byte = Nothing) As String
        If (hash IsNot Nothing AndAlso Not hash.SequenceEqual(Serializer.Hash(data))) Then
            Throw New Exception("data hash mismatch")
        End If
        Dim output As String = Serializer.Encoder.GetString(data)
        Select Case scrambler
            Case Scrambler.Rot13
                output = New Rot13().Output(output)
            Case Scrambler.Rot47
                output = New Rot47().Output(output)
            Case Scrambler.Bifid
                output = New Bifid().Output(output)
        End Select
        Return output
    End Function
    ''' <summary>
    ''' Returns hash (SHA256) from data array
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Private Shared Function Hash(data() As Byte) As Byte()
        Using provider As New SHA256CryptoServiceProvider
            Return provider.ComputeHash(data)
        End Using
    End Function
    ''' <summary>
    ''' Convert value to desired data type
    ''' </summary>
    ''' <param name="type"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Private Shared Function Convert(type As CTypes, value As String) As Object
        Select Case type
            Case CTypes.String
                Return value
            Case CTypes.Boolean
                Return Boolean.Parse(value)
            Case CTypes.Int16
                Return Int16.Parse(value, Serializer.Culture)
            Case CTypes.UInt16
                Return UInt16.Parse(value, Serializer.Culture)
            Case CTypes.Int32
                Return Int32.Parse(value, Serializer.Culture)
            Case CTypes.UInt32
                Return UInt32.Parse(value, Serializer.Culture)
            Case CTypes.Int64
                Return Int64.Parse(value, Serializer.Culture)
            Case CTypes.UInt64
                Return UInt64.Parse(value, Serializer.Culture)
            Case CTypes.Double
                Return Double.Parse(value, Serializer.Culture)
            Case CTypes.Single
                Return Single.Parse(value, Serializer.Culture)
            Case CTypes.Decimal
                Return Decimal.Parse(value, Serializer.Culture)
            Case CTypes.Byte
                Return Byte.Parse(value, Serializer.Culture)
            Case CTypes.SByte
                Return SByte.Parse(value, Serializer.Culture)
        End Select
        Throw New Exception(String.Format("unsupported type '{0}'", type.ToString))
    End Function
    ''' <summary>
    ''' Get CType from value type
    ''' </summary>
    ''' <param name="datatype"></param>
    ''' <returns></returns>
    Private Shared Function GetCType(datatype As Type) As CTypes
        Select Case datatype
            Case GetType(String)
                Return CTypes.String
            Case GetType(Boolean)
                Return CTypes.Boolean
            Case GetType(Int16)
                Return CTypes.Int16
            Case GetType(UInt16)
                Return CTypes.UInt16
            Case GetType(Int32)
                Return CTypes.Int32
            Case GetType(UInt32)
                Return CTypes.UInt32
            Case GetType(Int64)
                Return CTypes.Int64
            Case GetType(UInt64)
                Return CTypes.UInt64
            Case GetType(Double)
                Return CTypes.Double
            Case GetType(Single)
                Return CTypes.Single
            Case GetType(Decimal)
                Return CTypes.Decimal
            Case GetType(Byte)
                Return CTypes.Byte
            Case GetType(SByte)
                Return CTypes.SByte
        End Select
        Throw New Exception(String.Format("unsupported type '{0}'", datatype.Name))
    End Function
    ''' <summary>
    ''' The fixed catonic marker
    ''' </summary>
    ''' <returns></returns>
    Private Shared ReadOnly Property Header As Byte()
        Get
            Return {&H43, &H41}
        End Get
    End Property
End Class
