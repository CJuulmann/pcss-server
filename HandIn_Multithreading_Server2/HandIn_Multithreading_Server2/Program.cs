using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class HandIn_Multithreading_Server2 {

    public static int totalCount = 0;
    static TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 1234);
    static List<Socket> clientes = new List<Socket>();
    static List<Thread> clientthreads = new List<Thread>();
    static Thread newThread;
    static int myInt = 0;

    public static void Main() {
        tcpListener.Start();
        Console.WriteLine("How many clients are going to connect to this server?:");
        int numberOfClientsYouNeedToConnect = int.Parse(Console.ReadLine());
        for (int i = 0; i < numberOfClientsYouNeedToConnect; i++) {
            newThread = new Thread(new ThreadStart(Listeners));
            newThread.Start();
            newThread.Name = "Thread " + i;
            clientthreads.Add(Thread.CurrentThread);
        }
    }

    static void Listeners() {

        Socket ClientSocket = tcpListener.AcceptSocket();
        clientes.Add(ClientSocket);
        if (ClientSocket.Connected) {
            Console.WriteLine("Client:" + ClientSocket.RemoteEndPoint + " now connected to server.");
            NetworkStream networkStream = new NetworkStream(ClientSocket);
            StreamWriter streamWriter = new StreamWriter(networkStream, Encoding.ASCII) { AutoFlush = true };
            StreamReader streamReader = new StreamReader(networkStream, Encoding.ASCII);

            while (true) {
                string inputLine = streamReader.ReadLine();
                Console.WriteLine("Message recieved by client:" + inputLine);

                if (inputLine == "exit")
                    break;

                if (inputLine == "update") {
                    Console.WriteLine(myInt);
                    streamWriter.WriteLine(myInt);
                }
            }

            streamReader.Close();
            networkStream.Close();
            streamWriter.Close();
        }

        ClientSocket.Close();
        Console.WriteLine("Client(s) disconnected! : Press any key to exit program");
        Console.ReadKey();

    }
}