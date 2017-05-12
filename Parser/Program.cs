/*
 * Creado por SharpDevelop.
 * Usuario: hernani
 * Fecha: 12/05/2017
 * Hora: 17:42
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Parser
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("- testEvaluaExpresion -");
			testEvaluaExpresion();
			Console.WriteLine("- Loop Consola calculadora -");
			Loop();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		static void testEvaluaExpresion()
		{
			string cad = "21.8+3-5/3.11*(10+6+2)+4";
			cad = TextUtils.DelEmptyString(cad);
			Console.WriteLine("Evalua la expresion --> " + cad);
			try {
				Expression expr = new Expression(new TokenStream(cad.Trim()));
				double d = expr.EvaluaExpression();
				Console.WriteLine(d);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}	
		}
		//
		static void Loop()
		{
			bool userWantsToExit = false;
			TextReader cin = Console.In;
			TextWriter Out = Console.Out;
			string cad;
			//get input
			while (!userWantsToExit) 
			{
				{
					Out.Write('>');
					cad = cin.ReadLine();
				}
				try 
				{
					Expression expr = new Expression(new TokenStream(cad.Trim()));
					expr.Calculate();
				} 
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}	
				if (cad.ToUpper().Equals("exit".ToUpper()))
					userWantsToExit = true;
			}
		}
	}
}