import socket
import select

# Local IP address for testing
HOST = socket.gethostbyname(socket.gethostname())
#HOST = '192.168.100.53'
PORT = 60000

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    try:
        s.connect((HOST, PORT))
        s.sendall(b'START')

        #Wait up to 5 sec for acknowledge.        
        ready = select.select([s], [], [], 5)
        if ready[0]:
            data = s.recv(1024)
            if str(data, 'UTF-8') == 'ACK':
                print('GERO process started')
                s.close()
            else:
                # Do something if cant receive the acknowledge from server.
                print('Invalid answer.')
        else:
            # Do something if cant receive the acknowledge from server.
            print('There is a problem receiving the answer from the server.')
    except:
        # Do something if cant connect to server
        print('There is a problem connecting to server.')
