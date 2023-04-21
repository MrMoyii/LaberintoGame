using System;
using System.Collections.Generic;
using System.Threading;

namespace JuegoRetro
{
    public static class GameLoop
    {
        static int jugadorPosX = 1;
        static int jugadorPosY = 1;

        static int enemigoPosX = 6;
        static int enemigoPosY = 6;

        static int mapaNumero = 0;
        public static void IniciarGameLoop()
        {
            
            char[,] mapaActual = Laberinto.DevolverLaberinto(mapaNumero);
            bool alcanzado;

            ConsoleKeyInfo tecla = new ConsoleKeyInfo();
            do
            {
                Console.Clear();
                if (mapaNumero > 2) break;

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
        }

        static void ImprimirPuntos(char[,] mapaActual)
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
        static bool TeAlcanzoElEnemigo()
        {
            return jugadorPosX == enemigoPosX && jugadorPosY == enemigoPosY;
        }
        static void DibujarLaberinto(char[,] laberinto)
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
        static int CantidadDePuntos(char[,] laberinto)
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
        static void DibujarJugador()
        {
            Console.SetCursorPosition(jugadorPosY, jugadorPosX);
            Console.Write('P');
        }
        static void DibujarEnemigo()
        {
            Console.SetCursorPosition(enemigoPosY, enemigoPosX);
            Console.Write('E');
        }
        static void MoverJugador(ConsoleKey tecla, ref char[,] laberinto)
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
                laberinto = Laberinto.SiguienteNivel(mapaNumero);
                jugadorPosX = 1;
                jugadorPosY = 1;
                SpawnearEnemigo(ref laberinto);
            }
        }
        static void SpawnearEnemigo(ref char[,] laberinto)
        {
            Random r = new Random();
            do
            {
                enemigoPosX = r.Next(7, 11);
                enemigoPosY = r.Next(7, 11);
                
            } while (laberinto[enemigoPosX, enemigoPosY] == '#');
        }
        static void MoverEnemigo(ref char[,] laberinto)
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
        static void DibujarPuerta()
        {
            Laberinto.CrearPuerta(mapaNumero);
        }
    }
}
