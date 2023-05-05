using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Paradigmas.Controllers

{
    public class PasswordGenerator
    {
        // Conjuntos de caracteres para inclusão na senha
        private const string letrasMinusculas = "abcdefghijklmnopqrstuvwxyz";
        private const string LetrasMaisculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string numerico = "0123456789";
        private const string caracteriesEspecial = "!@#$%^&*()_+";

         // Comprimento mínimo da senha
        private const int tamanhoMinimo = 8;

         // Expressões regulares para verificar a presença de caracteres específicos    
        private static readonly Regex HasNumber = new Regex(@"[0-9]+");
        private static readonly Regex HasUpperChar = new Regex(@"[A-Z]+");
        private static readonly Regex HasLowerChar = new Regex(@"[a-z]+");
        private static readonly Regex HasSymbols = new Regex(@"[!@#$%^&*()_+]+");


        // Método para gerar uma senha aleatória
        public static string GeneratePassword(int tamanho, bool incluirLetrasMinusculas, bool incluirLetrasMaiusculas, bool incluirNumeros, bool incluirCaracteresEspeciais)
        {
            // Concatena os caracteres escolhidos pelo usuário em uma string
            string charSet = "";
            if (tamanho < tamanhoMinimo)
            {
                throw new ArgumentException($"O tamanho mínimo da senha é {tamanhoMinimo} caracteres.");
            }
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
            if (charSet == null)
            {
                throw new ArgumentNullException(nameof(charSet));
            }

            if (tamanho <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tamanho), "O tamanho deve ser maior do que zero.");
            }

            // Converte o comprimento desejado da senha em bytes
            int byteLength = (int)Math.Ceiling(tamanho * 0.75);
            byte[] buffer = new byte[byteLength];
            int numChars = 0;
            char[] senha = new char[tamanho];

            // Loop para gerar bytes aleatórios até que tenha sido gerado o número de caracteres desejado
            while (numChars < tamanho)
            {
                try
                {
                    cryptoProvider.GetBytes(buffer);
                }
                catch (CryptographicException ex)
                {
                    throw new CryptographicException("Erro ao gerar bytes aleatórios.", ex);
                }

                // Loop pelos bytes gerados e converte cada byte em um caractere na string de caracteres
                for (int i = 0; i < byteLength && numChars < tamanho; i++)
                {
                    int value = buffer[i];

                    // Garante que o valor está dentro do intervalo permitido
                    while (value > byte.MaxValue - (byte.MaxValue % charSet.Length))
                    {
                        try
                        {
                            cryptoProvider.GetBytes(buffer, i, 1);
                        }
                        catch (CryptographicException ex)
                        {
                            throw new CryptographicException("Erro ao gerar bytes aleatórios.", ex);
                        }
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
            MuitoFraca,
            Fraca,
            Media,
            Forte,
            MuitoForte
        }
        public static PasswordStrength CheckPasswordStrength(string senha)
{
     // Verifica se a senha é nula ou vazia e retorna "Muito Fraca" se for o caso
    int score = 0;

if (string.IsNullOrEmpty(senha))
{
    return PasswordStrength.MuitoFraca;
}

for (int i = 0; i <= 5; i++)
{
    if (i == 0 && senha.Length >= tamanhoMinimo)
    {
        score++;
    }
    else if (i == 1 && HasNumber.IsMatch(senha))
    {
        score++;
    }
    else if (i == 2 && HasUpperChar.IsMatch(senha))
    {
        score++;
    }
    else if (i == 3 && HasLowerChar.IsMatch(senha))
    {
        score++;
    }
    else if (i == 4 && HasSymbols.IsMatch(senha))
    {
        score++;
    }
}

    // Retorna o nível de segurança com base na pontuação obtida
    switch (score)
    {
        case 1:
            return PasswordStrength.MuitoFraca;
        case 2:
            return PasswordStrength.Fraca;
        case 3:
            return PasswordStrength.Media;
        case 4:
            return PasswordStrength.Forte;
        case 5:
            return PasswordStrength.MuitoForte;
        default:
            return PasswordStrength.MuitoFraca;
    }
}
    }
    
}
