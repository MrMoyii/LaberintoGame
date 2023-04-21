using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoRetro
{
    public class Vista
    {
        public void MostrarMenu()
        {
            GameLoop gameLoop = new GameLoop();
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("¡Hola, 'P'layer espero que estes listo para perder!" +
                    "\nEn este minijuego tendrás que superar 3 laberintos, recogiendo todos los puntos" +
                    "\nde cada nivel para que así se desbloquee la puerta(K) y puedas avanzar." +
                    "\nPero no será tan sencillo, ya que un 'E'nemigo te estará persiguiendo >:)");
                Console.WriteLine("\n[1] Jugar \n[0] Salir");
            } while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 0 || opcion > 1);

            if (opcion == 1) gameLoop.IniciarGameLoop();
        }
    }
}
