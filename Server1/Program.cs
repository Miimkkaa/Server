using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using DLLfile;

namespace Server1
{
    class Program
        {
            private static readonly int PORT = 7;
            static void Main(string[] args)
            {
                IPAddress localAddress = IPAddress.Loopback;
                TcpListener serverSocket = new TcpListener(localAddress, PORT);
                serverSocket.Start();
                Console.WriteLine("TCP Server running on port" + PORT);

                while (true)
                {
                    try
                    {
                        TcpClient client = serverSocket.AcceptTcpClient();
                        Console.WriteLine("Incoming client");
                        Task.Run(() => Weight(client));
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Exception will continue");
                    }

                }
            }
            private static void Weight(TcpClient client)
            {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                while (true)
                {
                    string request = reader.ReadLine();
                    if (request == "Bye") break;
                    Console.WriteLine("Request: " + request);

                    string[] values = request.Split(" ");
                    string function = (string)values[0];
                    double x = Double.Parse(values[1]);
                    double response = 0;

                    Converting cv = new Converting();
                    if (function == "ConvertToGrams")
                    {
                        response = cv.ConvertToGrams(x);
                    }
                    else if (function == "ConvertToOunces")
                    {
                        response = cv.ConvertToOunces(x);
                    }

                    Console.WriteLine("Response: " + response);
                    writer.WriteLine(response);
                    writer.Flush();
                }
                client.Close();
            }
        }
    }
