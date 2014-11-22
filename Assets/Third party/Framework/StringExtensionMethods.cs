using System.Collections.Generic;

public static class StringExtensionMethods
{
	public static string RemoveAllChars ( this string self, char charToRemove )
	{
		string result = "";
		for ( int i = 0; i < self.Length; i++ )
		{
			char c = self[i];
			if ( c != charToRemove )
				result += c;
		}
		return result;
	}

	public static string Wrap ( this string self, int maxLineLength, WrapStringOption option )
	{
		string result = "";
		if ( maxLineLength > 1 )
		{
			bool slashR = false;
			int lineLength = 0;
			// string[] words = self.Split ( new char[] { '\n', '\r', ' ' }, System.StringSplitOptions.None );
			string[] words = self.SplitAndKeep ( new char[] { '\n', '\r', ' ' } ); // '\n\r '
			foreach ( string word in words )
			{
				if ( word == "\r" )
					slashR = true;
				else
				{
					if ( word == " " )
					{ }
					else if ( word == "\n" )
					{
						if ( !slashR )
						{
							result += "\n";
							lineLength = 0;
						}
					}
					else
					{
						if ( option == WrapStringOption.WrapBefore )
							lineLength += word.Length;

						if ( result.Length > 0 )
							if ( lineLength >= maxLineLength )
							{
								result += "\r\n";
								if ( option == WrapStringOption.WrapBefore )
									lineLength = word.Length;
								else
									lineLength = 0;
							}
							else
								result += " ";

						result += word;

						if ( option == WrapStringOption.WrapAfter )
							lineLength += word.Length;
					}

					slashR = false;
				}
			}
		}
		return result;
	}

	public static string[] SplitAndKeep ( this string self, params char[] delimiters )
	{
		List<string> result = new List<string> ();
		string word = "";
		foreach ( char c in self )
		{
			bool isDelimiter = false;
			foreach ( char d in delimiters )
			{
				if ( c == d )
				{
					isDelimiter = true;
					break;
				}
			}

			if ( isDelimiter )
			{
				if ( word.Length > 0 )
				{
					result.Add ( word );
					word = "";
				}
				result.Add ( c.ToString () );
			}
			else
				word += c;
		}

		if ( word.Length > 0 )
			result.Add ( word );

		return result.ToArray ();
	}
}