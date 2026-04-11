using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using System;


class suma
{
    public void Numeros()
    {
        Console.Write("Ingrese el primer número:");
        string num1 = Console.ReadLine();

        if (!int.TryParse(num1, out int numero1))
        {
            Console.WriteLine("Entrada no válida. Por favor, ingrese un número entero.");
            return;
        }

        Console.Write("Ingrese el segundo número:");
        string num2 = Console.ReadLine();

        if (!int.TryParse(num2, out int numero2))
        {
            Console.WriteLine("Entrada no válida. Por favor, ingrese un número entero.");
            return;
        }
        int suma = numero1 + numero2;
        Console.WriteLine($"La suma de {numero1} y {numero2} es: {suma}");
    }
}

//-------------------------------------------------------------------------------------------------

class Calculo
{
    public void calificacion()
    {
        Console.Write("ingresa la primera claificacion: ");
        string cal1 = Console.ReadLine();

        if (!double.TryParse(cal1, out double calificacion1))
        {
            Console.WriteLine("Entrada no válida. Por favor, ingrese un número decimal.");
            return;
        }

        Console.Write("ingresa la segunda claificacion: ");
        string cal2 = Console.ReadLine();

        if (!double.TryParse(cal2, out double calificacion2))
        {
            Console.WriteLine("Entrada no válida. Por favor, ingrese un número decimal.");
            return;
        }

        Console.Write("ingresa la tercera claificacion: ");
        string cal3 = Console.ReadLine();

        if (!double.TryParse(cal3, out double calificacion3))
        {
            Console.WriteLine("Entrada no válida. Por favor, ingrese un número decimal.");
            return;
        }

        double promedio = (calificacion1 + calificacion2 + calificacion3) / 3;
        Console.WriteLine($"El promedio de las calificaciones es: {promedio}");
    }
}


class Cadenas
{
    public void ManipularCadenas()
    {
        Console.WriteLine("Ingrese su nombre: ");
        string nombre = Console.ReadLine();

        Console.WriteLine("Ingrese su apellido: ");
        string apellido = Console.ReadLine();

        string nombreCompleto = $"{nombre} {apellido}";
        Console.WriteLine($"Hola {nombreCompleto} ojala te encuestres bien");
    }
}



class calculadora
{
    public void operaciones()
    {
        Console.WriteLine("ingrese el primer numero: ");
        string num1 = Console.ReadLine();

        if (!int.TryParse(num1, out int numero1))
        {
            Console.WriteLine("Entrada no válida. Por favor, ingrese un número entero.");
            return;
        }

        Console.WriteLine("ingrese el segundo numero: ");
        string num2 = Console.ReadLine();

        if (!int.TryParse(num2, out int numero2))
        {
            Console.WriteLine("Entrada no válida. Por favor, ingrese un número entero.");
            return;
        }

        Console.WriteLine("Seleccione la operación a realizar:");
        Console.WriteLine("1. Suma");
        Console.WriteLine("2. Resta");
        Console.WriteLine("3. Multiplicación");
        Console.WriteLine("4. División");

        string operacion = Console.ReadLine();

        switch (operacion)
        {
            case "1":
                Console.WriteLine($"La suma de {numero1} y {numero2} es: {numero1 + numero2}");
                break;
            case "2":
                Console.WriteLine($"La resta de {numero1} y {numero2} es: {numero1 - numero2}");
                break;
            case "3":
                Console.WriteLine($"La multiplicación de {numero1} y {numero2} es: {numero1 * numero2}");
                break;
            case "4":
                if (numero2 != 0)
                {
                    Console.WriteLine($"La división de {numero1} entre {numero2} es: {(double)numero1 / numero2}");
                }
                else
                {
                    Console.WriteLine("Error: No se puede dividir entre cero.");
                }
                break;
            default:
                Console.WriteLine("Operación no válida.");
                break;
        }
    }
}


class ListaEnteros
{
    public void CrearLista()
    {
        List<int> numeros = new List<int>();
        Console.WriteLine("Ingrese 5 números enteros (escriba 'fin' para terminar):");

        Console.WriteLine("Ingrese el número 1:");
        int input1 = int.Parse(Console.ReadLine());

        Console.WriteLine("Ingrese el número 2:");
        int input2 = int.Parse(Console.ReadLine());

        Console.WriteLine("Ingrese el número 3:");
        int input3 = int.Parse(Console.ReadLine());

        Console.WriteLine("Ingrese el número 4:");
        int input4 = int.Parse(Console.ReadLine());

        Console.WriteLine("Ingrese el número 5:");
        int input5 = int.Parse(Console.ReadLine());

        numeros.Add(input1);
        numeros.Add(input2);
        numeros.Add(input3);
        numeros.Add(input4);
        numeros.Add(input5);

        foreach (int numero in numeros)
        {
            Console.WriteLine("la suma total es: " + numero);
        }


    }
}




class Program
{
    
    static void Main()
    {
        Console.WriteLine("===========================");
        Console.WriteLine("Seleccione una acción:");
        Console.WriteLine("===========================");
        Console.WriteLine("Selecciona 1 para sumar: ");
        Console.WriteLine("Selecciona 2 para calcular: ");
        Console.WriteLine("Selecciona 3 para cadenas: ");
        Console.WriteLine("Selecciona 4 para calculadora: ");
        Console.WriteLine("Selecciona 5 para lista de enteros: ");
        Console.WriteLine("Selecciona 6 para lista de cadenas: ");
        Console.WriteLine("Selecciona 7 para arreglos y busqueda: ");
        Console.WriteLine("Selecciona 8 para clases de POO: ");
        Console.WriteLine("Selecciona 9 para Lista de objetos: ");
        Console.WriteLine("Selecciona 10 para salir: ");
        Console.WriteLine("===========================");

        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("ingrese dos numeros para sumarlos");
                suma s = new suma();
                s.Numeros();
                break;
            case "2":
                Console.WriteLine("ingrese tres calificaciones para calcular el promedio");
                Calculo c = new Calculo();
                c.calificacion();
                break;
            case "3":
                Console.WriteLine("Ingrese su nombre y apellido");
                Cadenas cadenas = new Cadenas();
                cadenas.ManipularCadenas();
                break;
            case "4":
                Console.WriteLine("Ingrese dos números y seleccione una operación");
                calculadora calculadora = new calculadora();
                calculadora.operaciones();
                break;
            case "5":
                Console.WriteLine("Ingrese 5 números enteros para sumarlos");
                ListaEnteros listaEnteros = new ListaEnteros();
                listaEnteros.CrearLista();
                break;
            case "6":
                Console.WriteLine("Acción 6");
                break;
            case "7":
                Console.WriteLine("Acción 7");
                break;
            case "8":
                Console.WriteLine("Acción 8");
                break;
            case "9":
                Console.WriteLine("Acción 9");
                break;
            case "10":
                Console.WriteLine("Saliendo...");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Acción no válida");
                break;
        }
    }
}