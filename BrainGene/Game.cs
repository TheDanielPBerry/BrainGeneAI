using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BrainGene {

    class Game {

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        static IPAddress address = IPAddress.Parse("127.0.0.1");
        IPEndPoint remoteEndPoint = new IPEndPoint(address, 0);
        UdpClient listener;
        public NeatPlayer player;
        public int threadId = 0;
        public int port;
        String buttonOutput = "";

        public Game(NeatPlayer p, int id)
        {
            threadId = id;
            player = p;
            listener = new UdpClient(0);
            port = ((IPEndPoint)listener.Client.LocalEndPoint).Port;
        }


        public double PlayGameToEnd() {
            System.Diagnostics.Process proc = StartGame();

            float fitness = 0;
            byte[] send_buffer;
            byte[] receive_byte_array;
            try {
                String response = "";
                string data = "";
                do
                {
                    receive_byte_array = listener.Receive(ref remoteEndPoint);
                    data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
                    response = ProcessCommand(data);
                    send_buffer = Encoding.ASCII.GetBytes(response);
                    socket.SendTo(send_buffer, remoteEndPoint);
                } while (!data.Contains("WIN") && !data.Contains("DEAD"));
                listener.Close();
                fitness = float.Parse(response);
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            socket.Close();
            proc.Close();
            return fitness;
        }



        public String ProcessCommand(String command)
        {
            String[] args = command.Split(new char[] { '|' });
            switch(args[0])
            {
                case "GET_MOVE":
                    byte[] neuralNetInput = Encoding.ASCII.GetBytes(args[1]);
                    byte[] moves = player.GetMove(neuralNetInput);
                    buttonOutput = string.Format("{0}{1}{2}", moves[0], moves[1], moves[2]);
                    return "RETURN_MOVE|" + buttonOutput + "|";
                case "DEAD":
                case "WIN":
                    Console.WriteLine(threadId + " : " + command);
                    Console.WriteLine(threadId + " Button Output : " + buttonOutput);
                    return args[1];
            }
            return null;
        }


        public System.Diagnostics.Process StartGame()
        {
            string cmd = "-cp C:\\Users\\Cantino\\Documents\\Java\\RogerRabbit\\bin; game.Neat " + port;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:\\Program Files\\Java\\jre1.8.0_111\\bin\\java.exe";
            proc.StartInfo.Arguments = cmd;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.Start();
            return proc;
        }

    }
}
