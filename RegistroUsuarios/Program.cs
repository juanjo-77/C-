

using System;

class Program
{
    static void Main()
    {
        Console.Write("Ingrese su nombre: ");
        string name = Console.ReadLine();

        Console.Write("Ingrese su edad: ");
        string input = Console.ReadLine();

        int edad;

        if (!int.TryParse(input, out edad)){
            Console.WriteLine("Ingresa un dato numerico");
            return;
        }



        Console.Write("Ingrese su ciudad: ");
        string city = Console.ReadLine();

        Console.WriteLine("-----------------------------------------------");


        if (edad < 12)
        {
            Console.WriteLine($"Hola {name} desde {city}\n Tienes {edad} años y perteneces a la categoria: Niño \n Acceso: Denegado - no cumples con la edad minima");
        }
        else if (edad >= 12 && edad <= 17)
        {
            Console.WriteLine($"Hola {name} eres de {city}\n Tienes {edad} años y perteneces a la categoria: Adolescente \n Acceso: Denegado - no cumples con la edad minima");
        }
        else if (edad >= 18)
        {
            Console.WriteLine($"Hola {name} eres de {city}\n Tienes {edad} años y perteneces a la categoria: Adulto \n Acceso: Permitido");
        }
        else
        {
            Console.WriteLine("Verifica los datos ingresados \n Acceso:Denegado");
        }
    }
}