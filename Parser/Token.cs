/*
 * Creado por SharpDevelop.
 * Usuario: hernani
 * Fecha: 01/05/2017
 * Hora: 11:25
 * 
 The grammar for input is:
	Statement:
		Expression
		Print
		Quit
	Print:
		;
	Quit:
		q
	Expression:
		Term
		Expression + Term
		Expression – Term
	Term:
		Primary
		Term * Primary
		Term / Primary
		Term % Primary
	Primary:
		Number
		( Expression )
		– Primary
		+ Primary
	Number:
		floating-point-literal
	Input comes from cin through the Token_stream called ts.
 *
 * 
 * gramatica para variables
   Calculation:
		Statement
		Print
		Quit
		Calculation Statement
	Statement:
		Declaration
		Expression
	Declaration:
		"let" Name "=" Expression
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Parser
{
	public enum Kind
	{
		number,
		variable,
		quit,
		print,
	}
	/// <summary>
	/// Description of Token.
	/// </summary>
	public class Token
	{
		//constantes globales.
		public const char number = '8';
		// t.kind==number means that t is a number Token
		public const char quit = 'q';
		// t.kind==quit means that t is a quit Token
		public const char print = ';';
		//t.kind==print means that t is a print Token
		public const char end = '|';
		public const string pront = "> ";
		//pront de consola
		public const string result = "= ";
		//indicar resultado
		//contantes token.
		/// <summary>
		/// what kind of token
		/// </summary>
		public char Kind{ get; set; }
		/// <summary>
		/// for numbers: a value 
		/// </summary>
		public double Val{ get; set; }
		public string Name{ get; set; }
		/// <summary>
		/// make a Token from a char and a double
		/// </summary>
		/// <param name="ch"></param>
		/// <param name="val"></param>
		public Token(char ch, double val)
		{
			Kind = ch;
			Val = val;
		}
		public Token(char ch, string name)
		{
			Kind = ch;
			Name = name;
		}
		/// <summary>
		/// make a Token from a char
		/// </summary>
		/// <param name="ch"></param>
		public Token(char ch)
			: this(ch, 0)
		{
		}
	}
	
	/// <summary>
	/// clase tokenstream
	/// </summary>
	public class TokenStream
	{
		StreamReader Str{ get; set; }
		public TokenStream(string cad)
			: this()
		{
			AddStream(cad);
		}
		public void AddStream(string cad)
		{
			// convert string to stream
			byte[] byteArray = Encoding.UTF8.GetBytes(cad);
			//byte[] byteArray = Encoding.ASCII.GetBytes(contents);
			MemoryStream stream = new MemoryStream(byteArray);
			// convert stream to string
			Str = new StreamReader(stream);
		}
		public TokenStream(StreamReader str)
			: this()
		{
			Str = str;
		}
		/// <summary>
		/// make a Token_stream that reads from
		/// The constructor just sets full to indicate 
		/// that the buffer is empty:
		/// </summary>
		public TokenStream()
		{
			this.full = false;
			this.buffer = null; //no Token in buffer
		}
		/// <summary>
		/// end of TokenStream ture.
		/// </summary>
		public bool EndOfStream {
			get {
				return Str.EndOfStream;
			}
		}
		/// <summary>
		/// get a Token (get() is defined elsewhere)
		/// </summary>
		/// <returns></returns>
		public Token get()
		{
			if (full) { //do we already have a Token ready?
				full = false; //remove Token from buffer
				return buffer;
			}
			char ch;
			if (Str.EndOfStream)
				return new Token(Token.end);
			ch = Convert.ToChar(Str.Read());
			switch (ch) {
				case ' ':
					//Ignore(' ');
					return get();
				case '\0':
				case Token.print:
				case Token.quit:
				case '=':
				case '(':
				case ')': 
				case '+': 
				case '-': 
				case '*': 
				case '/': 
				case '%':
					Debug.WriteLine(ch);
					return new Token(ch);        // let each character represent itself
			//break;
				case '.':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					{
						StringBuilder strb = new StringBuilder();
						strb.Append(ch);
						
						while (!Str.EndOfStream) {
							if (TextUtils.IsNumeric(ch = Convert.ToChar(Str.Peek())) || Convert.ToChar(Str.Peek()) == '.') {
								ch = Convert.ToChar(Str.Read());
								strb.Append(ch);
							} else {
								break;
							}
						}
						
						Debug.WriteLine(strb.ToString());
						return new Token(Token.number, Convert.ToDouble(strb.ToString(), System.Globalization.CultureInfo.InvariantCulture));
					}
			//break;
				default:
					if (TextUtils.IsAlphabetic(ch)) 
					{
						StringBuilder name = new StringBuilder();
						
						name.Append(ch);
						
						while (!Str.EndOfStream) 
						{
							if (TextUtils.IsAlphabetic(ch = Convert.ToChar(Str.Peek()))) {
								ch = Convert.ToChar(Str.Read());
								name.Append(ch);
							} else 
							{
								break;
							}
						}
						
						Debug.WriteLine("variable nombre = "+ name.ToString());
						Token t= new Token('L', name.ToString());
						return t;
				
					}
					throw new Exception("exception bad get Token");
			//break;					
			}
		}
		/// <summary>
		/// put a Token back
		/// </summary>
		/// <param name="t"></param>
		public void putback(Token t)
		{
			if (full)
				throw new Exception("putback() into a full buffer");
			buffer = t; // copy t to buffer
			full = true; // buffer is now full
		}
		/// <summary>
		/// discard tokens up to an including a c
		/// </summary>
		/// <param name="ch"></param>
		public void Ignore(char ch)
		{
			if (full && ch == buffer.Kind) {
				full = false;
				return;
			}
			full = false;
			char c;
			while (!Str.EndOfStream) {
				c = Convert.ToChar(Str.Peek());
				if (c == ch){
					Str.Read();
					return;
				}
			}
			
		}
		public void Ignore(){
			Str.Read();
		}
		/// <summary>
		/// is there a Token in the buffer?
		/// </summary>
		private bool full{ get; set; }
		/// <summary>
		/// here is where we keep a Token put back using putback()
		/// </summary>
		private Token buffer{ get; set; }
	}
	
	public class Variable
	{		
		public string name{ get; set; }
		public double value{ get; set; }
		public Variable(string n, double v)
		{
			name = n;
			value = v;
		}
	}
	/// <summary>
	/// clase Array de variables
	/// </summary>
	public class ArrayVariables
	{
		List<Variable> variable_tabla;
		
		public ArrayVariables()
		{
			variable_tabla = new List<Variable>();
		}
		public double getValue(string n)
		{
			foreach (var element in variable_tabla) {
				if (element.name.Equals(n))
					return element.value;
			}
			throw new Exception("variable no definida .. n ");
		}
		public void setValue(string n, double value)
		{
			foreach (var element in variable_tabla) {
				if (element.name.Equals(n)) {	
					element.value = value;
					return;
				}
			}
			throw new Exception("variable no definida .. n - vlue ");
		}
		
		public bool isdeclared(string n)
		{
			foreach (var element in variable_tabla) {
				if (element.name.Equals(n)) {				
					return true;
				}
			}
			return false;
		}
		public void add(Variable var)
		{
			variable_tabla.Add(var);
		}
		
	}
	
	public class Expression
	{
		TokenStream ts;
		
		public Expression()
		{
			
		}
		public Expression(TokenStream ts)
		{
			this.ts = ts;
		}
		public void AddTokenStream(TokenStream ts)
		{
			this.ts = ts;
		}
		public double EvaluaExpression()
		{
			double left = Term();
			Token t = ts.get();
			while (true) {
				switch (t.Kind) {
					case '+':
						left += Term();
						t = ts.get();
						break;
					case '-':
						left -= Term();
						t = ts.get();
						break;
					default:
						ts.putback(t);
						return left;
				}
			}
			
		}

		double Term()
		{
			double left = Primary();
			Token t = ts.get();
			while (true) {
				switch (t.Kind) {
					case '*':
						left *= Primary();
						t = ts.get();
						break;
					case '/':
						double d = Primary();
						if (d == 0)
							throw new Exception("Divicion por cero..");
						left /= d;
						t = ts.get();
						break;
					case '%':
						d = Primary();
						if (d == 0)
							throw new Exception("Divicion por cero ...");
						left %= d;
						t = ts.get();
						break;
					default:
						ts.putback(t);
						return left;	
				}
			}
		}

		double Primary()
		{
			Token t = ts.get();
			switch (t.Kind) {
				case '(': //maneja una (expresion)
					{
						double d = EvaluaExpression();
						t = ts.get();
						if (t.Kind != ')')
							throw new Exception(" ')' excepcion.");
						return d;
					}
				case '8':
					return t.Val;
				case 'L':
					//TOTO: depurar para obtener el valor de la varibla
					return variable_lista.getValue(t.Name);
					//return t.Val;
				case '-':
					return -Primary();
				case '+':
					return Primary();
				default:
					throw new Exception("Primary exception...");
			}
		}
		
		TextWriter cout = Console.Out;
		readonly TextReader cin = Console.In;
		/// <summary>
		/// calcular la expresion
		/// </summary>
		public void Calculate()
		{
			while (!ts.EndOfStream)
			{
				try
				{
					cout.Write('>');
					Token t = ts.get();
					while (t.Kind == ';')
						ts.get(); 
					if(t.Kind=='q') return;
					ts.putback(t);
					cout.Write("= " + Statement() + "\n");
					
				}catch(Exception ex)
				{
					Debug.WriteLine(ex.ToString());
				}
			}		
		}
		//
		double Statement()
		{
			Token T = ts.get();
			switch (T.Kind)
			{
				case 'L':
					ts.putback(T);
					return Declaration();
				default:
					ts.putback(T);
					return EvaluaExpression();
			}
		}
		//
		ArrayVariables variable_lista = new ArrayVariables();
		//	
		double Declaration()
		{
			Token t = ts.get();
			if (t.Kind != 'L')
				throw new Exception("Name variable not establecida ");
			string name_variable = t.Name;
			Token t2 = ts.get();
			if (t2.Kind != '=')
				throw new Exception(" = se ha perdido en la declaracion de la variable" + t.Name);
			double d = EvaluaExpression();
			Variable variable = new Variable(t.Name, d);
			variable_lista.add(variable);
			t.Val = d;
			return d;
		}
		//
	}
	
}
