using System.Security.Cryptography;
using System.Text;

namespace TurboApi.Extensions
{
	public static class StringExtensions
	{
		public static string CreateMD5(this string str)
		{
			var inputBytes = Encoding.ASCII.GetBytes(str);
			using (var md5 = MD5.Create())
			{
				var hashBytes = md5.ComputeHash(inputBytes);

				var sb = new StringBuilder();
				foreach (var t in hashBytes)
				{
					sb.Append(t.ToString("X2"));
				}
				return sb.ToString();
			}
		}
	}
}
