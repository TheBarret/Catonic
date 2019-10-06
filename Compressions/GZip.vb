Imports System.IO
Imports System.IO.Compression
Namespace Compressions
    Public Class GZip
        Public Shared Function Compress(data() As Byte) As Byte()
            Using ms As New MemoryStream
                Using gzs As New GZipStream(ms, CompressionLevel.Optimal, True)
                    gzs.Write(data, 0, data.Length)
                End Using
                Return ms.ToArray
            End Using
        End Function
        Public Shared Function Decompress(data() As Byte) As Byte()
            Using input As New MemoryStream(data)
                Using gzs As New GZipStream(input, CompressionMode.Decompress)
                    Using ms As New MemoryStream
                        gzs.CopyTo(ms)
                        Return ms.ToArray
                    End Using
                End Using
            End Using
        End Function

    End Class
End Namespace
