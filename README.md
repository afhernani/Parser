# Parser
# Consiste en un interprete para el calculo de expresiones matematicas sobre una cadena, es decir una simple calculadora.
# las operaciones que realiza son sumas, restas, multiplicación, divición y modulo, aunque es susceptible de añadidos para otras operaciones
# de la libreria matematicas.
# Utiliza una gramatica simple para resolver expresiones, aunque también permite el cálculo de expresiones.
# La version actual, de consola, debe ser modificada pues, sólo admite el primer flujo de cadena de entrada
# por lo que las operaciones se remiten al primer bucle, aunque, admita más, estos son independientes.
# Se puede solucionar declarando la expresion fuera del bucle, y ir añadiendo las cadenas sucesivas a procesar.

# La gramatica utilizada aparece reflejada en el fichero Token.cs
# Un ejemplo de resolución de expresion de forma simple seria:

      string cad = "21.8+3-5/3.11*(10+6+2)+4";
			cad = TextUtils.DelEmptyString(cad);
      //
			Expression expr = new Expression(new TokenStream(cad.Trim()));
			double d = expr.EvaluaExpression();
			Console.WriteLine(d);

# Esta claro que tambien hay que modificar otras cosas, pero en lo básico funciona
# sirviendo, como elemento de ejemplo para crear una clase base para el cálculo de
# expresiones simples.


# Hernnai
