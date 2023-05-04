using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Paradigmas.Controllers

{
    public class PasswordGenerator
    {
        private const string letrasMinusculas = "abcdefghijklmnopqrstuvwxyz";
        private const string LetrasMaisculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string numerico = "0123456789";
        private const string caracteriesEspecial = "!@#$%^&*()_+";
        private const int tamanhoMinimo = 8;

        private static readonly Regex HasNumber = new Regex(@"[0-9]+");
        private static readonly Regex HasUpperChar = new Regex(@"[A-Z]+");
        private static readonly Regex HasLowerChar = new Regex(@"[a-z]+");
        private static readonly Regex HasSymbols = new Regex(@"[!@#$%^&*()_+]+");


        // Método para gerar uma senha aleatória
        public static string GeneratePassword(int tamanho, bool incluirLetrasMinusculas, bool incluirLetrasMaiusculas, bool incluirNumeros, bool incluirCaracteresEspeciais)
        {
            // Concatena os caracteres escolhidos pelo usuário em uma string
            string charSet = "";
            if (incluirLetrasMinusculas)
            {
                charSet += letrasMinusculas;
            }
            if (incluirLetrasMaiusculas)
            {
                charSet += LetrasMaisculas;
            }
            if (incluirNumeros)
            {
                charSet += numerico;
            }
            if (incluirCaracteresEspeciais)
            {
                charSet += caracteriesEspecial;
            }

            // Verifica se o conjunto de caracteres está vazio
            if (string.IsNullOrEmpty(charSet))
            {
                throw new ArgumentException("Deve selecionar pelo menos um tipo de caractere para gerar uma senha.");
            }

            // Cria uma instância de RNGCryptoServiceProvider para gerar números aleatórios
            using (RandomNumberGenerator cryptoProvider = RandomNumberGenerator.Create())
            {
                // Converte o comprimento desejado da senha em bytes
                int byteLength = (int)Math.Ceiling(tamanho * 0.75);
                byte[] buffer = new byte[byteLength];
                int numChars = 0;
                char[] senha = new char[tamanho];

                // Loop para gerar bytes aleatórios até que tenha sido gerado o número de caracteres desejado
                while (numChars < tamanho)
                {
                    cryptoProvider.GetBytes(buffer);

                    // Loop pelos bytes gerados e converte cada byte em um caractere na string de caracteres
                    for (int i = 0; i < byteLength && numChars < tamanho; i++)
                    {
                        int value = buffer[i];

                        // Garante que o valor está dentro do intervalo permitido
                        while (value > byte.MaxValue - (byte.MaxValue % charSet.Length))
                        {
                            cryptoProvider.GetBytes(buffer, i, 1);
                            value = buffer[i];
                        }

                        // Converte o valor em um caractere e adiciona à senha
                        char c = charSet[value % charSet.Length];
                        if (incluirLetrasMinusculas && incluirCaracteresEspeciais && !char.IsLetterOrDigit(c))
                        {
                            // Se a senha deve incluir caracteres especiais e números, mas o caractere gerado não é nem um nem outro,
                            // gere outro caractere aleatório.
                            continue;
                        }
                        senha[numChars] = c;
                        numChars++;
                    }
                }
                return new string(senha);
            }
        }
        public enum PasswordStrength
        {
            VeryWeak,
            Weak,
            Medium,
            Strong,
            VeryStrong
        }
        public static PasswordStrength CheckPasswordStrength(string senha)
{
    if (string.IsNullOrEmpty(senha))
    {
        return PasswordStrength.VeryWeak;
    }

    int score = 0;

    if (senha.Length >= tamanhoMinimo)
    {
        score++;
    }

    if (HasNumber.IsMatch(senha))
    {
        score++;
    }

    if (HasUpperChar.IsMatch(senha))
    {
        score++;
    }

    if (HasLowerChar.IsMatch(senha))
    {
        score++;
    }

    if (HasSymbols.IsMatch(senha))
    {
        score++;
    }

    switch (score)
    {
        case 1:
            return PasswordStrength.VeryWeak;
        case 2:
            return PasswordStrength.Weak;
        case 3:
            return PasswordStrength.Medium;
        case 4:
            return PasswordStrength.Strong;
        case 5:
            return PasswordStrength.VeryStrong;
        default:
            return PasswordStrength.VeryWeak;
    }
}
    }
    
}
