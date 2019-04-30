Imports Microsoft.AspNet.Identity
Imports System.Security.Cryptography
Imports System.Text

Public Class PasswordHasher
    Implements IPasswordHasher

    Public Function HashPassword(password As String) As String Implements IPasswordHasher.HashPassword
        Return GetMd5Hash(password)
    End Function

    Public Function VerifyHashedPassword(hashedPassword As String, providedPassword As String) As PasswordVerificationResult Implements IPasswordHasher.VerifyHashedPassword
        Return VerifyMd5Hash(providedPassword, hashedPassword)
    End Function

    Shared Function GetMd5Hash(ByVal input As String) As String
        Dim md5Hash = MD5.Create

        ' Convert the input string to a byte array and compute the hash. 
        Dim data As Byte() = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input))

        ' Create a new Stringbuilder to collect the bytes 
        ' and create a string. 
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data  
        ' and format each one as a hexadecimal string. 
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string. 
        Return sBuilder.ToString()

    End Function 'GetMd5Hash

    Shared Function GetMd5Hash(ByVal input As Byte()) As String
        Dim md5Hash = MD5.Create

        ' Convert the input string to a byte array and compute the hash. 
        Dim data As Byte() = md5Hash.ComputeHash(input)

        ' Create a new Stringbuilder to collect the bytes 
        ' and create a string. 
        Dim sBuilder As New StringBuilder()

        ' Loop through each byte of the hashed data  
        ' and format each one as a hexadecimal string. 
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        ' Return the hexadecimal string. 
        Return sBuilder.ToString()

    End Function 'GetMd5Hash


    Shared Function VerifyMd5Hash(ByVal input As String, ByVal hash As String) As Boolean
        ' Hash the input. 
        Dim hashOfInput As String = GetMd5Hash(input)

        ' Create a StringComparer an compare the hashes. 
        Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase

        If 0 = comparer.Compare(hashOfInput, hash) Then
            Return True
        Else
            Return False
        End If

    End Function 'VerifyMd5Hash

    Shared Function VerifyMd5Hash(ByVal input As Byte(), ByVal hash As Byte()) As Boolean
        ' Hash the input. 
        Dim hashOfInput As String = GetMd5Hash(input)

        If input.SequenceEqual(hash) Then
            Return True
        Else
            Return False
        End If

    End Function 'VerifyMd5Hash
End Class

