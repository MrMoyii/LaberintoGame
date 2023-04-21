using System;
using System.Collections.Generic;
using System.Threading;

namespace JuegoRetro
{
    public class GameLoop
    {
        private int jugadorPosX = 1;
        private int jugadorPosY = 1;

        private int enemigoPosX = 6;
        private int enemigoPosY = 6;

        private int mapaNumero = 0;
        private bool gano = false;

        Laberinto lab = new Laberinto();
        public void IniciarGameLoop()
        {
            char[,] mapaActual = lab.DevolverLaberinto(mapaNumero);
            bool alcanzado;

            ConsoleKeyInfo tecla = new ConsoleKeyInfo();
            do
            {
                if (mapaNumero > 2) 
                {
                    gano = true;
                    break;
                }
                
                Console.Clear();
                DibujarLaberinto(mapaActual);
                ImprimirPuntos(mapaActual);

                DibujarJugador();
                DibujarEnemigo();

                alcanzado = TeAlcanzoElEnemigo();
                if (!alcanzado)
                {
                    tecla = Console.ReadKey();
                    MoverJugador(tecla.Key, ref mapaActual);
                    MoverEnemigo(ref mapaActual);
                }
            } while (tecla.Key != ConsoleKey.Escape && !alcanzado);

            if (gano)
            {
                //Terminar
                //-- o podria hacer que el gameLoop retorne un bool que siga si gano o no --//
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Creo que te han atrapado, inténtalo de nuevo!");
                Console.ReadKey();
                Vista v = new Vista();
                v.MostrarMenu();
            }
        }

        private void ImprimirPuntos(char[,] mapaActual)
        {
            //puntos restantes:
            int cantPuntos = CantidadDePuntos(mapaActual);
            Console.WriteLine($"Mapa N°: {mapaNumero + 1}");
            Console.WriteLine($"Cantidad de puntos restantes: {cantPuntos}");
            if (cantPuntos <= 1)
            {
                DibujarPuerta();
            }
        }
        private bool TeAlcanzoElEnemigo()
        {
            return jugadorPosX == enemigoPosX && jugadorPosY == enemigoPosY;
        }
        private void DibujarLaberinto(char[,] laberinto)
        {
            for (int i = 0; i < laberinto.GetLength(0); i++)
            {
                for (int j = 0; j < laberinto.GetLength(1); j++)
                {
                    Console.Write(laberinto[i, j]);
                }
                Console.WriteLine();
            }
        }
        private int CantidadDePuntos(char[,] laberinto)
        {
            int puntos = 0;
            for (int i = 0; i < laberinto.GetLength(0); i++)
            {
                for (int j = 0; j < laberinto.GetLength(1); j++)
                {
                    if (laberinto[i,j] == '.')
                    {
                        puntos++;
                    }
                }
            }
            return puntos;
        }
        private void DibujarJugador()
        {
            Console.SetCursorPosition(jugadorPosY, jugadorPosX);
            Console.Write('P');
        }
        private void DibujarEnemigo()
        {
            Console.SetCursorPosition(enemigoPosY, enemigoPosX);
            Console.Write('E');
        }
        private void MoverJugador(ConsoleKey tecla, ref char[,] laberinto)
        {
            int nuevaPosX = jugadorPosX;
            int nuevaPosY = jugadorPosY;

            switch (tecla)
            {
                case ConsoleKey.UpArrow:
                    nuevaPosX--;
                    break;
                case ConsoleKey.DownArrow:
                    nuevaPosX++;
                    break;
                case ConsoleKey.LeftArrow:
                    nuevaPosY--;
                    break;
                case ConsoleKey.RightArrow:
                    nuevaPosY++;
                    break;
            }

            if (laberinto[nuevaPosX, nuevaPosY] != '#')
            {
                laberinto[jugadorPosX, jugadorPosY] = ' ';
                jugadorPosX = nuevaPosX;
                jugadorPosY = nuevaPosY;
            }
            if (laberinto[nuevaPosX, nuevaPosY] == 'K')
            {
                mapaNumero++;
                laberinto = lab.SiguienteNivel(mapaNumero);
                jugadorPosX = 1;
                jugadorPosY = 1;
                SpawnearEnemigo(ref laberinto);
            }
        }
        private void SpawnearEnemigo(ref char[,] laberinto)
        {
            Random r = new Random();
            do
            {
                enemigoPosX = r.Next(7, 11);
                enemigoPosY = r.Next(7, 11);
                
            } while (laberinto[enemigoPosX, enemigoPosY] == '#');
        }
        private void MoverEnemigo(ref char[,] laberinto)
        {
            // Buscar la posición del jugador
            int jugadorDistanciaX = jugadorPosX - enemigoPosX;
            int jugadorDistanciaY = jugadorPosY - enemigoPosY;

            // Mover al enemigo en la dirección del jugador
            if (Math.Abs(jugadorDistanciaX) > Math.Abs(jugadorDistanciaY))
            {
                if (jugadorDistanciaX > 0 && laberinto[enemigoPosX + 1, enemigoPosY] != '#')
                {
                    enemigoPosX++;
                }
                else if (jugadorDistanciaX < 0 && laberinto[enemigoPosX - 1, enemigoPosY] != '#')
                {
                    enemigoPosX--;
                }
            }
            else
            {
                if (jugadorDistanciaY > 0 && laberinto[enemigoPosX, enemigoPosY + 1] != '#')
                {
                    enemigoPosY++;
                }
                else if (jugadorDistanciaY < 0 && laberinto[enemigoPosX, enemigoPosY - 1] != '#')
                {
                    enemigoPosY--;
                }
            }
        }
        private void DibujarPuerta()
        {
            lab.CrearPuerta(mapaNumero);
        }
    }
}
