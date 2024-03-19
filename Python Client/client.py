import socket
import time

# Local IP address for testing
HOST = '192.168.100.53'
PORT = 50001

print(socket.gethostbyname(socket.gethostname()))

# Client. Connect, sends a message, and read and ACK for acknowledge from server.
with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    # Initial 'command' for testing purposes
    s.sendall(b'INIT')
    while True:
        time.sleep(1)
        # This block continuosly receive 'commands' and send it back (mirror) to server for testing purposes
        # ACK and CLOSE are not sent back
        # CLOSE 'command' terminate the program
        data = s.recv(1024)
        print('Received: {}'.format(str(data, 'UTF-8')))
        if (str(data, 'UTF-8') != 'ACK') and (str(data, 'UTF-8') != 'CLOSE'): 
            s.sendall(data) 
        if str(data, 'UTF-8') == 'CLOSE':  
            s.close()
            break;