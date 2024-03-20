import socket
import select

# Port for UDP Broadcast
PORT = 60000

with socket.socket(socket.AF_INET, socket.SOCK_DGRAM, socket.IPPROTO_UDP) as s:
    try:
        s.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
        message = b"START"
        s.sendto(message, ('<broadcast>', 60000))

        #Wait up to 10 sec for acknowledge.        
        ready = select.select([s], [], [], 10)
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