using System;
using System.Security.Cryptography;
using System.Text;

namespace TiendaRopaCano.Aplicacion.Auxiliares
{
    /// <summary>
    /// Proporciona métodos de utilidad estáticos para realizar el hashing y la verificación segura de contraseñas utilizando el algoritmo PBKDF2.
    /// </summary>
    public static class EncriptadorContrasena
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 10000;

        /// <summary>
        /// Crea un hash seguro para una contraseña en texto plano utilizando PBKDF2 y un salt aleatorio.
        /// </summary>
        /// <param name="password">La contraseña en texto plano a encriptar.</param>
        /// <returns>Una cadena con formato {Iteraciones}.{SaltBase64}.{HashBase64} lista para almacenarse en la base de datos.</returns>
        public static string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con el hash almacenado en la base de datos.
        /// Soporta una caída de compatibilidad (fallback) para contraseñas en texto plano si no contienen puntos.
        /// </summary>
        /// <param name="hash">El hash almacenado con formato de tres partes separadas por puntos.</param>
        /// <param name="password">La contraseña ingresada en texto plano que se desea validar.</param>
        /// <returns><c>true</c> si la contraseña es válida; de lo contrario, <c>false</c>.</returns>
        public static bool Verify(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                // Fallback para texto plano (si existen usuarios antiguos sin hashear)
                return hash == password;
            }

            var iterations = int.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, key);
        }
    }
}
